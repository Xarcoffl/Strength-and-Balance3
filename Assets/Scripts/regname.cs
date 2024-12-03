using UnityEngine;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{
    public InputField usernameInputField; // Reference to the InputField for the username

    public Button registerButton; // Reference to the Button that triggers registration
    public Text messageText; // Reference to a Text element for displaying messages (optional)

    private double[] stabilityvalues = new double[5];

    public GameObject LevelLoader;

    void Start()
    {
        // Ensure the button triggers the OnRegisterButtonClicked method when clicked
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        PlayerPrefs.SetInt("stabilityindex", 0);

        // Ensure PlayerPrefs is saved
        PlayerPrefs.Save();

        string serializedArray = ListToString(stabilityvalues);
        PlayerPrefs.SetString("stabilityarray", serializedArray);
        PlayerPrefs.Save();

    }

    void OnRegisterButtonClicked()
    {
        // Get the input values from the InputFields
        string username = usernameInputField.text;


        // Perform any validation if necessary
        if (string.IsNullOrEmpty(username) )
        {
            // Display a message if validation fails
            if (messageText != null)
            {
                messageText.text = "Please enter Your Name.";
            }
            return;
        }

        // Save the input values to PlayerPrefs
        PlayerPrefs.SetString("regname", username);

        PlayerPrefs.Save();

        // Display a success message
        if (messageText != null)
        {
            messageText.text = "Registration successful!";
        }

        // Optionally, navigate to another scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");

        if (username != null)
        {

            customlevelloader otherScript = LevelLoader.GetComponent<customlevelloader>();
            otherScript.Switchscene("Register Age");
        }
    }

    string ListToString(double[] array)
    {

        string serializedList = string.Join(",", array);
        return serializedList;
    }
}
