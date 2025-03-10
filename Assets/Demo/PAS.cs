using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class PAS : MonoBehaviour
{
    
    private const float ExpiryDays = 0.00347222222f; // Supports hours (e.g., 0.5 = 12 hours) 5 mins
    private const string TimeApiUrl = "https://www.timeapi.io/api/Time/current/zone?timeZone=UTC";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    private class TimeApiResponse
    {
        public string dateTime;
    }
}
