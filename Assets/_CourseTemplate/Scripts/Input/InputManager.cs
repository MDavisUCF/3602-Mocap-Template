using UnityEngine;

/// <summary>
/// Handles the different ways the tracked prop can be controlled.
/// 
/// This script does not directly handle scoring or gameplay zone behavior.
/// Its main job is to move Target_MocapInput, then tell Player_TrackedProp
/// where to go and whether it should be active.
/// 
/// Current input modes:
/// - DebugKeyboard: player moves the prop with WASD
/// - MocapPlaceholder: prop moves automatically in a circle
/// - MocapLive: prop follows live OptiTrack/Motive tracking data
/// </summary>
public class InputManager : MonoBehaviour
{
    // PlayerPrefs key used to remember the last selected input mode.
    // This allows the project to reopen or reset using the most recent mode.
    private const string InputModePrefKey = "LastInputMode";

    [Header("Tracked Prop")]

    // The visible gameplay prop that interacts with zones.
    // InputManager sends movement and activation data to this object.
    public TrackedPropController trackedProp;

    [Header("Targets")]

    // The invisible input anchor that the tracked prop follows.
    // Keyboard input, placeholder motion, and live mocap all move this object.
    public Transform mocapTarget;

    // The OptiTrack-driven object that receives live rigid body tracking data.
    // This is only used in MocapLive mode.
    public Transform liveMocapSource;

    /// <summary>
    /// The available input/control modes for the template.
    /// </summary>
    public enum InputMode
    {
        // Manual testing mode using WASD and SPACE.
        DebugKeyboard,

        // Simulated mocap mode using automatic circular movement.
        MocapPlaceholder,

        // Live OptiTrack/Motive mode using the live mocap source transform.
        MocapLive
    }

    [Header("Current Input Mode")]

    // Determines which input mode is currently controlling the prop.
    public InputMode currentMode = InputMode.DebugKeyboard;

    [Header("Debug Keyboard")]

    // Movement speed used for WASD keyboard testing.
    public float moveSpeed = 5f;

    [Header("Mocap Placeholder")]

    // How wide the placeholder circle movement is.
    public float placeholderRadius = 3f;

    // How fast the placeholder circle movement happens.
    public float placeholderSpeed = 1f;

    // How high the placeholder target should stay above the floor.
    public float placeholderHeight = 1f;

    [Header("Live Mocap")]

    // Optional position adjustment for live mocap data.
    // Use this if the OptiTrack position needs to be shifted in the Unity scene.
    public Vector3 liveMocapPositionOffset = Vector3.zero;

    // Optional rotation adjustment for live mocap data.
    // Use this if the tracked prop appears rotated incorrectly.
    public Vector3 liveMocapRotationOffset = Vector3.zero;

    // Optional scale adjustment for live mocap movement.
    // Increase or decrease this if mocap movement feels too large or too small in Unity.
    public float liveMocapScale = 1f;

    // Tracks whether the prop is currently active.
    // In this template, SPACE controls activation.
    private bool propActive;

    void Start()
    {
        // At the start of the scene, place the input target at the prop's current position.
        // This prevents the prop from snapping somewhere unexpected on the first frame.
        if (trackedProp != null && mocapTarget != null)
        {
            mocapTarget.position = trackedProp.transform.position;
        }

        // If an input mode was saved from a previous play session, load it.
        // This is why the template can remember the last selected mode after reset.
        if (PlayerPrefs.HasKey(InputModePrefKey))
        {
            currentMode = (InputMode)PlayerPrefs.GetInt(InputModePrefKey);
        }
    }

    void Update()
    {
        // Press M to cycle through the input modes.
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleInputMode();
        }

        // If required references are missing, stop here to avoid errors.
        if (trackedProp == null || mocapTarget == null)
        {
            return;
        }

        // If the round is over, force the prop to become inactive.
        // This prevents scoring or danger interactions after time runs out.
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            propActive = false;
            trackedProp.ApplyInput(mocapTarget.position, false, currentMode);
            return;
        }

        // Run the correct input update based on the current mode.
        switch (currentMode)
        {
            case InputMode.DebugKeyboard:
                UpdateDebugKeyboardInput();
                break;

            case InputMode.MocapPlaceholder:
                UpdateMocapPlaceholderInput();
                break;

            case InputMode.MocapLive:
                UpdateLiveMocapInput();
                break;
        }

        // After the input mode updates the target, send the final position and active state
        // to the visible gameplay prop.
        trackedProp.ApplyInput(mocapTarget.position, propActive, currentMode);
    }

    /// <summary>
    /// Manual keyboard testing mode.
    /// WASD moves the target, and SPACE activates the prop.
    /// </summary>
    void UpdateDebugKeyboardInput()
    {
        Vector3 move = Vector3.zero;

        // Build a movement direction based on which keys are currently held.
        if (Input.GetKey(KeyCode.W)) move += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) move += Vector3.back;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;

        // Move the target using normalized movement so diagonal movement is not faster.
        mocapTarget.position += move.normalized * moveSpeed * Time.deltaTime;

        // SPACE acts as the prop's "active/use" input.
        propActive = Input.GetKey(KeyCode.Space);
    }

    /// <summary>
    /// Fake mocap mode used for testing without hardware.
    /// The target moves in a circle to simulate incoming tracking data.
    /// </summary>
    void UpdateMocapPlaceholderInput()
    {
        float x = Mathf.Sin(Time.time * placeholderSpeed) * placeholderRadius;
        float z = Mathf.Cos(Time.time * placeholderSpeed) * placeholderRadius;

        // Move the target in a circle at a fixed height.
        mocapTarget.position = new Vector3(x, placeholderHeight, z);

        // SPACE still controls prop activation in placeholder mode.
        propActive = Input.GetKey(KeyCode.Space);
    }

    /// <summary>
    /// Live mocap mode.
    /// Uses the OptiTrack-driven source object as the input source.
    /// </summary>
    void UpdateLiveMocapInput()
    {
        // If no live mocap source is assigned, warn the user and fall back to placeholder movement.
        // This keeps the template from appearing completely broken when mocap is not connected.
        if (liveMocapSource == null)
        {
            Debug.LogWarning("MocapLive mode is active, but no liveMocapSource is assigned.");

            UpdateMocapPlaceholderInput();
            return;
        }

        // Convert the live mocap source position into the position used by the Unity template.
        // Scale happens first, then offset is added.
        Vector3 mappedPosition = liveMocapSource.position * liveMocapScale + liveMocapPositionOffset;

        // Apply the mapped mocap position and rotation to the target.
        mocapTarget.position = mappedPosition;
        mocapTarget.rotation = liveMocapSource.rotation * Quaternion.Euler(liveMocapRotationOffset);

        // For now, SPACE still acts as the prop's "active/use" input.
        // Later, this could be replaced with a physical button, sensor, gesture, or distance check.
        propActive = Input.GetKey(KeyCode.Space);
    }

    /// <summary>
    /// Cycles through the available input modes.
    /// The order is:
    /// DebugKeyboard → MocapPlaceholder → MocapLive → DebugKeyboard
    /// </summary>
    void ToggleInputMode()
    {
        if (currentMode == InputMode.DebugKeyboard)
        {
            currentMode = InputMode.MocapPlaceholder;
            Debug.Log("Input Mode: Mocap Placeholder");
        }
        else if (currentMode == InputMode.MocapPlaceholder)
        {
            currentMode = InputMode.MocapLive;
            Debug.Log("Input Mode: Mocap Live");
        }
        else
        {
            currentMode = InputMode.DebugKeyboard;
            Debug.Log("Input Mode: Debug Keyboard");
        }

        // Save the selected mode so it persists after scene reset.
        PlayerPrefs.SetInt(InputModePrefKey, (int)currentMode);
        PlayerPrefs.Save();

        // Play a small feedback sound when the input mode changes.
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayInputModeSound();
        }
    }

    /// <summary>
    /// Returns the current input mode as text.
    /// UIManager can use this to display the mode on the HUD.
    /// </summary>
    public string GetInputModeName()
    {
        return currentMode.ToString();
    }
}