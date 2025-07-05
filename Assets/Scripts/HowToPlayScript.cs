using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayScript : MonoBehaviour
{
    public Button closeButton;

    public void OnEnable()
    {
        // Initialize the how-to-play panel
        closeButton.onClick.AddListener(CloseHowToPlay);
        // Debug.Log("How to play panel opened");
    }

    public void CloseHowToPlay()
    {
        // Close the how-to-play panel
        gameObject.SetActive(false);
        // Debug.Log("How to play panel closed");
    }
}
