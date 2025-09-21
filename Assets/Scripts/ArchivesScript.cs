using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchivesScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    public int endingCount = 0;
    public Button closeButton;
    public TextMeshProUGUI endingCountText;
    public Sprite[] endingSprites;
    public string[] endingNames;
    public GameObject endingDisplayArea; 
    public GameObject endingPrefab; 

    public void OnEnable()
    {
        if(audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        // Initialize the how-to-play panel
        closeButton.onClick.AddListener(CloseArchives);
        DisplayReachedEndings();
    }

    public void DisplayReachedEndings()
    {
        // Clear the previous endings displayed
        foreach (Transform child in endingDisplayArea.transform)
        {
            Destroy(child.gameObject);
        }

        // Display the endings that have been reached
        endingCount = 0;
        
        // Loop through all endings
        for (int i = 1; i <= 3; i++)
        {
            int endingReached = PlayerPrefs.GetInt($"Ending{i}Reached", 0);
            
            if (endingReached > 0)
            {
                endingCount++; // Count unique endings reached
                
                // Instantiate the prefab for this ending
                GameObject ending = Instantiate(endingPrefab, endingDisplayArea.transform);
                ending.GetComponentInChildren<Image>().sprite = endingSprites[i - 1];
                ending.GetComponentInChildren<TextMeshProUGUI>().text = endingNames[i - 1];
            }
        }

        endingCountText.text = "Endings Reached (" + endingCount + " / 3)";
    }

    public void CloseArchives()
    {
        // Close the how-to-play panel
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        gameObject.SetActive(false);
    }
}
