using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public SecondsValue timerinstance;
    float timervalue;
    public string Scenename;
   
     // Set the countdown time in seconds
    public Text timerText; // Reference to the Text component
    public customlevelloader levelloaderinstance;


void Start()
    {
        if (PlayerPrefs.HasKey("timervalue"))
        {
            timervalue = PlayerPrefs.GetFloat("timervalue");
        }
        // Initialize the timerText with the initial time
        UpdateTimerText();
    }

    void Update()
    {
        // Check if there is time remaining
        if (timervalue > 0)
        {
            // Reduce the time remaining by the time since last frame
            timervalue -= Time.deltaTime;

            // Update the text component to show the remaining time
            UpdateTimerText();
        }
        else
        {
            // When the timer runs out, make sure it doesn't go negative
            timervalue = 0;
            UpdateTimerText();

            // Optionally, add code to handle what happens when the timer finishes
            levelloaderinstance.Switchscene(Scenename);
            // For example, triggering an event or stopping some process
        }
    }

    void UpdateTimerText()
    {
        // Format the time as minutes and seconds
        int minutes = Mathf.FloorToInt(timervalue / 60);
        int seconds = Mathf.FloorToInt(timervalue % 60);

        // Update the text component
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}