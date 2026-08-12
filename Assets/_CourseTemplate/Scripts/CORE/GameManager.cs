using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score;
    public float roundTime = 60f;
    public bool gameRunning = true;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (gameRunning)
        {
            roundTime -= Time.deltaTime;

            if (roundTime <= 0)
            {
                roundTime = 0;
                gameRunning = false;
                Debug.Log("Round Over! Final Score: " + score);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    public void AddScore(int amount)
    {
        if (!gameRunning) return;

        score += amount;
        Debug.Log("Score: " + score);
    }

    public void ResetGame()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayResetSound();
        }

        Invoke(nameof(ReloadCurrentScene), .65f);
    }

    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}