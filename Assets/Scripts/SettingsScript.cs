using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsScript : MonoBehaviour
{
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
        LoadSettings();
        bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        dialogueVolumeSlider.onValueChanged.AddListener(SetDialogueVolume);
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
        // Set the BGM volume on the specific AudioSource
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
        }
        Debug.Log("BGM Volume set to: " + volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = volume;
        }
        Debug.Log("SFX Volume set to: " + volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetDialogueVolume(float volume)
    {
        if (dialogueAudioSource != null)
        {
            dialogueAudioSource.volume = volume;
        }
        Debug.Log("Dialogue Volume set to: " + volume);
        PlayerPrefs.SetFloat("DialogueVolume", volume);
    }
    
    public void CloseSettings()
    {
        Debug.Log("Closing window");
        gameObject.SetActive(false);
    }
}