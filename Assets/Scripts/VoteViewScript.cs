using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VoteViewScript : MonoBehaviour
{
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
        //Remove all existing listeners to prevent duplicates
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

        RandomizeChosenNPC();
    }

    public void OpenVoteView()
    {
        bottomPanelText.text = "";
        Time.timeScale = 0;
        // Activate the VoteView GameObject
        VoteView.SetActive(true);
        gatherAndVoteButton.SetActive(false);
        notesButton.SetActive(false);
        StartCoroutine(RandomizeChosenNPC());
    }

    public void CloseVoteView()
    {
        VoteView.SetActive(false);
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
        ShowVoteOptions();
    }

    void ShowVoteOptions()
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
        }
    }

    IEnumerator VoteButtonClicked(NPC npc)
    {
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
        //Determine Ending Here
        endingPanel.SetActive(true);
        endingImage.sprite = endingOneSprites[DetermineWhichNPCItIs(npc)];
        endingText.text = endingTexts[0];
        endingNameText.text = endingNames[0];
        endingDescText.text = endingDescTexts[0];
        hintText.text = hintTexts[0];
    }

    void EndingTwo(NPC npc)
    {
        //Determine Ending Here
        endingPanel.SetActive(true);
        endingImage.sprite = endingTwoSprite;
        endingText.text = endingTexts[1];
        endingNameText.text = endingNames[1];
        endingDescText.text = endingDescTexts[1];
        hintText.text = hintTexts[1];
    }

    void EndingThree(NPC npc)
    {
        //Determine Ending Here
        endingPanel.SetActive(true);
        endingImage.sprite = endingThreeSprite;
        endingText.text = endingTexts[2];
        endingNameText.text = endingNames[2];
        endingDescText.text = endingDescTexts[2];
        hintText.text = hintTexts[2];
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