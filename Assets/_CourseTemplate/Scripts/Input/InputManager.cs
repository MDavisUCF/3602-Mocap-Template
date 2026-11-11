
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
/// - MocapLive: prop follows live OptiTrack/Motive tracking data
///
/// In MocapLive mode:
/// - Tracking detected = ACTIVE
/// - Tracking lost = INACTIVE
///
/// TrackedPropController handles the visible model and its colliders.
/// </summary>
public class InputManager : MonoBehaviour
{
    // PlayerPrefs remembers the selected input mode after scene resets.
    private const string InputModePrefKey = "LastInputMode";

    [Header("Tracked Prop")]

    // The visible gameplay prop that interacts with zones.
    public TrackedPropController trackedProp;

    [Header("Targets")]

    // The invisible target that the gameplay prop follows.
    // Both keyboard and live mocap modes move this object.
    public Transform mocapTarget;

    // The GameObject controlled by the OptiTrack rigid body component.
    // This provides the position and rotation of the physical prop.
    public Transform liveMocapSource;

    /// <summary>
    /// The available input modes.
    ///
    /// Explicit numbers preserve compatibility with previously saved modes.
    /// The old MocapPlaceholder mode used value 1 and is no longer supported.
    /// </summary>
    public enum InputMode
    {
        DebugKeyboard = 0,
        MocapLive = 2
    }

    [Header("Current Input Mode")]

    // Determines which input system controls the gameplay prop.
    public InputMode currentMode = InputMode.DebugKeyboard;

    [Header("Debug Keyboard")]

    // How fast the prop moves when using WASD.
    public float moveSpeed = 5f;

    [Header("Live Mocap")]

    // Shifts the received mocap position within the Unity scene.
    public Vector3 liveMocapPositionOffset = Vector3.zero;

    // Adjusts the orientation of the tracked physical prop.
    public Vector3 liveMocapRotationOffset = Vector3.zero;

    // Adjusts the size of the movement received from Motive.
    public float liveMocapScale = 1f;

    [Header("Live Tracking Detection")]

    // The OptiTrack rigid body component receiving Motive tracking data.
    // Assign Source_OptiTrackRigidBody in the Inspector.
    public OptitrackRigidBody liveRigidBody;

    // Maximum time without fresh tracking data before the prop
    // is considered lost.
    //
    // A short delay helps prevent temporary tracking interruptions
    // from immediately deactivating the prop.
    [Range(0.05f, 1f)]
    public float trackingTimeout = 0.25f;

    // Stores whether the gameplay prop is currently active.
    //
    // DebugKeyboard uses SPACE.
    // MocapLive uses tracking detection.
    private bool propActive;

    void Start()
    {
        // Start the input target at the gameplay prop's position.
        if (trackedProp != null && mocapTarget != null)
        {
            mocapTarget.position = trackedProp.transform.position;
        }

        // Restore the last selected input mode if one was saved.
        if (PlayerPrefs.HasKey(InputModePrefKey))
        {
            currentMode =
                (InputMode)PlayerPrefs.GetInt(InputModePrefKey);
        }

        // Older versions included MocapPlaceholder (value 1).
        // If an unsupported mode was saved or assigned,
        // return to DebugKeyboard instead.
        if (!System.Enum.IsDefined(typeof(InputMode), currentMode))
        {
            currentMode = InputMode.DebugKeyboard;
        }

        // Save the validated mode for future scene resets.
        PlayerPrefs.SetInt(InputModePrefKey, (int)currentMode);
        PlayerPrefs.Save();
    }

    void Update()
    {
        // Press M to switch between keyboard and live mocap.
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleInputMode();
        }

        // Stop if either required gameplay reference is missing.
        if (trackedProp == null || mocapTarget == null)
        {
            return;
        }

        // If the round has ended, force the prop to become inactive.
        // This prevents gameplay interactions after time runs out.
        if (GameManager.instance != null &&
            !GameManager.instance.gameRunning)
        {
            propActive = false;

            trackedProp.ApplyInput(
                mocapTarget.position,
                false,
                currentMode,
                mocapTarget.rotation
            );

            return;
        }

        // Run the movement system for the selected input mode.
        switch (currentMode)
        {
            case InputMode.DebugKeyboard:
                UpdateDebugKeyboardInput();
                break;

            case InputMode.MocapLive:
                UpdateLiveMocapInput();
                break;
        }

        // Send movement, rotation, input mode, and activation
        // information to the gameplay prop.
        //
        // TrackedPropController handles:
        // - moving the visible model
        // - applying rotation
        // - changing materials
        // - playing activation sounds
        // - hiding the model when tracking is lost
        trackedProp.ApplyInput(
            mocapTarget.position,
            propActive,
            currentMode,
            mocapTarget.rotation
        );
    }

    /// <summary>
    /// Moves the prop using keyboard controls.
    ///
    /// WASD = Movement
    /// SPACE = Activate prop
    /// </summary>
    void UpdateDebugKeyboardInput()
    {
        Vector3 move = Vector3.zero;

        // Determine the movement direction.
        if (Input.GetKey(KeyCode.W)) move += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) move += Vector3.back;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;

        // Normalize movement so diagonal movement is not faster.
        // Time.deltaTime keeps movement consistent across frame rates.
        mocapTarget.position +=
            move.normalized * moveSpeed * Time.deltaTime;

        // The prop is active only while SPACE is held.
        propActive = Input.GetKey(KeyCode.Space);
    }

    /// <summary>
    /// Checks whether Motive has recently provided tracking data
    /// for the assigned physical rigid body.
    ///
    /// This checks how recently tracking was received,
    /// not whether the physical prop is moving.
    ///
    /// A stationary prop can still be tracked and active.
    /// </summary>
    public bool HasLiveTracking()
    {
        // Confirm the rigid body and streaming client exist.
        if (liveRigidBody == null ||
            liveRigidBody.StreamingClient == null)
        {
            return false;
        }

        // Get the latest received rigid body state.
        //
        // False requests the received pose without
        // network compensation or prediction.
        OptitrackRigidBodyState state =
            liveRigidBody.StreamingClient.GetLatestRigidBodyState(
                liveRigidBody.RigidBodyId,
                false
            );

        // No rigid body state means tracking is unavailable.
        if (state == null)
        {
            return false;
        }

        // Check how old the latest tracking data is.
        float age = state.DeliveryTimestamp.AgeSeconds;

        // If the data is recent enough, consider tracking active.
        return age >= 0f && age <= trackingTimeout;
    }

    /// <summary>
    /// Controls the prop using live OptiTrack/Motive data.
    ///
    /// Unlike keyboard mode, SPACE is not required.
    ///
    /// Tracking detected = ACTIVE
    /// Tracking lost = INACTIVE
    /// </summary>
    void UpdateLiveMocapInput()
    {
        // Assume the prop is inactive until tracking is confirmed.
        propActive = false;

        // Without a source object, live movement is unavailable.
        if (liveMocapSource == null)
        {
            return;
        }

        // If no recent tracking is available, remain inactive.
        //
        // The last known position is preserved.
        // TrackedPropController handles hiding the model
        // and disabling its gameplay colliders.
        if (!HasLiveTracking())
        {
            return;
        }

        // Motive is currently providing tracking data.
        // Activate the gameplay prop automatically.
        propActive = true;

        // Convert the physical prop's position into Unity space.
        Vector3 mappedPosition =
            liveMocapSource.position * liveMocapScale
            + liveMocapPositionOffset;

        // Move the invisible input target.
        mocapTarget.position = mappedPosition;

        // Apply physical rotation with an optional adjustment.
        mocapTarget.rotation =
            liveMocapSource.rotation *
            Quaternion.Euler(liveMocapRotationOffset);
    }

    /// <summary>
    /// Switches between the two available input modes.
    ///
    /// DebugKeyboard -> MocapLive -> DebugKeyboard
    ///
    /// The selected mode is saved between scene resets.
    /// </summary>
    void ToggleInputMode()
    {
        if (currentMode == InputMode.DebugKeyboard)
        {
            currentMode = InputMode.MocapLive;
            Debug.Log("Input Mode: Mocap Live");
        }
        else
        {
            currentMode = InputMode.DebugKeyboard;
            Debug.Log("Input Mode: Debug Keyboard");
        }

        // Save the selected mode.
        PlayerPrefs.SetInt(InputModePrefKey, (int)currentMode);
        PlayerPrefs.Save();

        // Play feedback when switching modes.
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayInputModeSound();
        }
    }

    /// <summary>
    /// Returns the current input mode as text.
    /// UIManager uses this to display the mode on the HUD.
    /// </summary>
    public string GetInputModeName()
    {
        return currentMode.ToString();
    }
}
