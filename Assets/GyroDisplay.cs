using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GyroDisplay : MonoBehaviour
{
    public TextMeshProUGUI gyroText;

    void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        Debug.Log(Input.gyro.attitude);
        transform.rotation = Input.gyro.attitude;
        // Check if the gyroscope is available
        if (SystemInfo.supportsGyroscope)
        {
            // Get the gyroscope rotation rate

            // Display gyro information in the UI text
            if (gyroText != null)
            {
                gyroText.text = "Gyro Rotation Rate:\n" +
                                "X: " + transform.rotation.x + "\n" +
                                "Y: " + transform.rotation.y + "\n" +
                                "Z: " + transform.rotation.z;
            }
            
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device.");
        }
    }
}
