using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sensitivitysnap : MonoBehaviour
{
    public Slider slider;
    public float[] snapValues; // Array of values to snap to
    public bool valuechange = false;

    void Start()
    {
        if (slider != null)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);

        }
    }

    public void OnSliderValueChanged(float value)
    {
        float closest = FindClosestValue(value);
        slider.value = closest;


        PlayerPrefs.SetFloat("Sensitivity", closest);
        PlayerPrefs.Save();
        valuechange = true;
    }

    float FindClosestValue(float value)
    {
        float closest = snapValues[0];
        float minDistance = Mathf.Abs(value - closest);

        foreach (float snapValue in snapValues)
        {
            float distance = Mathf.Abs(value - snapValue);
            if (distance < minDistance)
            {
                closest = snapValue;
                minDistance = distance;
            }
        }

        return closest;
    }
}
