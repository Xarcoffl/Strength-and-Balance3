using UnityEngine;
using UnityEngine.UI;

public class SliderSnap : MonoBehaviour
{
    public Slider slider;
    public float[] snapValues; // Array of values to snap to
    public bool valuechange = false;

    public GameObject LevelLoader;

    void Start()
    {
        if (slider != null)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);

        }
    }

    void OnSliderValueChanged(float value)
    {
        float closest = FindClosestValue(value);
        slider.value = closest;
        

    
        PlayerPrefs.SetFloat("timervalue", closest);
        PlayerPrefs.Save();
        if( closest != 0)
        {
            valuechange = true;
        }
        else
        {
            valuechange = false;
        }
        
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

    public void ChangeScene(string scenename)
    {
        if(valuechange == true)
        {
            customlevelloader otherScript = LevelLoader.GetComponent<customlevelloader>();
            otherScript.Switchscene(scenename);
            
        }
    }
}
