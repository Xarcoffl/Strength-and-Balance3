using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MODEVIEWEYES : MonoBehaviour
{
    private int vieweyes;
    public GameObject eyesopen;
    public GameObject eyeclose;
    // Start is called before the first frame update
    void Start()
    {
        vieweyes = PlayerPrefs.GetInt("eyes");
        if (vieweyes == 1) //open
        {
            eyesopen.SetActive(true);
            eyeclose.SetActive(false);
        }
        else // closed
        {
            eyeclose.SetActive(true);
            eyesopen.SetActive(false);
        }
    }

    // Update is called once per frame
    
}
