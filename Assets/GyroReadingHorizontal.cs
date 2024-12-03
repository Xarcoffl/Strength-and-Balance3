using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GyroReadingHorizontal : MonoBehaviour
{
    private int DataLength = 0; // Adjust the length as needed
    private List<float> gyroDataArray = new List<float>();
    private float[] GyroDataArrayh = null;
    private int currentIndex = 0;
    private Coroutine collectDataCoroutine;

    Vector3 rot; // Corrected the type to Vector3
    public Text gyroText;
    public float rotationSpeed = 10f; // Adjust the rotation speed as needed
    private float initialPosition = 0f; // New variable to store the initial position
    private float currentPosition = 0f; // New variable to store the current position

    // Start is called before the first frame update
    void Start()
    {
        rot = Vector3.zero; // Corrected the type to Vector3
        Input.gyro.enabled = true;

        // Ensure gyroText is assigned in the inspector
        if (gyroText == null)
        {
            Debug.LogError("GyroText is not assigned in the inspector.");
            return;
        }

        currentPosition = initialPosition; // Set the initial position as the current position
        collectDataCoroutine = StartCoroutine(CollectGyroData());
    }

    void OnDisable()
    {
        
        SaveGyroData();
        if (collectDataCoroutine != null)
        {
            StopCoroutine(collectDataCoroutine);
        }
    }

    private IEnumerator CollectGyroData()
    {
        while (true)
        {
            float gyroValue = -2 * Input.gyro.rotationRateUnbiased.y; // Corrected the case to Input
            currentPosition += gyroValue; // Update the current position based on the gyroscope value
            rot.y = currentPosition;

            transform.Rotate(rot);

            gyroDataArray.Add(currentPosition); // Add the current position to the gyro data array
            gyroText.text = currentPosition.ToString("F3"); // Display the current position with 3 decimal places
            currentIndex++;

            yield return new WaitForSeconds(2f);
        }
    }

    private void SaveGyroData()
    {
        DataLength = gyroDataArray.Count;
        GyroDataArrayh = new float[DataLength];
        GyroDataArrayh = gyroDataArray.ToArray();
        // Convert the array to JSON
        string jsonData = JsonUtility.ToJson(new GyroDataArrayWrapper { gyroData = GyroDataArrayh });
        PlayerPrefs.SetString("GyroDatah", jsonData);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class GyroDataArrayWrapper
    {
        public float[] gyroData;
    }

    // Update is called once per frame
    void Update()
    {
        // CollectGyroData(); // This should not be called here as it is already handled by the coroutine
    }
}
