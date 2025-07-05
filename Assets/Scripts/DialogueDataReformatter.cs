using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDataReformatter : MonoBehaviour
{
    public DialogueDataSO[] dialogueDatas;
    public string playerName;

    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        
        foreach (var dialogueData in dialogueDatas)
        {
            ReformatForPlayerName(dialogueData);
        }
    }

    void ReformatForPlayerName(DialogueDataSO dialogueData)
    {

        foreach (var dialogue in dialogueData.dialogues)
        {
            dialogue.ourQuestion = dialogue.ourQuestion.Replace("{PlayerName}", playerName);

            for (int i = 0; i < dialogue.npcQuestionResponse.Length; i++)
            {
                dialogue.npcQuestionResponse[i] = dialogue.npcQuestionResponse[i].Replace("{PlayerName}", playerName);
            }

            foreach (var choice in dialogue.playerChoices)
            {
                choice.ourChoice = choice.ourChoice.Replace("{PlayerName}", playerName);
                for (int j = 0; j < choice.responseToOurChoice.Length; j++)
                {
                    choice.responseToOurChoice[j] = choice.responseToOurChoice[j].Replace("{PlayerName}", playerName);
                }
            }
        }
    }
}
