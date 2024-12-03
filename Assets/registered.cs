using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class registered : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("IsRegistered", 1); // 1 means registered
        PlayerPrefs.Save();
    }

    // Update is called once per frame
}