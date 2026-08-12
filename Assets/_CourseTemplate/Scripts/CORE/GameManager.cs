using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the basic game rules for the template.
/// 
/// This script keeps track of:
/// - the player's score
/// - the round timer
/// - whether the game is currently running
/// - scene reset behavior
/// </summary>
public class GameManager : MonoBehaviour
{
    // Static instance allows other scripts to easily find the GameManager.
    // Example: GameManager.instance.AddScore(1);
    public static GameManager instance;

    [Header("Game State")]

    // Current player score.
    public int score;

    // How long the round lasts, in seconds.
    public float roundTime = 60f;

    // If true, the game is currently active.
    // If false, scoring and timer updates should stop.
    public bool gameRunning = true;

    void Awake()
    {
        // Save this GameManager as the shared instance.
        // Other scripts can access it through GameManager.instance.
        instance = this;
    }

    void Update()
    {
        // Only count down the timer while the game is running.
        if (gameRunning)
        {
            // Time.deltaTime is the amount of time that passed since the last frame.
            // Subtracting it every frame creates a countdown timer.
            roundTime -= Time.deltaTime;

            // When the timer reaches 0, stop the game.
            if (roundTime <= 0)
            {
                roundTime = 0;
                gameRunning = false;

                Debug.Log("Round Over! Final Score: " + score);
            }
        }

        // Press R to reset/reload the scene.
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    /// <summary>
    /// Adds or subtracts points from the score.
    /// Positive numbers increase score.
    /// Negative numbers decrease score.
    /// </summary>
    public void AddScore(int amount)
    {
        // If the game is over, do not allow the score to change.
        if (!gameRunning) return;

        score += amount;

        Debug.Log("Score: " + score);
    }

    /// <summary>
    /// Plays the reset sound, then reloads the current scene.
    /// 
    /// The scene reload is delayed slightly so the sound is not cut off immediately.
    /// </summary>
    public void ResetGame()
    {
        // Play reset sound if an AudioManager exists in the scene.
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayResetSound();
        }

        // Wait briefly before reloading so the reset sound has time to play.
        Invoke(nameof(ReloadCurrentScene), .65f);
    }

    /// <summary>
    /// Reloads the currently active scene.
    /// This resets the game objects back to their starting state.
    /// </summary>
    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}