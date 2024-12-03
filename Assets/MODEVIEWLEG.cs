using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MODEVIEWLEG : MonoBehaviour
{
    private int viewleg;
    public GameObject rightleg;
    public GameObject leftleg;
    public GameObject bothleg;
    // Start is called before the first frame update
    void Start()
    {
        viewleg = PlayerPrefs.GetInt("oneleg");
        if (viewleg == 1) //open
        {
            rightleg.SetActive(true);
            leftleg.SetActive(false);
            bothleg.SetActive(false);
        }
        
        if (viewleg == 0)
        {
            leftleg.SetActive(true);
            rightleg.SetActive(false);
            bothleg.SetActive(false);
        }

        if (viewleg == 10) //open
        {
            bothleg.SetActive(true);
            leftleg.SetActive(false);
            rightleg.SetActive(false);
        }
    }
}
