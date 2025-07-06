using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchivesScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    public int endingCount = 0; // Number of endings reached
    public Button closeButton;
    public TextMeshProUGUI endingCountText;
    public Sprite[] endingSprites;
    public string[] endingNames;
    public GameObject endingDisplayArea; // Areas where each ending will be displayed
    public GameObject endingPrefab; // Prefab for displaying each ending

    public void OnEnable()
    {
        if(audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        // Initialize the how-to-play panel
        closeButton.onClick.AddListener(CloseArchives);
        DisplayReachedEndings();
        // Debug.Log("How to play panel opened");
    }

    public void DisplayReachedEndings()
    {
        // Clear the previous endings displayed
        foreach (Transform child in endingDisplayArea.transform)
        {
            Destroy(child.gameObject);
        }
        // Display the endings that have been reached
        int ending1Reached = PlayerPrefs.GetInt("Ending1Reached", 0);
        int ending2Reached = PlayerPrefs.GetInt("Ending2Reached", 0);
        int ending3Reached = PlayerPrefs.GetInt("Ending3Reached", 0);

        endingCount = ending1Reached + ending2Reached + ending3Reached;
        endingCountText.text = "Endings Reached (" + endingCount + " / 3)";

        // Instantiate and display each ending
        if (ending1Reached > 0)
        {
            //instantiate the prefab for ending 1
            GameObject ending1 = Instantiate(endingPrefab, endingDisplayArea.transform);
            ending1.GetComponentInChildren<Image>().sprite = endingSprites[0];
            ending1.GetComponentInChildren<TextMeshProUGUI>().text = endingNames[0];
        }

        if (ending2Reached > 0)
        {
            //instantiate the prefab for ending 2
            GameObject ending2 = Instantiate(endingPrefab, endingDisplayArea.transform);
            ending2.GetComponentInChildren<Image>().sprite = endingSprites[1];
            ending2.GetComponentInChildren<TextMeshProUGUI>().text = endingNames[1];
        }

        if (ending3Reached > 0)
        {
            //instantiate the prefab for ending 3
            GameObject ending3 = Instantiate(endingPrefab, endingDisplayArea.transform);
            ending3.GetComponentInChildren<Image>().sprite = endingSprites[2];
            ending3.GetComponentInChildren<TextMeshProUGUI>().text = endingNames[2];
        }
    }

    public void CloseArchives()
    {
        // Close the how-to-play panel
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        gameObject.SetActive(false);
        // Debug.Log("How to play panel closed");
    }
}
