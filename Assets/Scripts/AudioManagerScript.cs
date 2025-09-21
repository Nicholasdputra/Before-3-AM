using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript Instance { get; private set; }
    [Header("Audio Sources")]
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource dialogueAudioSource;

    [Header("Audio Clips")]
    public AudioClip mainMenuBGMClip;
    public AudioClip gameBGMClip;
    public AudioClip gameOverBGMClip;

    [Header("Sound Effects")]
    public AudioClip buttonClickSFXClip;
    public AudioClip clockTickSFXClip;
    public AudioClip paperRuffleSFXClip;
    public AudioClip before3AmSFXClip;

    [Header("Dialogue Clips")]
    public AudioClip nikolasDialogueClip;
    public AudioClip renattaDialogueClip;
    public AudioClip gunawanDialogueClip;

    private void Awake()
    {
        Debug.Log("AudioManager Awake called");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize audio sources
        bgmAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
        sfxAudioSource = transform.GetChild(1).GetComponent<AudioSource>();  
        dialogueAudioSource = transform.GetChild(2).GetComponent<AudioSource>();
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }
    
    public void PlayDialogue(AudioClip clip)
    {
        dialogueAudioSource.PlayOneShot(clip);
    }
}