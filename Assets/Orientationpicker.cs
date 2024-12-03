using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientationpicker : MonoBehaviour
{
    private customlevelloader customlevelloaderInstance;
    public GameObject Sensitivitymapobj;
    private Sensitivitysnap Sensitivitymap;
    // Start is called before the first frame update
    public void orientationselect()
    {
        if (PlayerPrefs.GetInt("Orientation") == 1)
        {
            GameObject obj = GameObject.Find("Level Loader");
            customlevelloaderInstance = obj.GetComponent<customlevelloader>();
            Sensitivitymap = Sensitivitymapobj.GetComponent<Sensitivitysnap>();


            if (customlevelloaderInstance != null)
            {
                if(Sensitivitymap.valuechange == true)
                {
                    customlevelloaderInstance.Switchscene("Start Analysis Vertical");
                }

                
            }
            else
            {
                Debug.LogError("customlevelloader instance not found 1.");
            }
        }

        if (PlayerPrefs.GetInt("Orientation") == 0)
        {
            GameObject obj = GameObject.Find("Level Loader");
            customlevelloaderInstance = obj.GetComponent<customlevelloader>();
            Sensitivitymap = Sensitivitymapobj.GetComponent<Sensitivitysnap>();

            if (customlevelloaderInstance != null)
            {
                if (Sensitivitymap.valuechange == true)
                {
                    customlevelloaderInstance.Switchscene("Start Analysis Horizontal");
                }
                    
            }
            else
            {
                Debug.LogError("customlevelloader instance not found 0.");
            }
        }

    }
}
