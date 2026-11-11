
using TMPro;
using UnityEngine;

/// <summary>
/// Updates the on-screen HUD for the Physical Computing template.
///
/// Displays:
/// - Current score
/// - Prop activation/tracking status
/// - Time remaining
/// - Current input mode
/// - Controls for the selected input mode
///
/// Instructions automatically change when switching between
/// DebugKeyboard and MocapLive.
///
/// Students can customize the HUD without changing the
/// underlying gameplay or motion capture systems.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("HUD Text References")]

    // Displays the player's current score.
    public TextMeshProUGUI scoreText;

    // Displays whether the gameplay prop is active or inactive.
    public TextMeshProUGUI propStateText;

    // Displays the remaining time in the round.
    public TextMeshProUGUI timerText;

    // Displays the currently selected input mode.
    public TextMeshProUGUI inputModeText;

    // Displays the restart and input switching controls.
    public TextMeshProUGUI resetText;

    // Displays instructions appropriate to the input mode.
    public TextMeshProUGUI instructionsText;

    [Header("Scene References")]

    // The visible gameplay prop.
    public GameObject trackedProp;

    // Provides information about the selected input mode.
    public InputManager inputManager;

    // Stores the PropState component on the gameplay prop.
    private PropState propState;

    void Start()
    {
        // Find the component that stores activation information.
        if (trackedProp != null)
        {
            propState = trackedProp.GetComponent<PropState>();
        }
    }

    void Update()
    {
        // The HUD depends on GameManager for score and time.
        if (GameManager.instance == null)
        {
            return;
        }

        // Determine which input mode is currently selected.
        bool isMocapLive =
            inputManager != null &&
            inputManager.currentMode == InputManager.InputMode.MocapLive;

        // Determine whether the round is still running.
        bool roundRunning = GameManager.instance.gameRunning;

        // ---------------------------------------
        // SCORE
        // ---------------------------------------

        if (scoreText != null)
        {
            scoreText.text =
                "Score: " + GameManager.instance.score;
        }

        // ---------------------------------------
        // TIMER
        // ---------------------------------------

        if (timerText != null)
        {
            // Prevent negative numbers when the timer expires.
            int remainingSeconds = Mathf.Max(
                0,
                Mathf.CeilToInt(GameManager.instance.roundTime)
            );

            // Display the remaining time as a whole number.
            timerText.text = "Time: " + remainingSeconds;
        }

        // ---------------------------------------
        // PROP STATUS
        // ---------------------------------------

        if (propStateText != null)
        {
            bool isActive =
                propState != null && propState.propActive;

            if (!roundRunning)
            {
                propStateText.text = "Prop: INACTIVE";
            }
            else if (isActive)
            {
                propStateText.text = "Prop: ACTIVE";
            }
            else if (isMocapLive)
            {
                // In live mocap mode, inactivity means that
                // current tracking has not been confirmed.
                propStateText.text = "Prop: NOT TRACKED";
            }
            else
            {
                // In keyboard mode, SPACE controls activation.
                propStateText.text = "Prop: INACTIVE";
            }
        }

        // ---------------------------------------
        // INPUT MODE
        // ---------------------------------------

        if (inputModeText != null)
        {
            inputModeText.text = isMocapLive
                ? "Mode: Motion Capture"
                : "Mode: Debug Keyboard";
        }

        // ---------------------------------------
        // DYNAMIC INSTRUCTIONS
        // ---------------------------------------

        if (instructionsText != null)
        {
            if (!roundRunning)
            {
                instructionsText.text =
                    "Round Over!";
            }
            else if (isMocapLive)
            {
                instructionsText.text =
                    "Move the physical prop to play\n" +
                    "Tracking detected = ACTIVE";
            }
            else
            {
                instructionsText.text =
                    "WASD: Move   |   SPACE: Activate";
            }
        }

        // ---------------------------------------
        // RESET / MODE SWITCHING
        // ---------------------------------------

        if (resetText != null)
        {
            resetText.text =
                "M: Switch Mode   |   R: Restart";
        }
    }
}
