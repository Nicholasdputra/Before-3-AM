using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public string npcName;
    public Sprite npcSprite;
    // public int favorability;
    public DialogueDataSO dialogueData;
    [SerializeField] public string firstTimeBaseDialogue;
    [SerializeField] public string nonFirstTimeBaseDialogue;
    [SerializeField] public string notFirstQuestionDialogue;

    public bool isFirstTime = true;

    public GameObject canvas;
    public GameObject dialogueView;
    public GameObject roomView;

    void Start()
    {
        Button npcButton = GetComponent<Button>();
        npcButton.onClick.AddListener(() => ConversationMode());

        canvas = GameObject.Find("Canvas");
        dialogueView = canvas.transform.Find("DialogueView").gameObject;
        roomView = canvas.transform.Find("RoomView").gameObject;
    }

    void ConversationMode()
    {
        DialogueViewScript dialogueViewScript = dialogueView.GetComponent<DialogueViewScript>();
        dialogueViewScript.focusedNPC = this;
        dialogueViewScript.state = "";
        roomView.SetActive(false);
        dialogueView.SetActive(true);
    }

    public void RoomMode()
    {
        DialogueViewScript dialogueViewScript = dialogueView.GetComponent<DialogueViewScript>();
        dialogueViewScript.focusedNPC = null;
        dialogueViewScript.state = "";
        dialogueView.SetActive(false);
        roomView.SetActive(true);
    }
}