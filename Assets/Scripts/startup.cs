using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("IsRegistered", 0) == 1)
        {
            // User is registered, load the main scene
            /* SceneManager.LoadScene("Dashboard");*/
            SceneManager.LoadScene("WELCOME 1");
        }
        else
        {
            // User is not registered, load the registration scene
            SceneManager.LoadScene("WELCOME 1");
        }
    }
}