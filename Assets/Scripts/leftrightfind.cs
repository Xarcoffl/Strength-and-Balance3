using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class leftrightfind : MonoBehaviour
{
    private float[] gyroDataArray;
    int positiveCount = 0;
    int negativeCount = 0;
    public GameObject Left;
    public GameObject Right;
    // Start is called before the first frame update
    private void Awake()
    {
        string jsonData = PlayerPrefs.GetString("GyroData");
        if (!string.IsNullOrEmpty(jsonData))
        {
            GyroDataArrayWrapper dataWrapper = JsonUtility.FromJson<GyroDataArrayWrapper>(jsonData);
            gyroDataArray = dataWrapper.gyroData;
            Debug.LogWarning("gyro data found in leftright");
            foreach (int number in gyroDataArray)
            {
                if (number > 0)
                {
                    positiveCount++;
                }
                else if (number < 0)
                {
                    negativeCount++;
                }
            }
            if (positiveCount > negativeCount)
            {
                Right.SetActive(true);
                Left.SetActive(false);
            }
            else
            {
                Left.SetActive(true);
                Right.SetActive(false);
            }

        }
        else
        {
            Debug.LogWarning("No gyro data found in leftright.");
        }
    }

    [System.Serializable]
    private class GyroDataArrayWrapper
    {
        public float[] gyroData;
    }
    // Update is called once per frame

}
