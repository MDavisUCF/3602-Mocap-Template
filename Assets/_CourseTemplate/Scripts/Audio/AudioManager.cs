using UnityEngine;

/// <summary>
/// Handles all simple sound effect playback for the template.
/// 
/// Other scripts do not need to know how audio works.
/// They can simply call methods like:
/// AudioManager.instance.PlayScoreSound();
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Static instance allows other scripts to access this AudioManager easily.
    // Example: AudioManager.instance.PlayScoreSound();
    public static AudioManager instance;

    [Header("Audio Source")]

    // The AudioSource is the Unity component that actually plays sounds.
    // This should be assigned in the Inspector, or the script will try to find one automatically.
    public AudioSource audioSource;

    [Header("Gameplay Sounds")]

    // These are the sound clips used by different gameplay events.
    // Drag audio files into these slots in the Inspector.
    public AudioClip scoreClip;
    public AudioClip dangerClip;
    public AudioClip propActivateClip;
    public AudioClip propDeactivateClip;
    public AudioClip inputModeClip;
    public AudioClip resetClip;

    [Header("Settings")]

    // Controls how loud sound effects are.
    // Range keeps the value between 0 and 1 in the Inspector.
    [Range(0f, 1f)]
    public float soundVolume = 0.7f;

    void Awake()
    {
        // This makes sure there is only one AudioManager in the scene.
        // If another one appears, it destroys the duplicate.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Store this AudioManager so other scripts can find it.
        instance = this;

        // If no AudioSource was assigned manually, try to find one on this same GameObject.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Plays when the player successfully scores.
    public void PlayScoreSound()
    {
        PlaySound(scoreClip);
    }

    // Plays when the player hits a danger zone.
    public void PlayDangerSound()
    {
        PlaySound(dangerClip);
    }

    // Plays when the prop becomes active.
    public void PlayPropActivateSound()
    {
        PlaySound(propActivateClip);
    }

    // Plays when the prop becomes inactive.
    public void PlayPropDeactivateSound()
    {
        PlaySound(propDeactivateClip);
    }

    // Plays when the input mode changes, such as switching from keyboard to mocap mode.
    public void PlayInputModeSound()
    {
        PlaySound(inputModeClip);
    }

    // Plays when the scene resets.
    public void PlayResetSound()
    {
        PlaySound(resetClip);
    }

    /// <summary>
    /// Plays one sound effect without interrupting other sounds.
    /// This is private because other scripts should use the specific sound methods above.
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        // If there is no AudioSource or no clip assigned, do nothing.
        // This prevents the game from throwing errors if a sound is missing.
        if (audioSource == null || clip == null)
        {
            return;
        }

        // PlayOneShot plays the sound once.
        // It is useful for short sound effects like clicks, beeps, and score sounds.
        audioSource.PlayOneShot(clip, soundVolume);
    }
}