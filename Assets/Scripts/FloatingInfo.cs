using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingInfo : MonoBehaviour
{
    
    public void messageinfo(string message)
    {
        SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);
    }

}
