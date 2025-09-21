using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }

    AudioManagerScript audioManager;
    [SerializeField] int startingTime = 0;
    public int timer;
    public Coroutine timerCoroutine;
    public TextMeshProUGUI timerText;

    public GameObject notesPanel;
    public Button openNotesButton;
    public Button closeNotesButton;

    public GameObject pausePanel;
    public Button resumeButton;
    public Button backToMainMenuButton;
    public Button settingsButton;
    public Button archivesButton;
    public GameObject archivesPanel;
    public GameObject settingsPanel;

    public VoteViewScript voteViewScript;
    public bool hasPlayedClockReminder;
    public bool hasTriggeredEndingTwo;

    // Start is called before the first frame update
    void OnEnable()
    {
        hasTriggeredEndingTwo = false;
        if (audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();

        audioManager.PlayBGM(audioManager.gameBGMClip);
        hasPlayedClockReminder = false;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        timer = 0;
        timerCoroutine = StartCoroutine(TimerCoroutine());
        pausePanel.SetActive(false);
        //add listeners only if they're not already added

        if (resumeButton.onClick.GetPersistentEventCount() > 0)
        {
            resumeButton.onClick.RemoveAllListeners();
        }
        resumeButton.onClick.AddListener(ResumeGame);

        if (backToMainMenuButton.onClick.GetPersistentEventCount() > 0)
        {
            backToMainMenuButton.onClick.RemoveAllListeners();
        }
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);

        if (settingsButton.onClick.GetPersistentEventCount() > 0)
        {
            settingsButton.onClick.RemoveAllListeners();
        }
        settingsButton.onClick.AddListener(Settings);

        if (archivesButton.onClick.GetPersistentEventCount() > 0)
        {
            archivesButton.onClick.RemoveAllListeners();
        }
        archivesButton.onClick.AddListener(OpenArchives);

        if (openNotesButton.onClick.GetPersistentEventCount() > 0)
        {
            openNotesButton.onClick.RemoveAllListeners();
        }
        openNotesButton.onClick.AddListener(OpenNotes);

        if (closeNotesButton.onClick.GetPersistentEventCount() > 0)
        {
            closeNotesButton.onClick.RemoveAllListeners();
        }
        closeNotesButton.onClick.AddListener(CloseNotes);
    }

    // Update is called once per frame
    void Update()
    {
        // Pause the game when the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale > 0f)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0f)
        {
            ResumeGame();
            CloseAllPanels();
        }

        if(timer >= 350 && !hasPlayedClockReminder && timer < 360)
        {
            hasPlayedClockReminder = true;
            audioManager.PlaySFX(audioManager.clockTickSFXClip);
        }

        if (timer >= 360 && !hasTriggeredEndingTwo)
        {
            hasTriggeredEndingTwo = true;
            voteViewScript.EndingTwo();
        }
    }

    void CloseAllPanels()
    {
        pausePanel.SetActive(false);
        archivesPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void Pause()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            timer++;
            
            // Calculate total minutes from starting point
            int totalMinutes = startingTime + timer;
            
            int displayHour, displayMinute;
            
            if (totalMinutes < 180) // First 3 hours: 09:00 to 12:00
            {
                displayHour = 21 + (totalMinutes / 60);
                displayMinute = totalMinutes % 60;
            }
            else // After 12:00, switch to 00:00 - 03:00
            {
                int minutesAfterNoon = totalMinutes - 180;
                displayHour = minutesAfterNoon / 60;
                displayMinute = minutesAfterNoon % 60;
            }
            
            timerText.text = string.Format("{0:00}:{1:00}", displayHour, displayMinute);
            yield return new WaitForSeconds(2f);
        }
    }

    public void ResumeGame()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }
    }

    public void BackToMainMenu()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    void Settings()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        // Show the settings panel
        settingsPanel.SetActive(true);
    }

    void OpenArchives()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        archivesPanel.SetActive(true);
    }

    void OpenNotes()
    {
        audioManager.PlaySFX(audioManager.paperRuffleSFXClip);
        notesPanel.SetActive(true);
    }

    void CloseNotes()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        notesPanel.SetActive(false);
    }
}