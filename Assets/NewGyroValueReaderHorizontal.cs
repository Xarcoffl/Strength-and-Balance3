using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewGyroValueReaderHorizontal : MonoBehaviour
{
    private int DataLength = 0; // Adjust the length as needed
    private int NewDataLength = 0;
    private List<float> gyroDataArray = new List<float>();
    private List<float> NeededgyroDataArray = new List<float>();
    private float[] GyroDataArrayh = null;
    private Coroutine collectDataCoroutine;

    Vector3 rot; // Corrected the type to Vector3
    public Text gyroText;
    public float rotationSpeed = 10f; // Adjust the rotation speed as needed
    private float initialPosition = 0f; // New variable to store the initial position
    private float currentPosition = 0f; // New variable to store the current position

    private float angleGyro = 0f;
    private float previousTime;
    private float alpha = 0.6f; // Complementary filter constant

    float Sensitive;
    float sensitivityval;


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

        Sensitive = PlayerPrefs.GetFloat("Sensitivity");

        currentPosition = initialPosition; // Set the initial position as the current position
        previousTime = Time.time;

        Vector3 accel = Input.acceleration;
        angleGyro = Mathf.Atan2(accel.x, Mathf.Sqrt(accel.y * accel.y + accel.z * accel.z));// * Mathf.Rad2Deg;
    }

    void OnDisable()
    {

        SaveGyroData();
        
    }

    private void CollectGyroData()
    {
        float currentTime = Time.time;
        float dt = currentTime - previousTime;
        previousTime = currentTime;


        //float gyroValue = 2 * Input.gyro.rotationRateUnbiased.y ; // Corrected the case to Input
        Vector3 gyroValue = Input.gyro.rotationRateUnbiased; // Gyroscope data in radians per second


        angleGyro += gyroValue.y *dt;// * Mathf.Rad2Deg * dt;
        Vector3 accel = Input.acceleration;
        float angleAccel = Mathf.Atan2(accel.x, Mathf.Sqrt(accel.y * accel.y + accel.z * accel.z)); //* Mathf.Rad2Deg;
        float angle = alpha * angleGyro + (1 - alpha) * angleAccel;
        float finalangle = angleAccel * Sensitivity(Sensitive);
        //currentPosition += gyroValue; // Update the current position based on the gyroscope value
        gyroDataArray.Add(finalangle);
        //gyroDataArray.Add(currentPosition); // Add the current position to the gyro data array
        //gyroText.text = currentPosition.ToString("F3"); 
        gyroText.text = finalangle.ToString("F3");

    }

    private void NeededGyroArrayValues()
    {    
        int loopindex = 0;
        DataLength = gyroDataArray.Count;
        int factor = 0;

        if (PlayerPrefs.GetFloat("timervalue") == 30)
        {
            factor = DataLength / 20;
        }
        if (PlayerPrefs.GetFloat("timervalue") == 60)
        {
            factor = DataLength / 50;
        }
        if (PlayerPrefs.GetFloat("timervalue") == 90)
        {
            factor = DataLength / 66;
        }
        if (PlayerPrefs.GetFloat("timervalue") == 120)
        {
            factor = DataLength / 98;
        }
        if (PlayerPrefs.GetFloat("timervalue") == 150)
        {
            factor = DataLength / 137;
        }

        for (int i = 0; i < DataLength; i++)
        {
            if (i == loopindex)
            {
                NeededgyroDataArray.Add(gyroDataArray[i]);
                loopindex += factor;
            }
        }
    }

    private void SaveGyroData()
    {
        //NeededGyroArrayValues();
        NewDataLength = gyroDataArray.Count;
        GyroDataArrayh = new float[NewDataLength];
        GyroDataArrayh = gyroDataArray.ToArray();
        // Convert the array to JSON
        for (int k = 0; k < GyroDataArrayh.Length; k++)
        {
            Debug.Log(GyroDataArrayh[k]);
        }
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
        CollectGyroData(); // This should not be called here as it is already handled by the coroutine
    }

    private float Sensitivity(float sensitive)
    {

        if (sensitive == 0)
        {
            sensitivityval = 1;
        }
        if (sensitive == 1)
        {
            sensitivityval = 10;
        }
        if (sensitive == 2)
        {
            sensitivityval = 20;
        }
        if (sensitive == 3)
        {
            sensitivityval = 30;
        }
        if (sensitive == 4)
        {
            sensitivityval = 40;
        }
        if (sensitive == 5)
        {
            sensitivityval = 50;
        }
        return sensitivityval;
    }
}