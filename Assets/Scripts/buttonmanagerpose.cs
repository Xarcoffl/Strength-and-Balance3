using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonmanagerpose : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    private Color defaultColor = Color.white;
    private Color selectedColor = Color.grey;

    private void Start()
    {
        // Set the initial state of the buttons
        SetButtonState(yesButton, false);
        SetButtonState(noButton, false);

        // Add listeners to the buttons
        yesButton.onClick.AddListener(() => OnButtonClicked(yesButton, noButton));
        noButton.onClick.AddListener(() => OnButtonClicked(noButton, yesButton));
    }

    private void OnButtonClicked(Button clickedButton, Button otherButton)
    {
        // Toggle the state of the clicked button
        bool isSelected = clickedButton.image.color == defaultColor;

        // Set the states accordingly
        SetButtonState(clickedButton, isSelected);
        SetButtonState(otherButton, !isSelected);
    }

    private void SetButtonState(Button button, bool isSelected)
    {
        button.image.color = isSelected ? selectedColor : defaultColor;
    }
}