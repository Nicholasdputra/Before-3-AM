using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueViewScript : MonoBehaviour
{
    public AudioManagerScript audioManager; // Reference to AudioManagerScript
    public string state;
    public NPC focusedNPC;
    public GameObject npcSprite;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogueText;
    public Button backButton;

    public GameObject playerChoicesArea;
    public GameObject playerChoicePrefab;
    // public GameObject playerChoiceWithTimePrefab;

    public Coroutine currentTypingCoroutine;
    public float typingSpeed = 0.05f;

    private bool waitingForInput = false;
    private bool isTyping = false;
    private bool skipTyping = false;
    private float typingStartTime = 0f;
    public bool askedQuestionThisInteraction = false;
    public bool inGoToNextState = false;

    void OnEnable()
    {
        if(audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();

        if (backButton != null && focusedNPC != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => focusedNPC.RoomMode());
        }
        askedQuestionThisInteraction = false;

        // Initialize the dialogue view
        state = "InitialState";
        npcName.text = focusedNPC.npcName;
        npcSprite.GetComponent<Image>().sprite = focusedNPC.npcSprite;
        npcDialogueText.text = "";
        ClearPlayerChoices();
        GoToNextState();
    }

    void Update()
    {
        if(audioManager == null)
            audioManager = AudioManagerScript.Instance;
            
        // Check for input when waiting
        if (waitingForInput && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            waitingForInput = false;
        }

        // Check for skip typing input (only after 1.5 seconds of typing)
        if (isTyping && Time.time - typingStartTime > 1.5f && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            skipTyping = true;
        }

    }

    //Flow of dialogue view
    void ShowBaseDialogue()
    {
        PlayDialogueSFXForNPC();
        backButton.gameObject.SetActive(true);

        if (state == "ShowBaseDialogue")
        {
            Debug.LogWarning("ShowBaseDialogue called while already in ShowBaseDialogue state. This might cause unexpected behavior.");
            return;
        }

        state = "ShowBaseDialogue";
        playerChoicesArea.SetActive(false);
        string baseLineToShow;

        if (focusedNPC.isFirstTime)
        {
            baseLineToShow = focusedNPC.firstTimeBaseDialogue;
            ResetText();
            currentTypingCoroutine = StartCoroutine(TypeLetterByLetter(baseLineToShow));
            focusedNPC.isFirstTime = false;
        }
        else if (focusedNPC.nonFirstTimeBaseDialogue != null && !askedQuestionThisInteraction)
        {
            baseLineToShow = focusedNPC.nonFirstTimeBaseDialogue;
            ResetText();
            currentTypingCoroutine = StartCoroutine(TypeLetterByLetter(baseLineToShow));
        }
        else if (askedQuestionThisInteraction)
        {
            baseLineToShow = focusedNPC.notFirstQuestionDialogue;
            ResetText();
            currentTypingCoroutine = StartCoroutine(TypeLetterByLetter(baseLineToShow));
        }
    }

    void DetermineWhichQuestionsToShow()
    {
        state = "DetermineWhichQuestionsToShow";
        // Reset indices to -1
        // This ensures that if no questions are available, the indices remain -1
        int indexQ1 = -1;
        int indexQ2 = -1;
        int indexQ3 = -1;

        Debug.Log("Focused NPC: " + focusedNPC.npcName);
        Debug.Log("Dialogue Data Length: " + focusedNPC.dialogueData.dialogues.Length);

        //Loop through the dialogues of the focused NPC
        for (int i = 0; i < focusedNPC.dialogueData.dialogues.Length; i += 3)
        {
            if (!focusedNPC.dialogueData.dialogues[i].hasBeenSaid)
            {
                indexQ1 = i;
                break;
            }
        }

        for (int i = 1; i < focusedNPC.dialogueData.dialogues.Length; i += 3)
        {
            if (!focusedNPC.dialogueData.dialogues[i].hasBeenSaid)
            {
                indexQ2 = i;
                break;
            }
        }

        for (int i = 2; i < focusedNPC.dialogueData.dialogues.Length; i += 3)
        {
            if (!focusedNPC.dialogueData.dialogues[i].hasBeenSaid)
            {
                indexQ3 = i;
                break;
            }
        }

        List<int> validIndices = new List<int>();
        if (indexQ1 != -1)
            validIndices.Add(indexQ1);
        if (indexQ2 != -1)
            validIndices.Add(indexQ2);
        if (indexQ3 != -1)
            validIndices.Add(indexQ3);

        validIndices.Sort(); // Now only contains valid dialogue indices
        StartCoroutine(CheckIfQuestionAreAvailable(validIndices));
    }

    IEnumerator CheckIfQuestionAreAvailable(List<int> validIndices)
    {
        if (state == "CheckIfQuestionAreAvailable")
        {
            yield break;
        }

        if (validIndices.Count == 0)
        {
            state = "NoQuestionsAvailable";
            playerChoicesArea.SetActive(false);
            npcName.text = PlayerPrefs.GetString("PlayerName", "Player");
            ResetText();
            yield return StartCoroutine(TypeLetterByLetter("Nevermind, I don't think there's anything else I can ask you."));
        }
        else
        {
            ShowPlayerQuestions(validIndices);
        }
    }

    void ShowPlayerQuestions(List<int> validIndices)
    {
        playerChoicesArea.SetActive(true);
        state = "PlayerPickingQuestion";

        // Clear previous choices
        foreach (Transform child in playerChoicesArea.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new choices based on valid indices
        foreach (int index in validIndices)
        {
            GameObject choice = Instantiate(playerChoicePrefab, playerChoicesArea.transform);
            TextMeshProUGUI textComponent = choice.GetComponentInChildren<TextMeshProUGUI>();
            DialogueSO dialogue = focusedNPC.dialogueData.dialogues[index];
            string fullText = dialogue.ourQuestion + " (" + dialogue.timeItWillTake + " mins)";
            textComponent.text = fullText;

            // Force the text to update and check if it needs multiple lines
            textComponent.ForceMeshUpdate();
            if (textComponent.textInfo.lineCount > 1)
            {
                // Directly adjust the RectTransform height based on extra lines
                RectTransform rectTransform = choice.GetComponent<RectTransform>();
                int extraLines = textComponent.textInfo.lineCount - 1;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + (extraLines * 50f));
            }

            Button choiceButton = choice.GetComponent<Button>();
            choiceButton.onClick.AddListener(() => ShowNPCQuestionResponse(index));
        }
    }

    void ShowNPCQuestionResponse(int index)
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        PlayDialogueSFXForNPC();
        GameManagerScript.Instance.timer += focusedNPC.dialogueData.dialogues[index].timeItWillTake;
        backButton.gameObject.SetActive(false);
        askedQuestionThisInteraction = true;
        playerChoicesArea.SetActive(false);
        state = "ShowNPCQuestionResponse";
        ResetText();

        StartCoroutine(ShowNPCQuestionResponseSequentially(index));

        // Mark the dialogue as said
        focusedNPC.dialogueData.dialogues[index].hasBeenSaid = true;
        if (focusedNPC.isFirstTime)
        {
            focusedNPC.isFirstTime = false; // Set to false after the first interaction
        }
    }

    IEnumerator ShowNPCQuestionResponseSequentially(int index)
    {
        state = "ShowNPCQuestionResponseSequentially";
        string[] responses = focusedNPC.dialogueData.dialogues[index].npcQuestionResponse;
        yield return StartCoroutine(TypeSequentially(responses));

        if (focusedNPC.dialogueData.dialogues[index].hasPlayerChoices)
        {
            ShowPlayerChoices(index);
        }
        else
        {
            // If no follow-up choices, return to base dialogue
            ShowBaseDialogue();
        }

    }

    void ShowPlayerChoices(int index)
    {
        playerChoicesArea.SetActive(true);
        state = "ShowPlayerChoices";
        ClearPlayerChoices();

        // Create new choices based on the player's choice
        PlayerChoiceSO[] playerChoices = focusedNPC.dialogueData.dialogues[index].playerChoices;
        foreach (PlayerChoiceSO choice in playerChoices)
        {
            GameObject choiceObj = Instantiate(playerChoicePrefab, playerChoicesArea.transform);
            TextMeshProUGUI textComponent = choiceObj.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = choice.ourChoice;

            // Force the text to update and check if it needs multiple lines
            textComponent.ForceMeshUpdate();
            if (textComponent.textInfo.lineCount > 1)
            {
                // Directly adjust the RectTransform height based on extra lines
                RectTransform rectTransform = choiceObj.GetComponent<RectTransform>();
                int extraLines = textComponent.textInfo.lineCount - 1;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + (extraLines * 50f));
            }

            Button choiceButton = choiceObj.GetComponent<Button>();
            choiceButton.onClick.AddListener(() => ShowResponseToPlayerChoice(choice));
        }
    }

    void ShowResponseToPlayerChoice(PlayerChoiceSO choice)
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        PlayDialogueSFXForNPC();
        playerChoicesArea.SetActive(false);
        state = "ShowResponseToPlayerChoice";
        ResetText();
        StartCoroutine(ShowNPCResponseToPlayerChoiceSequentially(choice.responseToOurChoice, choice));
    }

    IEnumerator ShowNPCResponseToPlayerChoiceSequentially(string[] responses, PlayerChoiceSO choice)
    {
        state = "ShowNPCResponseToPlayerChoiceSequentially";
        yield return StartCoroutine(TypeSequentially(responses));

        // After all responses are shown, check for follow-up choices
        if (choice.hasFollowUpPlayerResponse)
        {
            ShowFollowUpChoices(choice.followUpPlayerChoices);
        }
        else
        {
            // If no follow-up choices, return to base dialogue
            ShowBaseDialogue();
        }
    }

    void ShowFollowUpChoices(FollowUpPlayerChoiceSO[] followUpChoices)
    {
        if (state == "ShowFollowUpChoices")
        {
            Debug.LogWarning("ShowFollowUpChoices called while already in ShowFollowUpChoices state. This might cause unexpected behavior.");
            return;
        }

        playerChoicesArea.SetActive(true);
        state = "ShowFollowUpChoices";
        ClearPlayerChoices();

        // Create new choices based on follow-up choices
        foreach (FollowUpPlayerChoiceSO followUpChoice in followUpChoices)
        {
            GameObject choiceObj = Instantiate(playerChoicePrefab, playerChoicesArea.transform);
            TextMeshProUGUI textComponent = choiceObj.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = followUpChoice.followUpChoice;

            // Force the text to update and check if it needs multiple lines
            textComponent.ForceMeshUpdate();
            if (textComponent.textInfo.lineCount > 1)
            {
                // Directly adjust the RectTransform height based on extra lines
                RectTransform rectTransform = choiceObj.GetComponent<RectTransform>();
                int extraLines = textComponent.textInfo.lineCount - 1;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + (extraLines * 50f));
            }

            Button choiceButton = choiceObj.GetComponent<Button>();
            choiceButton.onClick.AddListener(() => ShowResponseToFollowUpChoice(followUpChoice));
        }
    }

    void ShowResponseToFollowUpChoice(FollowUpPlayerChoiceSO followUpChoice)
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        PlayDialogueSFXForNPC();
        playerChoicesArea.SetActive(false);
        state = "ShowResponseToFollowUpChoice";
        ResetText();

        StartCoroutine(ShowNPCResponseToFollowUpChoiceSequentially(followUpChoice.responseToFollowUpChoice));
    }

    IEnumerator ShowNPCResponseToFollowUpChoiceSequentially(string[] responses)
    {
        state = "ShowNPCResponseToFollowUpChoiceSequentially";
        yield return StartCoroutine(TypeSequentially(responses));
        GoToNextState();
    }



    //Helper fuctions
    IEnumerator TypeSequentially(string[] responses)
    {
        foreach (string response in responses)
        {
            ResetText(); 
            // Wait for each typing coroutine to complete before starting the next
            yield return StartCoroutine(TypeLetterByLetter(response));

            // Wait for player input before continuing to next response
            yield return StartCoroutine(WaitForInput());
        }
    }

    IEnumerator WaitForInput()
    {
        Debug.Log("Waiting for input...");
        waitingForInput = true;
        while (waitingForInput)
        {
            yield return null;
        }
    }

    IEnumerator WaitForInputThenRoomMode()
    {
        yield return StartCoroutine(WaitForInput());
        focusedNPC.RoomMode();
    }

    void ResetText()
    {
        // Stop any currently running typing coroutine
        if (currentTypingCoroutine != null)
        {
            StopCoroutine(currentTypingCoroutine);
            currentTypingCoroutine = null;
        }

        // Reset typing state
        isTyping = false;
        skipTyping = false;

        npcDialogueText.text = "";
    }

    void ClearPlayerChoices()
    {
        foreach (Transform child in playerChoicesArea.transform)
        {
            Destroy(child.gameObject);
        }
    }

    IEnumerator TypeLetterByLetter(string text)
    {
        PlayDialogueSFXForNPC();
        isTyping = true;
        skipTyping = false;
        typingStartTime = Time.time;

        npcDialogueText.text = "";

        foreach (char letter in text)
        {
            // If skip was requested, show the full text immediately
            if (skipTyping)
            {
                npcDialogueText.text = text;
                isTyping = false;
                skipTyping = false;
                break;
            }

            npcDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        skipTyping = false;
        if (state != "NoQuestionsAvailable")
            yield return StartCoroutine(WaitForInput());
        GoToNextState();
    }

    void GoToNextState()
    {
        if (inGoToNextState)
        {
            return;
        }
        inGoToNextState = true;
        Debug.Log("Current state: " + state);
        Debug.Log("Going to next state:=...");
        switch (state)
        {
            case "InitialState":
                Debug.Log("Initial state reached, showing base dialogue.");
                ShowBaseDialogue();
                inGoToNextState = false;
                break;
            case "ShowBaseDialogue":
                DetermineWhichQuestionsToShow();
                inGoToNextState = false;
                break;
            case "DetermineWhichQuestionsToShow":
                Debug.Log("Determining which questions to show next.");
                inGoToNextState = false;
                break;
            case "PlayerPickingQuestion":
                Debug.Log("Player is picking a question.");
                // Already handled in ShowPlayerQuestions
                inGoToNextState = false;
                break;
            case "ShowNPCQuestionResponse":
                Debug.Log("Showing NPC question response.");
                // Handled in ShowNPCQuestionResponseSequentially
                inGoToNextState = false;
                break;
            case "ShowNPCQuestionResponseSequentially":
                Debug.Log("Finished showing NPC question response, now showing player choices.");
                // Handled in ShowNPCQuestionResponseSequentially
                inGoToNextState = false;
                break;
            case "ShowPlayerChoices":
                Debug.Log("Showing player choices.");
                // Handled in ShowPlayerChoices
                inGoToNextState = false;
                break;
            case "ShowResponseToPlayerChoice":
                Debug.Log("Showing response to player choice.");
                // Handled in ShowNPCResponseToPlayerChoiceSequentially
                inGoToNextState = false;
                break;
            case "ShowNPCResponseToPlayerChoiceSequentially":
                Debug.Log("Finished showing NPC response to player choice sequentially.");
                // Handled in ShowNPCResponseToPlayerChoiceSequentially
                inGoToNextState = false;
                break;
            case "ShowFollowUpChoices":
                Debug.Log("Showing follow-up choices.");
                // Handled in ShowFollowUpChoices
                inGoToNextState = false;
                break;
            case "ShowResponseToFollowUpChoice":
                // Handled in ShowNPCResponseToFollowUpChoiceSequentially
                inGoToNextState = false;
                break;
            case "ShowNPCResponseToFollowUpChoiceSequentially":
                Debug.Log("Finished showing NPC response to follow-up choice sequentially.");
                // Handled in ShowNPCResponseToFollowUpChoiceSequentially
                ShowBaseDialogue();
                inGoToNextState = false;
                break;
            case "NoQuestionsAvailable":
                Debug.Log("No questions available, returning to room mode.");
                StartCoroutine(WaitForInputThenRoomMode());
                inGoToNextState = false;
                break;
            default:
                Debug.LogWarning("Unknown state: " + state);
                inGoToNextState = false;
                break;
        }
    }
    
    void PlayDialogueSFXForNPC()
    {
        if (focusedNPC == null)
        {
            Debug.LogWarning("playDialogueSFXForNPC called with null NPC.");
            return;
        }

        AudioClip dialogueClip = null;

        switch (focusedNPC.npcName)
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
                Debug.LogWarning("No dialogue clip found for NPC: " + focusedNPC.npcName);
                return;
        }

        audioManager.PlayDialogue(dialogueClip);
    }
}