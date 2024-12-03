using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SecondsValue : MonoBehaviour
{
    public InputField inputField; // Reference to the InputField component
    public Text validationText; // Reference to the Text component for validation messages
    public float val;
    void Start()
    {
        // Add a listener to the input field to call ValidateInput whenever the text changes
        inputField.onValueChanged.AddListener(delegate { ValidateInput(val); });
    }

    public void ValidateInput(float val)
    {
        string input = inputField.text;
        
        float.TryParse(input, out val);
        PlayerPrefs.SetFloat("timervalue", val);
        PlayerPrefs.Save();
        // Example condition: input must be alphanumeric and between 5 and 10 characters
        if (IsValidInput(input, val))
        {
            validationText.text = "Valid input";
            validationText.color = Color.black;
        }
        else
        {
            validationText.text = "Invalid input. Must contain only numeric value lesser than 120.";
            validationText.color = Color.red;
        }
    }

    bool IsValidInput(string input , float val)
    {
        // Check if the input is between 5 and 10 characters and is alphanumeric
        return input.Length >= 0 && input.Length <= 3 && Regex.IsMatch(input, @"^[0-9]+$") && val <= 120 && val >= 5;
    }
}
