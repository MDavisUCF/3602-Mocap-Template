using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Gameplay Sounds")]
    public AudioClip scoreClip;
    public AudioClip dangerClip;
    public AudioClip propActivateClip;
    public AudioClip propDeactivateClip;
    public AudioClip inputModeClip;
    public AudioClip resetClip;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float soundVolume = 0.7f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayScoreSound()
    {
        PlaySound(scoreClip);
    }

    public void PlayDangerSound()
    {
        PlaySound(dangerClip);
    }

    public void PlayPropActivateSound()
    {
        PlaySound(propActivateClip);
    }

    public void PlayPropDeactivateSound()
    {
        PlaySound(propDeactivateClip);
    }

    public void PlayInputModeSound()
    {
        PlaySound(inputModeClip);
    }

    public void PlayResetSound()
    {
        PlaySound(resetClip);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip, soundVolume);
    }
}