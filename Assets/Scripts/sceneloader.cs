using UnityEngine;
using UnityEngine.SceneManagement;


public class sceneloader : MonoBehaviour
{
    public string sceneName;
     // Assuming you want to get text from a TextMeshPro input field

    // You can call this method using a UI button's OnClick event in the Unity Editor
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}

