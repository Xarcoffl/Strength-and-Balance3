using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class newgyroscript : MonoBehaviour
{
    Vector3 rot; // Corrected the type to Vector3
    public Text gyroText;
    public float rotationSpeed = 10f; // Adjust the rotation speed as needed

    // Start is called before the first frame update
    void Start()
    {
        rot = Vector3.zero; // Corrected the type to Vector3
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the gyroscope rotation
        rot.y = -2 * Input.gyro.rotationRateUnbiased.y;
        rot.x = -2 * Input.gyro.rotationRateUnbiased.x;// Corrected the case to Input
        Debug.Log(Input.gyro.attitude);
        transform.Rotate(rot);

        // Extract the rotation angles for x and y axis
    }
}
