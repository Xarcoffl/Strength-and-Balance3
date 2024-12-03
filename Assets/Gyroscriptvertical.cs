using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gyroscriptvertical : MonoBehaviour
{
    Vector3 rot; // Corrected the type to Vector3
    public Text gyroText;
    public float rotationSpeed = 10f; // Adjust the rotation speed as needed
    float sensitive;
    float sensitivityval;
    public Text Leveldisplay;

    // Start is called before the first frame update
    void Start()
    {
        rot = Vector3.zero; // Corrected the type to Vector3
        Input.gyro.enabled = true;
        sensitive = PlayerPrefs.GetFloat("Sensitivity");
        if( sensitive == 0)
        {
            sensitivityval = 2;
        }
        if (sensitive == 1)
        {
            sensitivityval = 4;
        }
        if (sensitive == 2)
        {
            sensitivityval = 6;
        }
        if (sensitive == 3)
        {
            sensitivityval = 8;
        }
        if (sensitive == 4)
        {
            sensitivityval = 10;
        }
        if (sensitive == 5)
        {
            sensitivityval = 12;
        }
        string formattedTime = returnsensitivity(sensitive);
        Leveldisplay.text = formattedTime;
        PlayerPrefs.SetString("Levelv", formattedTime);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the gyroscope rotation
        rot.x = -(sensitivityval) * Input.gyro.rotationRateUnbiased.x;

       // Debug.Log(Input.gyro.attitude);
        transform.Rotate(rot);

        // Extract the rotation angles for x and y axis
    }
    public string returnsensitivity(float sensitivereturn)
    {
        return string.Format("Level {0}", sensitivereturn );
    }
}
