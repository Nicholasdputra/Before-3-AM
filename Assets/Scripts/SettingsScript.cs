using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    public static SettingsScript Instance;

    [SerializeField] Slider bgmVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider dialogueVolumeSlider;

    [SerializeField] Button backButton;
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource dialogueAudioSource; 

    void Start()
    {
        if(audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        LoadSettings();
        
        // Add null checks and debug logs
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
            Debug.Log("BGM slider listener added");
        }
        else
        {
            Debug.LogError("BGM Volume Slider is null!");
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            Debug.Log("SFX slider listener added");
        }
        else
        {
            Debug.LogError("SFX Volume Slider is null!");
        }
        
        if (dialogueVolumeSlider != null)
        {
            dialogueVolumeSlider.onValueChanged.AddListener(SetDialogueVolume);
            Debug.Log("Dialogue slider listener added");
        }
        else
        {
            Debug.LogError("Dialogue Volume Slider is null!");
        }
        
        backButton.onClick.AddListener(CloseSettings);
    }

    public void LoadSettings()
    {
        // Load saved settings or set default values
        bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        dialogueVolumeSlider.value = PlayerPrefs.GetFloat("DialogueVolume", 1f);
    }

    public void SetBGMVolume(float volume)
    {
        // Set the BGM volume on the AudioManager's AudioSource
        if (audioManager != null)
        {
            // Access the AudioManager's BGM AudioSource (first child)
            AudioSource bgmSource = audioManager.transform.GetChild(0).GetComponent<AudioSource>();
            if (bgmSource != null)
            {
                bgmSource.volume = volume;
                Debug.Log("Setting BGM volume to: " + volume);
            }
        }
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        // Set the SFX volume on the AudioManager's AudioSource
        if (audioManager != null)
        {
            // Access the AudioManager's SFX AudioSource (second child)
            AudioSource sfxSource = audioManager.transform.GetChild(1).GetComponent<AudioSource>();
            if (sfxSource != null)
            {
                sfxSource.volume = volume;
                Debug.Log("Setting SFX volume to: " + volume);
            }
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetDialogueVolume(float volume)
    {
        // Set the Dialogue volume on the AudioManager's AudioSource
        if (audioManager != null)
        {
            // Access the AudioManager's Dialogue AudioSource (third child)
            AudioSource dialogueSource = audioManager.transform.GetChild(2).GetComponent<AudioSource>();
            if (dialogueSource != null)
            {
                dialogueSource.volume = volume;
                Debug.Log("Setting Dialogue volume to: " + volume);
            }
        }
        PlayerPrefs.SetFloat("DialogueVolume", volume);
    }
    
    public void CloseSettings()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        Debug.Log("Closing window");
        gameObject.SetActive(false);
    }
}