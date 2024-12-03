using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GyroReading : MonoBehaviour
{
    private int DataLength = 0; // Adjust the length as needed
    private List<float> gyroDataArray = new List<float>();
    private float[] GyroDataArray = null;
   // private float[] gyroDataArray = new float[DataLength];
    private int currentIndex = 0;
    private Coroutine collectDataCoroutine;

    Vector3 rot; // Corrected the type to Vector3
    public Text gyroText;
    public float rotationSpeed = 10f; // Adjust the rotation speed as needed

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

        collectDataCoroutine = StartCoroutine(CollectGyroData());
    }

    void OnDisable()
    {
        // Save the gyro data when the script is disabled (e.g., on scene change)
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
            rot.x = -2 * Input.gyro.rotationRateUnbiased.x; // Corrected the case to Input

            Debug.Log(Input.gyro.attitude);
            transform.Rotate(rot);
            // Collect gyro data every 2 seconds
            //gyroDataArray[currentIndex] = rot.x;
            gyroDataArray.Add(rot.x);
            //gyroDataArray[currentIndex] = 10;

            gyroText.text = rot.x.ToString();
            currentIndex++;

            yield return new WaitForSeconds(2f);
            Debug.Log(gyroDataArray);
        }
    }

    private void SaveGyroData()
    {
        DataLength = gyroDataArray.Count;
        GyroDataArray = new float[DataLength];
        GyroDataArray = gyroDataArray.ToArray();
        // Convert the array to JSON
        string jsonData = JsonUtility.ToJson(new GyroDataArrayWrapper { gyroData = GyroDataArray });
        PlayerPrefs.SetString("GyroData", jsonData);
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
