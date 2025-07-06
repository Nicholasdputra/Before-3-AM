using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueDataReformatter : MonoBehaviour
{
    public DialogueDataSO[] dialogueDatas;
    public NPC[] npcList;
    public string playerName;

    void OnEnable()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Player");

        foreach (var dialogueData in dialogueDatas)
        {
            ResetNPCFirstTimeDialogue();
            ResetPlayerNameInDialogueData(dialogueData);
            DetermineHasFollowUpChoices(dialogueData);
            DetermineHasChoices(dialogueData);
            ResetHasBeenSaidInDialogueData(dialogueData);
            ReformatForPlayerName(dialogueData);
            CalculateTimeItWillTake(dialogueData);
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

    void OnApplicationQuit()
    {
        foreach (var dialogueData in dialogueDatas)
        {
            ResetPlayerNameInDialogueData(dialogueData);
            ResetHasBeenSaidInDialogueData(dialogueData);
        }
    }

    void ResetPlayerNameInDialogueData(DialogueDataSO dialogueData)
    {
        foreach (var dialogue in dialogueData.dialogues)
        {
            dialogue.ourQuestion = dialogue.ourQuestion.Replace(playerName, "{PlayerName}");

            for (int i = 0; i < dialogue.npcQuestionResponse.Length; i++)
            {
                dialogue.npcQuestionResponse[i] = dialogue.npcQuestionResponse[i].Replace(playerName, "{PlayerName}");
            }

            foreach (var choice in dialogue.playerChoices)
            {
                choice.ourChoice = choice.ourChoice.Replace(playerName, "{PlayerName}");
                for (int j = 0; j < choice.responseToOurChoice.Length; j++)
                {
                    choice.responseToOurChoice[j] = choice.responseToOurChoice[j].Replace(playerName, "{PlayerName}");
                }
            }
        }
    }

    void ResetHasBeenSaidInDialogueData(DialogueDataSO dialogueData)
    {
        foreach (var dialogue in dialogueData.dialogues)
        {
            dialogue.hasBeenSaid = false;
        }
    }

    void DetermineHasChoices(DialogueDataSO dialogueData)
    {
        foreach (var dialogue in dialogueData.dialogues)
        {
            if (dialogue.playerChoices != null && dialogue.playerChoices.Length > 0)
            {
                dialogue.hasPlayerChoices = true;
            }
            else
            {
                dialogue.hasPlayerChoices = false;
            }
        }
    }

    void DetermineHasFollowUpChoices(DialogueDataSO dialogueData)
    {
        foreach (var dialogue in dialogueData.dialogues)
        {
            if (dialogue.playerChoices != null)
            {
                foreach (var choice in dialogue.playerChoices)
                {
                    if (choice.followUpPlayerChoices != null && choice.followUpPlayerChoices.Length > 0)
                    {
                        choice.hasFollowUpPlayerResponse = true;
                    }
                    else
                    {
                        choice.hasFollowUpPlayerResponse = false;
                    }
                }
            }
        }
    }

    void ResetNPCFirstTimeDialogue()
    {
        foreach (var npc in npcList)
        {
            npc.isFirstTime = true;
        }
    }

    void CalculateTimeItWillTake(DialogueDataSO dialogueData)
    {
        foreach (var dialogue in dialogueData.dialogues)
        {
            dialogue.timeItWillTake = 0;
            int count = 0;

            foreach (string response in dialogue.npcQuestionResponse)
            {
                count += response.Length;
            }

            if (dialogue.hasPlayerChoices)
            {
                foreach (var choice in dialogue.playerChoices)
                {
                    count += choice.ourChoice.Length / dialogue.playerChoices.Length;
                    foreach (string response in choice.responseToOurChoice)
                    {
                        count += response.Length / choice.responseToOurChoice.Length;
                    }

                    if (choice.hasFollowUpPlayerResponse)
                    {
                        foreach (var followUpChoice in choice.followUpPlayerChoices)
                        {
                            count += followUpChoice.followUpChoice.Length / choice.followUpPlayerChoices.Length;
                            foreach (string response in followUpChoice.responseToFollowUpChoice)
                            {
                                count += response.Length / followUpChoice.responseToFollowUpChoice.Length;
                            }
                        }
                    }
                }
            }
            dialogue.timeItWillTake =
            // count;
            Mathf.CeilToInt(count/20);
        }
    
    }
}
