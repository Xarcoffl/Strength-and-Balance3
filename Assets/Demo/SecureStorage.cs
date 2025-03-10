using UnityEngine;
using System;
using System.IO;
using System.Text;

public static class SecureStorage
{
    private static string directoryPath = "/storage/emulated/0/Download/Pain And Stroke/.SecureData"; // Internal storage
    private static string filePath = directoryPath + "/strokeCalibration.txt"; // Expiry file
    private static string deviceIdKey = "device_id"; // Unique device identifier key

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
                    string storedExpiry = data[0];
                    string storedDeviceId = data[1];

                    // Check if the device ID is the same
                    if (storedDeviceId == SystemInfo.deviceUniqueIdentifier)
                    {
                        return storedExpiry;
                    }
                    else
                    {
                        Debug.LogWarning("[SecureStorage] Device ID mismatch! Possible reinstall attempt.");
                        return "EXPIRED"; // Force expiry if device changes
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
        return "";
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

    /*private static string EncryptData(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        return Convert.ToBase64String(bytes);
    }
    
    private static string DecryptData(string data)
    {
        byte[] bytes = Convert.FromBase64String(data);
        return Encoding.UTF8.GetString(bytes);
    }*/

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
}