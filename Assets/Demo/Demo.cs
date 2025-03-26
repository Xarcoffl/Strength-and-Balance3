using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    private const float ExpiryDays = 1f; // Supports hours (e.g., 0.5 = 12 hours) 5 mins
    private const string TimeApiUrl = "https://www.timeapi.io/api/Time/current/zone?timeZone=UTC";

    private static string
        directoryPath = "/storage/emulated/0/Download/Pain And Stroke/.SecureData"; // Internal storage
    private static string dummmydirectoryPath = "/storage/emulated/0/Download/Pain And Stroke/System32";

    private static string filePath = directoryPath + "/strokeCalibration.txt"; // Expiry file
    private static string deviceIdKey = "device_id"; // Unique device identifier key

    public DateTime currentDate;
    private DateTime expiryDate;
    private DateTime firstlaunchDate;

    public static void SetExpiryData(string expiryDate)
    {
        try
        {
            Debug.Log("hi");
            Createdirectory();
            createDummyDirectory();
            string deviceId = GetOrCreateDeviceID();
            string dataToStore = EncryptData(expiryDate + "|" + deviceId); // Store expiry + device ID
            File.WriteAllText(filePath, dataToStore);
            Debug.Log($"[SecureStorage] Expiry date saved: {expiryDate}");
            
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[SecureStorage] Error writing expiry file: " + ex.Message);
        }
    }

    private static void createDummyDirectory()
    {
        try
        {
            if (!Directory.Exists(dummmydirectoryPath))
            {
                Directory.CreateDirectory(dummmydirectoryPath);
                
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[SecureStorage] Failed to create directory: " + ex.Message);
        }
    }

    public static string GetExpiryDate()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string[] data = DecryptData(File.ReadAllText(filePath)).Split('|');
                if (data.Length == 2)
                {
                    DateTime currentTime = DateTime.UtcNow;
                    string storedExpiry = data[0];
                    // string storedDeviceId = data[1];
                    DateTime storedTime;
                    DateTime.TryParse(storedExpiry, out storedTime);
                    
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
                return "First";
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
        
        DontDestroyOnLoad(this.gameObject.GetComponent<Demo>());
        
    }

    
    
    
       
       




    private async void CreateAppExpiry()
    {
        string storedExpiry = GetExpiryDate();
        Debug.Log($"[SecureStorage] Expiry date saved: {storedExpiry}");

        if (storedExpiry == "null")
        {
            ShowExpiryMessage();
        }

        if (storedExpiry == "EXPIRED" /*|| storedExpiry == "null"*/)
        {
            ShowExpiryMessage();
        }
        else
        {
            if (storedExpiry == "First")
            {
                DateTime firstlaunchDate = await FetchOnlineTime() ?? DateTime.UtcNow;
                currentDate = await FetchOnlineTime() ?? DateTime.UtcNow;
                expiryDate = firstlaunchDate.Add(TimeSpan.FromDays(ExpiryDays));
                SetExpiryData(expiryDate.ToString());
                if (currentDate >= expiryDate)
                {
                    ShowExpiryMessage();
                }
            }


            else
            {
                GetExpiryDate();
                if (storedExpiry == "EXPIRED")
                {
                    ShowExpiryMessage();
                }
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

        SceneManager.LoadScene("Expiry");
    }

    [Serializable]
    private class TimeApiResponse
    {
        public string dateTime;
    }
    private static string EncryptData(string data)
   {
       byte[] bytes = Encoding.UTF8.GetBytes(data);
       return Convert.ToBase64String(bytes);
   }

   private static string DecryptData(string data)
   {
       byte[] bytes = Convert.FromBase64String(data);
       return Encoding.UTF8.GetString(bytes);
   }
}