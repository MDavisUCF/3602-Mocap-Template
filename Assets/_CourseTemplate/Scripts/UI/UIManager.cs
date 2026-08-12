using TMPro;
using UnityEngine;

/// <summary>
/// Updates the on-screen HUD text for the template.
/// 
/// This script displays:
/// - score
/// - prop active/inactive state
/// - timer
/// - current input mode
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("HUD Text References")]

    // Displays the player's current score.
    public TextMeshProUGUI scoreText;

    // Displays whether the tracked prop is active or inactive.
    public TextMeshProUGUI propStateText;

    // Displays the remaining time in the round.
    public TextMeshProUGUI timerText;

    // Displays the current input mode, such as DebugKeyboard or MocapPlaceholder.
    public TextMeshProUGUI inputModeText;

    [Header("Scene References")]

    // The gameplay prop object.
    // This is used so UIManager can find the PropState component.
    public GameObject trackedProp;

    // Reference to InputManager so the UI can display the current input mode.
    public InputManager inputManager;

    // Stores the PropState component found on the tracked prop.
    private PropState propState;

    void Start()
    {
        // Find the PropState component on the tracked prop at the start of the scene.
        // PropState stores whether the prop is currently active.
        if (trackedProp != null)
        {
            propState = trackedProp.GetComponent<PropState>();
        }
    }

    void Update()
    {
        // If there is no GameManager in the scene, stop here to avoid errors.
        if (GameManager.instance == null) return;

        // Update score text.
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.instance.score;
        }

        // Update prop state text.
        // If propState exists and propActive is true, show ACTIVE.
        // Otherwise, show Inactive.
        if (propStateText != null)
        {
            propStateText.text = propState != null && propState.propActive
                ? "Prop: ACTIVE"
                : "Prop: Inactive";
        }

        // Update timer text.
        // CeilToInt rounds up so the timer displays as whole seconds.
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(GameManager.instance.roundTime);
        }

        // Update input mode text.
        if (inputModeText != null && inputManager != null)
        {
            inputModeText.text = "Input Mode: " + inputManager.GetInputModeName();
        }
    }
}