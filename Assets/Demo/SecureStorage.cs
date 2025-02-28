using UnityEngine;

public static class SecureStorage
{
    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key, "");
    }
}