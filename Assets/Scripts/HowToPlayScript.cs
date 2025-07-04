using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayScript : MonoBehaviour
{
    public void CloseHowToPlay()
    {
        // Close the how-to-play panel
        gameObject.SetActive(false);
        Debug.Log("How to play panel closed");
    }
}
