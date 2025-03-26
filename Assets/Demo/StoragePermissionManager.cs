using UnityEngine;
using UnityEngine.Android;

public class StoragePermissionManager : MonoBehaviour
{
    void Awake()
    {
        RequestStoragePermission();
    }

    void RequestStoragePermission()
    {
        // Check if permissions are already granted
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) &&
            Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Debug.Log("Storage permissions already granted.");
            return;
        }

        // Request normal storage permissions (for Android 10 and below)
        if (AndroidVersion() < 30)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        else
        {
            // For Android 11+ (API 30+), open system settings manually
            OpenManageAllFilesAccessSettings();
        }
    }

    void OpenManageAllFilesAccessSettings()
    {
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent",
                    "android.settings.MANAGE_APP_ALL_FILES_ACCESS_PERMISSION");
                
                string packageName = currentActivity.Call<string>("getPackageName");
                intent.Call<AndroidJavaObject>("setData", AndroidJavaObjectFromString("package:" + packageName));

                currentActivity.Call("startActivity", intent);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening MANAGE_EXTERNAL_STORAGE settings: " + e.Message);
        }
    }

    int AndroidVersion()
    {
        return int.Parse(SystemInfo.operatingSystem.Split(' ')[1].Split('.')[0]);
    }

    AndroidJavaObject AndroidJavaObjectFromString(string uriString)
    {
        using (AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri"))
        {
            return uriClass.CallStatic<AndroidJavaObject>("parse", uriString);
        }
    }
}