using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class AppExpiryManager : MonoBehaviour
{
 private const string ExpiryKey = "secure_app_expiry"; // Secure Key
    private const int ExpiryDays = 1; // Expiry duration in days
    private const string TimeApiUrl = "https://www.timeapi.io/api/Time/current/zone?timeZone=UTC"; // Online time API

    void Start()
    {
        CheckAppExpiry();
    }

    private async void CheckAppExpiry()
    {
        string storedDate = SecureStorage.GetString(ExpiryKey);
        DateTime firstLaunchDate;

        if (!string.IsNullOrEmpty(storedDate) && DateTime.TryParse(storedDate, out firstLaunchDate))
        {
            Debug.Log("Loaded expiry date from secure storage: " + firstLaunchDate);
        }
        else
        {
            firstLaunchDate = await FetchOnlineTime() ?? DateTime.UtcNow;
            SecureStorage.SetString(ExpiryKey, firstLaunchDate.ToString());
            Debug.Log("New expiry date saved securely: " + firstLaunchDate);
        }

        DateTime currentDate = await FetchOnlineTime() ?? DateTime.UtcNow;
        DateTime expiryDate = firstLaunchDate.AddDays(ExpiryDays);

        if (currentDate >= expiryDate)
        {
            ShowExpiryMessage();
        }
    }

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
                Debug.LogWarning($"Failed to fetch time from the internet: {ex.Message}");
                return null;
            }
        }
    }

    private void ShowExpiryMessage()
    {
        Debug.Log("This app has expired. Please update to continue.");
        Application.Quit();
    }

    [Serializable]
    private class TimeApiResponse
    {
        public string dateTime;
    }
    
}