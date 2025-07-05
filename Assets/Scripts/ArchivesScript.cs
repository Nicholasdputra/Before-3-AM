using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchivesScript : MonoBehaviour
{
    public Button closeButton;

    public void OnEnable()
    {
        // Initialize the how-to-play panel
        closeButton.onClick.AddListener(CloseArchives);
        // Debug.Log("How to play panel opened");
    }

    public void CloseArchives()
    {
        // Close the how-to-play panel
        gameObject.SetActive(false);
        // Debug.Log("How to play panel closed");
    }
}
