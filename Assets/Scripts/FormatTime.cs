using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormatTime : MonoBehaviour
{
    public Text BalanceTime;
    float value;
    void Start()
    {
        value = PlayerPrefs.GetFloat("timervalue");
        int intValue = (int)value;
        string formattedTime = FormatTimed(intValue);

        BalanceTime.text = formattedTime;
        //Debug.Log(formattedTime);
    }
    public string FormatTimed(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        if (minutes > 0)
        {
            return string.Format("{0} min {1} sec", minutes, seconds);
        }
        else
        {
            return string.Format("{0} sec", seconds);
        }
    }

    
}
