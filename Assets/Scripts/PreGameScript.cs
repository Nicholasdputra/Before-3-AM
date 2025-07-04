using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreGameScript : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TextMeshProUGUI warningText;
    public Button confirmButton;
    public Button backButton;

    void Start()
    {
        // Listen for when user finishes editing (including pressing Enter)
        usernameInputField.onEndEdit.AddListener(OnUsernameEndEdit);

        // Optional: Listen for value changes
        usernameInputField.onValueChanged.AddListener(OnUsernameChanged);

        // Set up button listeners
        confirmButton.onClick.AddListener(ConfirmButtonClicked);
        backButton.onClick.AddListener(BackButtonClicked);
    }
    
    // Called when user presses Enter or clicks outside the input field
    void OnUsernameEndEdit(string username)
    {
        // Check if Enter was pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (CheckForValidUsername(username))
                SaveUsername(username);
        }
    }

    void ConfirmButtonClicked()
    {
        string username = usernameInputField.text;
        if (CheckForValidUsername(username))
        {
            SaveUsername(username);
            SceneManager.LoadScene("GameScene"); // Load the game scene
        }
    }

    void BackButtonClicked()
    {
        // Go back to the main menu or previous scene
        Debug.Log("Back button clicked, returning to main menu.");
        SceneManager.LoadScene("MainMenu");
    }

    bool CheckForValidUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            warningText.text = "Username cannot be empty!";
            return false;
        }

        if (username.Length < 2)
        {
            warningText.text = "Username must be at least 2 characters long!";
            return false;
        }
        
        if (username.Length > 15)
        {
            warningText.text = "Username can't be more than 15 characters long!";
            return false;
        }
        
        if (username.Contains(" "))
        {
            warningText.text = "Username cannot contain spaces!";
            return false;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
        {
            warningText.text = "Username can only contain letters and numbers!";
            return false;
        }

        warningText.text = "";
        return true;
    }
    
    void OnUsernameChanged(string username)
    {
        // Called every time the text changes
        Debug.Log("Username changed to: " + username);
    }
    
    void SaveUsername(string username)
    {
        if (!string.IsNullOrEmpty(username))
        {
            // Save to PlayerPrefs for persistence
            PlayerPrefs.SetString("PlayerName", username);
            PlayerPrefs.Save();
            
            Debug.Log("Username saved: " + username);
        }
    }
}
