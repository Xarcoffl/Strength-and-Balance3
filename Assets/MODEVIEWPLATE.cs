using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MODEVIEWPLATE : MonoBehaviour
{
    // Start is called before the first frame update
    private int viewplate;
    public GameObject vertical;
    public GameObject horizontal;
    // Start is called before the first frame update
    void Start()
    {
        viewplate = PlayerPrefs.GetInt("Orientation");
        if (viewplate == 1) //open
        {
            vertical.SetActive(true);
            horizontal.SetActive(false);
        }
        else // closed
        {
            horizontal.SetActive(true);
            vertical.SetActive(false);
        }
    }
}
