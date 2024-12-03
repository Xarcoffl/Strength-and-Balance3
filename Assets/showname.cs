using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class showname : MonoBehaviour
{
    public Text Name;
    public Text Age;
    public Text Gender;
    public Text Patientid;
    public Text Date;
    // Start is called before the first frame update
    void Start()
    {
        Name.text = PlayerPrefs.GetString("regname");
        Patientid.text = PlayerPrefs.GetString("regid");
        Age.text = PlayerPrefs.GetString("regage");
        Gender.text = PlayerPrefs.GetString("reggender");
        DateTime currentDate = DateTime.Now;
        Date.text = currentDate.ToString("dd/MM/yyyy");// "dd-MM-yyyy");
    }


}
