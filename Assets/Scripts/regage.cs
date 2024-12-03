using UnityEngine;
using UnityEngine.UI;

public class regage : MonoBehaviour
{
    public InputField age; // Reference to the InputField for the username

    public Button registerButton; // Reference to the Button that triggers registration
    public Text messageText; // Reference to a Text element for displaying messages (optional)
    string ageval;

    public GameObject LevelLoader;

    void Start()
    {
        // Ensure the button triggers the OnRegisterButtonClicked method when clicked
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        
    }

    void OnRegisterButtonClicked()
    {
        // Get the input values from the InputFields
        ageval = age.text;


        // Perform any validation if necessary
        if (string.IsNullOrEmpty(ageval))
        {
            // Display a message if validation fails
            if (messageText != null)
            {
                messageText.text = "Please enter your age.";
            }
            return;
        }

        // Save the input values to PlayerPrefs
        PlayerPrefs.SetString("regage", ageval);


        PlayerPrefs.Save();

        // Display a success message
        if (messageText != null)
        {
            messageText.text = "Registration successful!";
        }

        // Optionally, navigate to another scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");

        if (ageval != null)
        {

            customlevelloader otherScript = LevelLoader.GetComponent<customlevelloader>();
            otherScript.Switchscene("Register Gender");
        }
    }
}
