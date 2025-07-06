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
    public bool hasBeenSaid;
    public string ourQuestion;
    public int timeItWillTake;
    public string[] npcQuestionResponse;
    public bool hasPlayerChoices;
    public PlayerChoiceSO[] playerChoices;
}

[System.Serializable]
public class PlayerChoiceSO
{
    public string ourChoice;
    public string[] responseToOurChoice;
    public bool hasFollowUpPlayerResponse;
    [SerializeField] public FollowUpPlayerChoiceSO[] followUpPlayerChoices;
}

[System.Serializable]
public class FollowUpPlayerChoiceSO
{
    public string followUpChoice;
    public string[] responseToFollowUpChoice;
}