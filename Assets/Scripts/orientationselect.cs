using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class orientationselect : MonoBehaviour
{
    bool orientation = false;
    bool eyestate = false;
    bool legpose = false;

    public Button NextButton;

    bool vertical = false;
    bool horizontal = false;
    bool eyeopen = false;
    bool eyeclose = false;
    bool bothlegs = false;
    bool singlelegs = false;

    void Start()
    {
        if (NextButton == null)
        {
            NextButton = GetComponent<Button>();
        }

        NextButton.interactable = false;
    }

    public void verticalselect()
    {
        PlayerPrefs.SetInt("Orientation", 1);

        // Ensure PlayerPrefs is saved
        PlayerPrefs.Save();
        vertical = true;
        EnableButton();
    }
    public void horizontalselect()
    {
        PlayerPrefs.SetInt("Orientation", 0);

        // Ensure PlayerPrefs is saved
        PlayerPrefs.Save();
        horizontal = true;
        EnableButton();
    }
    public void Eyesopen()
    {
        PlayerPrefs.SetInt("eyes", 1);

        // Ensure PlayerPrefs is saved
        PlayerPrefs.Save();
        eyeopen = true;
        EnableButton();
    }
    public void Eyesclosed()
    {
        PlayerPrefs.SetInt("eyes", 0);

        // Ensure PlayerPrefs is saved
        PlayerPrefs.Save();
        eyeclose = true;
        EnableButton();
    }
    public void bothleg()
    {
        PlayerPrefs.SetInt("legtype", 10);

        // Ensure PlayerPrefs is saved
        PlayerPrefs.Save();
        bothlegs = true;
        EnableButton();
    }
    public void singleleg()
    {
        PlayerPrefs.SetInt("legtype", 11);

        // Ensure PlayerPrefs is saved
        PlayerPrefs.Save();
        singlelegs = true;
        EnableButton();
    }

    public void EnableButton()
    {
        if(vertical || horizontal)
        {
            orientation = true;
        }
        if (eyeopen || eyeclose)
        {
            eyestate = true;
        }
        if (bothlegs || singlelegs)
        {
            legpose = true;
        }
        if (orientation && eyestate && legpose)
        {

            NextButton.interactable = true;

        }

    }
}