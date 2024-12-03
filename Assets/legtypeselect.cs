using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class legtypeselect : MonoBehaviour
{
    // Start is called before the first frame update
    
    bool pose = false;
    public Button NextButton;
    void Start()
    {
        if (NextButton == null)
        {
            NextButton = GetComponent<Button>();
        }
        
        NextButton.interactable = false;
    }

    public void rightleg()
    {
        PlayerPrefs.SetInt("oneleg", 1);

        // Ensure PlayerPrefs is saved
        PlayerPrefs.Save();
        pose = true;
        EnableButton();

    }
    public void leftleg()
    {
        PlayerPrefs.SetInt("oneleg", 0);

        // Ensure PlayerPrefs is saved
        PlayerPrefs.Save();
        pose = true;
        EnableButton();
    }

    public void EnableButton()
    {
        if(pose == true)
        {

            NextButton.interactable = true;
            
        }

    }
}
