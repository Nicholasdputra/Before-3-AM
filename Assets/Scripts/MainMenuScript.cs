using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button archivesButton;
    [SerializeField] Button quitButton;

    public GameObject mainMenuButtons;
    public GameObject settingsPanel;
    public GameObject archivesPanel;

    void Start()
    {
        playButton.onClick.AddListener(Play);
        settingsButton.onClick.AddListener(Settings);
        archivesButton.onClick.AddListener(Archives);
        quitButton.onClick.AddListener(Quit);
    }

    void OnEnable()
    {
        if(audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        Time.timeScale = 1f; // Pause the game at the start   
        settingsPanel.GetComponent<SettingsScript>().LoadSettings();
        settingsPanel.SetActive(false);
        archivesPanel.SetActive(false);
        mainMenuButtons.SetActive(true);
        audioManager.PlayBGM(audioManager.mainMenuBGMClip);
    }

    void Archives()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        // Show the how-to-play panel
        archivesPanel.SetActive(true);
    }

    void Play()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        Time.timeScale = 1f; // Ensure the game runs at normal speed
        SceneManager.LoadScene("PreGameScene");
    }

    void Settings()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        // Show the settings panel
        settingsPanel.SetActive(true);
    }

    void Quit()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        Application.Quit();
    }
}
