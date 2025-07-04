using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Button quitButton;

    public GameObject mainMenuButtons;
    public GameObject settingsPanel;
    public GameObject howToPlayPanel;

    void Start()
    {
        Time.timeScale = 1f; // Pause the game at the start   
        playButton.onClick.AddListener(Play);
        settingsButton.onClick.AddListener(Settings);
        howToPlayButton.onClick.AddListener(HowToPlay);
        quitButton.onClick.AddListener(Quit);
        settingsPanel.GetComponent<SettingsScript>().LoadSettings();

        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        mainMenuButtons.SetActive(true);
    }

    void HowToPlay()
    {
        // Show the how-to-play panel
        howToPlayPanel.SetActive(true);
    }

    void Play()
    {
        Time.timeScale = 1f; // Ensure the game runs at normal speed
        SceneManager.LoadScene("PreGameScene");
    }

    void Settings()
    {
        // Show the settings panel
        settingsPanel.SetActive(true);
    }

    void Quit()
    {
        Application.Quit();
    }
}
