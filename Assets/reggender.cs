using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reggender : MonoBehaviour
{
    public Dropdown ageDropdown; // Reference to the InputField for the username

    public Button registerButton; // Reference to the Button that triggers registration
    public Text messageText; // Reference to a Text element for displaying messages (optional)
    string genderval;

    public GameObject LevelLoader;

    void Start()
    {
        // Ensure the button triggers the OnRegisterButtonClicked method when clicked
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
    }

    void OnRegisterButtonClicked()
    {
        // Get the input values from the InputFields
        genderval = ageDropdown.options[ageDropdown.value].text;


        // Perform any validation if necessary
        if (string.IsNullOrEmpty(genderval) || genderval == "Select")
        {
            // Display a message if validation fails
            if (messageText != null)
            {
                messageText.text = "Please select your gender.";
            }
            return;
        }

        // Save the input values to PlayerPrefs
        PlayerPrefs.SetString("reggender", genderval);


        PlayerPrefs.Save();

        // Display a success message
        if (messageText != null)
        {
            messageText.text = "Registration successful!";
        }

        // Optionally, navigate to another scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");

        if (genderval != null)
        {

            customlevelloader otherScript = LevelLoader.GetComponent<customlevelloader>();
            otherScript.Switchscene("Register patient id");
        }
    }
}