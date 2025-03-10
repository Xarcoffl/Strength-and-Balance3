using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    private const float ExpiryDays = 0.00347222222f; // Supports hours (e.g., 0.5 = 12 hours) 5 mins
    private const string TimeApiUrl = "https://www.timeapi.io/api/Time/current/zone?timeZone=UTC";

    void Start()
    {
        CheckAppExpiry();
    }

    private async void CheckAppExpiry()
    {
        string storedExpiry = SecureStorage.GetExpiryDate();

        if (storedExpiry == "EXPIRED")
        {
            ShowExpiryMessage();
            
        }
        else
        {
            DateTime firstLaunchDate;

            if (!string.IsNullOrEmpty(storedExpiry) && DateTime.TryParse(storedExpiry, out firstLaunchDate))
            {
                Debug.Log($"[Expiry Check] Loaded Expiry Date: {firstLaunchDate}");
            }
            else
            {
                firstLaunchDate = await FetchOnlineTime() ?? DateTime.UtcNow;
                
                Debug.Log($"[Expiry Check] New Expiry Date Stored: {firstLaunchDate}");
            }

            DateTime currentDate = await FetchOnlineTime() ?? DateTime.UtcNow;
            DateTime expiryDate = firstLaunchDate.Add(TimeSpan.FromDays(ExpiryDays));
            SecureStorage.SetExpiryData(expiryDate.ToString());

            Debug.Log($"[Expiry Check] Current Date: {currentDate}, Expiry Date: {expiryDate}");

            if (currentDate >= expiryDate)
            {
                ShowExpiryMessage();
            }
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