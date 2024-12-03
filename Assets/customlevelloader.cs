using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class customlevelloader : MonoBehaviour
{
    public Animator Transition;
    public float transitiontime = 1f;
    public string SceneName;
    public int legvalue;

    public void legposepractise()
    {
        
        legvalue = PlayerPrefs.GetInt("legtype");
        Debug.Log(legvalue);
        if (legvalue == 11)
        {
            Switchscene("MODE LEG TYPE PRACTICE");
        }
        else
        {
            Switchscene("Set Timer practice");
        }
    }
    // Update is called once per frame
    public void legpose()
    {
        legvalue = PlayerPrefs.GetInt("legtype");
        if (legvalue == 11)
        {
            Switchscene("MODE LEG TYPE ANALYSIS");
        }
        if (legvalue == 10)
        {
            Switchscene("Set Timer Analysis");
        }
    }
    
    
    public void Switchscene(string SceneName)
    {
        loadnextlevel(SceneName);
    }
    
    public void loadnextlevel(string SceneName)
    {
        StartCoroutine(Loadlevel(SceneName));
    }
    
    IEnumerator Loadlevel(string SceneName)
    {
        Transition.SetTrigger("Starttheend");
        
        yield return new WaitForSeconds(transitiontime);

        SceneManager.LoadScene(SceneName);
    }
}
