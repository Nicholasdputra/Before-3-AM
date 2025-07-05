using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }

    [SerializeField] int startingTime = 0;
    int timer;
    public Coroutine timerCoroutine;
    public TextMeshProUGUI timerText;

    public GameObject pausePanel;
    public Button resumeButton;
    public Button backToMainMenuButton;
    public Button settingsButton;
    public Button HowToPlayButton;
    public GameObject howToPlayPanel;
    public GameObject settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
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
        resumeButton.onClick.AddListener(ResumeGame);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);
        settingsButton.onClick.AddListener(Settings);
        HowToPlayButton.onClick.AddListener(() => howToPlayPanel.SetActive(true));
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


    }

    void CloseAllPanels()
    {
        pausePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void Pause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            timer++;
            int minutes = (startingTime + timer) / 60;
            int seconds = (startingTime + timer) % 60;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1f);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }
    }

    public void BackToMainMenu()
    {
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
        // Show the settings panel
        settingsPanel.SetActive(true);
    }
}