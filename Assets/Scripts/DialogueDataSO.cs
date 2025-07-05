using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDataSO", menuName = "ScriptableObjects/DialogueDataSO")]
public class DialogueDataSO : ScriptableObject
{
    [Header("Dialogue Data")]
    public DialogueSO[] dialogues;
}

[System.Serializable]
public class DialogueSO
{
    public string ourQuestion;
    public string[] npcQuestionResponse;
    public PlayerChoiceSO[] playerChoices;
    public bool hasBeenSaid;
}

[System.Serializable]
public class PlayerChoiceSO
{
    public string ourChoice;
    public string[] responseToOurChoice;
    public bool hasFollowUpPlayerResponse;
    [SerializeField] // Make it more explicit
    public FollowUpPlayerChoiceSO[] followUpPlayerChoices;
}

[System.Serializable]
public class FollowUpPlayerChoiceSO
{
    public string followUpChoice;
    public string[] responseToFollowUpChoice;
}