using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDataSO", menuName = "ScriptableObjects/DialogueDataSO")]
public class DialogueDataSO : ScriptableObject
{
    [Header("Player Settings")]
    public string playerName = PlayerPrefs.GetString("PlayerName", "Player");
    
    [Header("Dialogue Data")]
    public DialogueSO[] dialogues;
}

[System.Serializable]
public class DialogueSO
{
    public string prompt;
    public string[] NPCFullResponse;
    public PlayerChoiceSO[] PlayerChoices;
    public bool hasBeenSaid;

    //Make sure to format {PlayerName} in everything with the player's name
}

[System.Serializable]
public class PlayerChoiceSO
{
    public string choiceType;
    public string[] choiceText;
    public string[] nextDialogue;
    
    // Make sure to format {PlayerName} with the player's name
}