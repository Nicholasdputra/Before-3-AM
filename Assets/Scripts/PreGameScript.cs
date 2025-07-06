using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreGameScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    public TMP_InputField usernameInputField;
    public TextMeshProUGUI warningText;
    public Button confirmButton;
    public Button backButton;

    public string[] preGameTexts;
    public GameObject nameInputView;
    public GameObject disclamerView;
    public TextMeshProUGUI disclaimerText;

    [Header("Font Assets")]
    public TMP_FontAsset normalFont;
    public TMP_FontAsset emphasisFont;

    void Start()
    {
        if(audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        // Listen for when user finishes editing (including pressing Enter)
        usernameInputField.onEndEdit.AddListener(OnUsernameEndEdit);

        // Optional: Listen for value changes
        usernameInputField.onValueChanged.AddListener(OnUsernameChanged);

        // Set up button listeners
        confirmButton.onClick.AddListener(ConfirmButtonClicked);
        backButton.onClick.AddListener(BackButtonClicked);
    }

    void OnEnable()
    {
        if(nameInputView != null)
            nameInputView.SetActive(true);
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
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        string username = usernameInputField.text;
        if (CheckForValidUsername(username))
        {
            SaveUsername(username);
            StartCoroutine(HeadToGameScene());
        }
    }

    IEnumerator HeadToGameScene()
    {
        // Load the game scene
        Debug.Log("Username confirmed, loading game scene.");
        nameInputView.SetActive(false);
        disclamerView.SetActive(true);
        audioManager.StopBGM();
        foreach (string text in preGameTexts)
        {
            disclaimerText.text = ""; // Clear previous text

            //if it's the last one, slowly make the font red
            if (text == preGameTexts[preGameTexts.Length - 1])
            {
                disclaimerText.color = Color.red;
                audioManager.PlaySFX(audioManager.before3AmSFXClip);
                disclaimerText.fontSize = 100; // Optional: Increase font size for emphasis
                disclaimerText.font = emphasisFont; // Use emphasis font
            }
            else
            {
                disclaimerText.color = Color.white;
                disclaimerText.fontSize = 66; // Reset to normal size
                disclaimerText.font = normalFont; // Use normal font
            }

            foreach (char letter in text)
            {
                disclaimerText.text += letter;
                yield return new WaitForSecondsRealtime(0.05f); // Use realtime to work with timeScale = 0
            }
            yield return new WaitForSeconds(2f); // Optional delay for user feedback
        }
        SceneManager.LoadScene("GameScene");
    }

    void BackButtonClicked()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
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
