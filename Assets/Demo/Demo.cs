using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{

  
    
    private const float ExpiryDays = 0.00347222222f; // Supports hours (e.g., 0.5 = 12 hours) 5 mins
    private const string TimeApiUrl = "https://www.timeapi.io/api/Time/current/zone?timeZone=UTC";
    
    private static string directoryPath = "/storage/emulated/0/Download/Pain And Stroke/.SecureData"; // Internal storage
    private static string filePath = directoryPath + "/strokeCalibration.txt"; // Expiry file
    private static string deviceIdKey = "device_id"; // Unique device identifier key

    public DateTime currentDate;
    private DateTime expiryDate;

    public static void SetExpiryData(string expiryDate)
    {
        try
        {
            Createdirectory();
            string deviceId = GetOrCreateDeviceID();
            string dataToStore = (expiryDate + "|" + deviceId); // Store expiry + device ID
            File.WriteAllText(filePath, dataToStore);
            Debug.Log($"[SecureStorage] Expiry date saved: {expiryDate}");
           
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[SecureStorage] Error writing expiry file: " + ex.Message);
        }
    }

    public static string GetExpiryDate()
    {
       
        
        try
        {
            if (File.Exists(filePath))
            {
                string[] data = (File.ReadAllText(filePath)).Split('|');
                if (data.Length == 2)
                {
                    DateTime currentTime = DateTime.UtcNow;
                    string storedExpiry = data[0];
                    // string storedDeviceId = data[1];
                    DateTime storedTime;
                    DateTime.TryParse(storedExpiry, out storedTime);
                    Debug.LogWarning("[SecureStorage] Device ID mismatch! Possible reinstall attempt.");
                    if (currentTime >= storedTime)
                    {
                        
                        return "EXPIRED";
                    }
                    else
                    {
                        return "NOT EXPIRED";
                    }
                    
                }
            }
            else
            {
                Createdirectory();

            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[SecureStorage] Error reading expiry file: " + ex.Message);
        }
        return "null";
    }

    private static void Createdirectory()
    {
        try
        {
            
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Debug.Log("[SecureStorage] Created secure directory.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[SecureStorage] Failed to create directory: " + ex.Message);
        }
    }
    
    private static string GetOrCreateDeviceID()
    {
        string storedDeviceId = PlayerPrefs.GetString(deviceIdKey, "");
        if (string.IsNullOrEmpty(storedDeviceId))
        {
            storedDeviceId = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString(deviceIdKey, storedDeviceId);
            PlayerPrefs.Save();
        }
        return storedDeviceId;
    }
    
    
    void Start()
    {
        CreateAppExpiry();
    }
    
   

    private async void CreateAppExpiry()
    {
        string storedExpiry = GetExpiryDate();
        Debug.Log($"[SecureStorage] Expiry date saved: {storedExpiry}");

        if (storedExpiry == "EXPIRED")
        {
            ShowExpiryMessage();
            
        }
        else
        {
            DateTime firstLaunchDate;

            if (!File.Exists(filePath) && (storedExpiry == null) && DateTime.TryParse(storedExpiry, out firstLaunchDate))
            {
                firstLaunchDate = await FetchOnlineTime() ?? DateTime.UtcNow;
              
            }
            else
            {
                
                GetExpiryDate();
                if (storedExpiry == "EXPIRED")
                {
                    ShowExpiryMessage();
            
                }

            }
            firstLaunchDate = await FetchOnlineTime() ?? DateTime.UtcNow;
            currentDate = await FetchOnlineTime() ?? DateTime.UtcNow;
            expiryDate = firstLaunchDate.Add(TimeSpan.FromDays(ExpiryDays));
            SetExpiryData(expiryDate.ToString());
            Debug.Log($"[Expiry Check] Current Date: {currentDate}, Expiry Date: {expiryDate}");
            if (currentDate >= expiryDate)
            {
                ShowExpiryMessage();
            }

            // DateTime currentDate = await FetchOnlineTime() ?? DateTime.UtcNow;
            // DateTime expiryDate = firstLaunchDate.Add(TimeSpan.FromDays(ExpiryDays));
            // SetExpiryData(expiryDate.ToString());
            
        }


    }
    // private async void CheckAppAlreadyExpired()
    // {
    //     
    //     string EstoredExpiry = GetExpiryDate();
    //     DateTime EcurrentDate =await FetchOnlineTime() ?? DateTime.UtcNow;
    //     DateTime EexpiryDate;
    //     if (!string.IsNullOrEmpty(EstoredExpiry) && DateTime.TryParse(EstoredExpiry, out EexpiryDate))
    //     {
    //         if (EcurrentDate >= EexpiryDate)
    //         {
    //             ShowExpiryMessage();
    //         }
    //         
    //     }
    //    
    //     else
    //     {
    //         CreateAppExpiry();
    //     }
    // }
    private async Task<DateTime?> FetchOnlineTime()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(TimeApiUrl);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var timeData = JsonUtility.FromJson<TimeApiResponse>(json);
                return DateTime.Parse(timeData.dateTime);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[Time Sync] Failed to fetch time: " + ex.Message);
                return null;
            }
        }
    }

    private void ShowExpiryMessage()
    {
        Debug.Log("[Expiry Check] This app has expired. Please update to continue.");
        Application.Quit();
    }

    [Serializable]
    private class TimeApiResponse
    {
        public string dateTime;
    }
}
