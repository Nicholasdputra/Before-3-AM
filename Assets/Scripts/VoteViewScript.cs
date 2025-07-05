using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoteViewScript : MonoBehaviour
{
    public GameObject VoteView;
    public bool canVote;
    public bool mustVote;
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

    void Start()
    {
        if (mustVote)
        {
            backButton.SetActive(false);
        }
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
        if (!mustVote)
        {
            VoteView.SetActive(false);
            chosenNPC.RoomMode();
            bottomPanelText.text = "";
            Time.timeScale = 1;
        }
    }

    void OnEnable()
    {
        RandomizeChosenNPC();
    }

    IEnumerator RandomizeChosenNPC()
    {
        // Randomly select an NPC from the npcList
        int randomIndex = UnityEngine.Random.Range(0, npcList.Length);
        chosenNPC = npcList[randomIndex];

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
        npcIcon.GetComponent<Image>().sprite = npc.npcSprite;
        npcNameText.text = npc.npcName;
        bottomPanelText.text = "";
        foreach (char letter in npc.chosenForVoteDialogue)
        {
            bottomPanelText.text += letter;
            yield return new WaitForSecondsRealtime(0.05f); // Use realtime to work with timeScale = 0
        }

        yield return new WaitUntil(CheckForInput);
        DetermineEnding();
    }

    void DetermineEnding()
    {
        //Determine Ending Here
    }
    
    bool CheckForInput()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);
    }
}
