using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VoteViewScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    [Header("Vote Elements")]
    public GameObject VoteView;
    public GameObject VoteButtonPrefab;
    public GameObject VoteButtonArea;
    public NPC[] npcList;
    public NPC chosenNPC;
    public GameObject npcIcon;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI bottomPanelText;
    public GameObject gatherAndVoteButton;
    public GameObject notesButton;
    public GameObject backButton;
    public GameObject roomView;
    public GameObject chooseWhoToVoteText;

    [Header("Ending UI Elements")]
    public GameObject endingPanel;
    public Image endingImage;
    public TextMeshProUGUI endingText;
    public TextMeshProUGUI endingNameText;
    public TextMeshProUGUI endingDescText;
    public TextMeshProUGUI hintText;
    public Button backToMainMenuButton;

    [Header("Ending Sprites and Texts")]
    public Sprite[] endingOneSprites;
    public Sprite endingTwoSprite;
    public Sprite endingThreeSprite;
    public string[] endingTexts;
    public string[] endingNames;
    public string[] endingDescTexts;
    public string[] hintTexts;


    void OnEnable()
    {
        if(audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        //Remove all existing listeners to prevent duplicates
        SetUpButtons();
        RandomizeChosenNPC();
    }
    void Update()
    {
        if(audioManager == null)
            audioManager = AudioManagerScript.Instance;
    }

    void SetUpButtons()
    {
        backToMainMenuButton.GetComponent<Button>().onClick.RemoveAllListeners();
        backToMainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });

        backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        backButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            CloseVoteView();
        });
    }

    public void OpenVoteView()
    {
        if (audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();

        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        chooseWhoToVoteText.SetActive(false);
        bottomPanelText.text = "";
        Time.timeScale = 0;
        // Activate the VoteView GameObject
        VoteView.SetActive(true);
        backButton.SetActive(true);
        gatherAndVoteButton.SetActive(false);
        notesButton.SetActive(false);
        StartCoroutine(RandomizeChosenNPC());
    }

    public void CloseVoteView()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        VoteView.SetActive(false);
        gatherAndVoteButton.SetActive(true);
        notesButton.SetActive(true);
        chosenNPC.RoomMode();
        bottomPanelText.text = "";
        Time.timeScale = 1;
    }

    IEnumerator RandomizeChosenNPC()
    {
        // Randomly select an NPC from the npcList
        int randomIndex = UnityEngine.Random.Range(0, npcList.Length);
        chosenNPC = npcList[randomIndex];
        chosenNPC.roomView.SetActive(false);

        // Update the UI elements with the chosen NPC's details
        npcIcon.GetComponent<Image>().sprite = chosenNPC.npcSprite;
        npcNameText.text = chosenNPC.npcName;

        // Update the bottom panel text
        bottomPanelText.text = "";
        foreach (char letter in chosenNPC.gatherToVoteDialogue)
        {
            bottomPanelText.text += letter;
            yield return new WaitForSecondsRealtime(0.05f); // Use realtime to work with timeScale = 0
        }
        //clear the VoteButtonArea
        foreach (Transform child in VoteButtonArea.transform)
        {
            Destroy(child.gameObject); // Clear existing vote buttons
        }
        VoteButtonArea.SetActive(true);
        chooseWhoToVoteText.SetActive(true);
        StartCoroutine(ShowVoteOptions());
    }

    IEnumerator ShowVoteOptions()
    {
        // Here you can implement the logic to show the vote options
        // For example, enabling buttons for each NPC in npcList
        foreach (NPC npc in npcList)
        {
            GameObject voteButton = Instantiate(VoteButtonPrefab, VoteButtonArea.transform);
            voteButton.name = "Vote Option For " + npc.npcName;
            voteButton.GetComponent<Image>().sprite = npc.npcSprite;
            Button buttonComponent = voteButton.GetComponent<Button>();
            Debug.Log("Button Component: " + buttonComponent);
            buttonComponent.onClick.AddListener(() => StartCoroutine(VoteButtonClicked(npc)));
            yield return new WaitForSecondsRealtime(0.5f); // Add a small delay between button instantiations
        }
    }

    IEnumerator VoteButtonClicked(NPC npc)
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        AudioClip dialogueClip = null;

        switch (npc.npcName)
        {
            case "Nikolas":
                dialogueClip = audioManager.nikolasDialogueClip;
                break;
            case "Renatta":
                dialogueClip = audioManager.renattaDialogueClip;
                break;
            case "Gunawan":
                dialogueClip = audioManager.gunawanDialogueClip;
                break;
            default:
                Debug.LogWarning("No dialogue clip found for NPC: " + npc.npcName);
                break;
        }

        audioManager.PlayDialogue(dialogueClip);

        VoteButtonArea.SetActive(false);
        backButton.SetActive(false);

        Debug.Log("Vote button clicked for NPC: " + npc.npcName);
        npcIcon.GetComponent<Image>().sprite = npc.npcSprite;
        npcNameText.text = npc.npcName;
        bottomPanelText.text = "";
        foreach (char letter in npc.chosenForVoteDialogue)
        {
            bottomPanelText.text += letter;
            yield return new WaitForSecondsRealtime(0.05f); // Use realtime to work with timeScale = 0
        }

        yield return new WaitUntil(CheckForInput);
        EndingOne(npc);
    }

    void EndingOne(NPC npc)
    {
        if (audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();

        audioManager.StopBGM();
        SetUpButtons();
        
        // Debug the BGM playback
        Debug.Log("Attempting to play game over BGM");
        Debug.Log("Game Over BGM Clip: " + audioManager.gameOverBGMClip);
        
        // Check BGM AudioSource volume
        AudioSource bgmSource = audioManager.transform.GetChild(0).GetComponent<AudioSource>();
        Debug.Log("BGM AudioSource volume: " + bgmSource.volume);
        
        audioManager.PlayBGM(audioManager.gameOverBGMClip);
        //Determine Ending Here
        endingPanel.SetActive(true);
        endingImage.sprite = endingOneSprites[DetermineWhichNPCItIs(npc)];
        endingText.text = endingTexts[0];
        endingNameText.text = endingNames[0];
        endingDescText.text = endingDescTexts[0];
        hintText.text = hintTexts[0];
        //save to playerprefs
        PlayerPrefs.SetInt("Ending1Reached", 1);
    }

    public void EndingTwo()
    {
        if (audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();

        
        // Debug the BGM playback
        Debug.Log("Attempting to play game over BGM (Ending Two)");
        AudioSource bgmSource = audioManager.transform.GetChild(0).GetComponent<AudioSource>();
        Debug.Log("BGM AudioSource volume: " + bgmSource.volume);
        
        audioManager.StopBGM();
        audioManager.PlayBGM(audioManager.gameOverBGMClip);
        SetUpButtons();
        //Determine Ending Here
        endingPanel.SetActive(true);
        endingImage.sprite = endingTwoSprite;
        endingText.text = endingTexts[1];
        endingNameText.text = endingNames[1];
        endingDescText.text = endingDescTexts[1];
        hintText.text = hintTexts[1];
        PlayerPrefs.SetInt("Ending2Reached", 1);
    }



    public void EndingThree()
    {
        if (audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        
        // Debug the BGM playback
        Debug.Log("Attempting to play game over BGM (Ending Three)");
        AudioSource bgmSource = audioManager.transform.GetChild(0).GetComponent<AudioSource>();
        Debug.Log("BGM AudioSource volume: " + bgmSource.volume);
        audioManager.StopBGM();
        audioManager.PlayBGM(audioManager.gameOverBGMClip);
        SetUpButtons();
        //Determine Ending Here
        endingPanel.SetActive(true);
        endingImage.sprite = endingThreeSprite;
        endingText.text = endingTexts[2];
        endingNameText.text = endingNames[2];
        endingDescText.text = endingDescTexts[2];
        hintText.text = hintTexts[2];
        PlayerPrefs.SetInt("Ending3Reached", 1);
    }

    int DetermineWhichNPCItIs(NPC npc)
    {
        if (npc.npcName == "Nikolas")
        {
            return 0;
        }
        else if (npc.npcName == "Renatta")
        {
            return 1;
        }
        else if (npc.npcName == "Gunawan")
        {
            return 2;
        }
        else
        {
            return -1;
        }
    }


    bool CheckForInput()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);
    }
}