using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelshow : MonoBehaviour
{
    string level;
    public Text Leveltet;
    // Start is called before the first frame update
    void Start()
    {
        level = PlayerPrefs.GetString("Levelv");
        Leveltet.text = level;
    }

    // Update is called once per frame
    
}
