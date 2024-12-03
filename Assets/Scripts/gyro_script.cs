using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class gyro_script : MonoBehaviour
{ 
    public Text gyroText;

    // Start is called before the first frame update
    void Start()
    {
      Input.gyro.enabled=true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.gyro.attitude);
        
    }
}
