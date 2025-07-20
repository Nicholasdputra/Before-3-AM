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
        if (dialogueData == null || dialogueData.dialogues == null)
        {
            Debug.LogWarning("DialogueData or dialogues array is null");
            return;
        }

        Debug.Log($"Reformatting dialogue data '{dialogueData.name}' for player name: '{playerName}'");

        foreach (var dialogue in dialogueData.dialogues)
        {
            if (dialogue == null) continue;

            // Replace in question
            if (!string.IsNullOrEmpty(dialogue.ourQuestion))
            {
                string originalQuestion = dialogue.ourQuestion;
                dialogue.ourQuestion = dialogue.ourQuestion.Replace("{PlayerName}", playerName);
                if (originalQuestion != dialogue.ourQuestion)
                {
                    Debug.Log($"Replaced in question: '{originalQuestion}' -> '{dialogue.ourQuestion}'");
                }
            }

            // Replace in NPC responses
            if (dialogue.npcQuestionResponse != null)
            {
                for (int i = 0; i < dialogue.npcQuestionResponse.Length; i++)
                {
                    if (!string.IsNullOrEmpty(dialogue.npcQuestionResponse[i]))
                    {
                        string original = dialogue.npcQuestionResponse[i];
                        dialogue.npcQuestionResponse[i] = dialogue.npcQuestionResponse[i].Replace("{PlayerName}", playerName);
                        if (original != dialogue.npcQuestionResponse[i])
                        {
                            Debug.Log($"Replaced in NPC response {i}: '{original}' -> '{dialogue.npcQuestionResponse[i]}'");
                        }
                    }
                }
            }

            // Replace in player choices
            if (dialogue.playerChoices != null)
            {
                foreach (var choice in dialogue.playerChoices)
                {
                    if (choice == null) continue;

                    if (!string.IsNullOrEmpty(choice.ourChoice))
                    {
                        string original = choice.ourChoice;
                        choice.ourChoice = choice.ourChoice.Replace("{PlayerName}", playerName);
                        if (original != choice.ourChoice)
                        {
                            Debug.Log($"Replaced in player choice: '{original}' -> '{choice.ourChoice}'");
                        }
                    }

                    if (choice.responseToOurChoice != null)
                    {
                        for (int j = 0; j < choice.responseToOurChoice.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(choice.responseToOurChoice[j]))
                            {
                                string original = choice.responseToOurChoice[j];
                                choice.responseToOurChoice[j] = choice.responseToOurChoice[j].Replace("{PlayerName}", playerName);
                                if (original != choice.responseToOurChoice[j])
                                {
                                    Debug.Log($"Replaced in choice response {j}: '{original}' -> '{choice.responseToOurChoice[j]}'");
                                }
                            }
                        }
                    }

                    if (choice.followUpPlayerChoices != null)
                    {
                        foreach (var followUpChoice in choice.followUpPlayerChoices)
                        {
                            if (followUpChoice == null) continue;

                            if (!string.IsNullOrEmpty(followUpChoice.followUpChoice))
                            {
                                string original = followUpChoice.followUpChoice;
                                followUpChoice.followUpChoice = followUpChoice.followUpChoice.Replace("{PlayerName}", playerName);
                                if (original != followUpChoice.followUpChoice)
                                {
                                    Debug.Log($"Replaced in follow-up choice: '{original}' -> '{followUpChoice.followUpChoice}'");
                                }
                            }

                            if (followUpChoice.responseToFollowUpChoice != null)
                            {
                                for (int k = 0; k < followUpChoice.responseToFollowUpChoice.Length; k++)
                                {
                                    if (!string.IsNullOrEmpty(followUpChoice.responseToFollowUpChoice[k]))
                                    {
                                        string original = followUpChoice.responseToFollowUpChoice[k];
                                        followUpChoice.responseToFollowUpChoice[k] = followUpChoice.responseToFollowUpChoice[k].Replace("{PlayerName}", playerName);
                                        if (original != followUpChoice.responseToFollowUpChoice[k])
                                        {
                                            Debug.Log($"Replaced in follow-up response {k}: '{original}' -> '{followUpChoice.responseToFollowUpChoice[k]}'");
                                        }
                                    }
                                }
                            }
                        }
                    }
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
