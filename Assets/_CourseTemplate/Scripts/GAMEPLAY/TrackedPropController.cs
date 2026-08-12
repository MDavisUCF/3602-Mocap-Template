using UnityEngine;

/// <summary>
/// Controls the visible gameplay prop.
/// 
/// This script receives input information from InputManager, then:
/// - moves the prop to the target position
/// - updates whether the prop is active
/// - plays activation/deactivation sounds
/// - changes the prop material based on input mode and active state
/// </summary>
public class TrackedPropController : MonoBehaviour
{
    [Header("Keyboard Mode Materials")]

    // Material used when the prop is in DebugKeyboard mode and inactive.
    public Material keyboardInactiveMaterial;

    // Material used when the prop is in DebugKeyboard mode and active.
    public Material keyboardActiveMaterial;

    [Header("Mocap Mode Materials")]

    // Material used when the prop is in MocapPlaceholder or MocapLive mode and inactive.
    public Material mocapInactiveMaterial;

    // Material used when the prop is in MocapPlaceholder or MocapLive mode and active.
    public Material mocapActiveMaterial;

    // The MeshRenderer controls what material/color appears on the prop.
    private MeshRenderer meshRenderer;

    // PropState stores whether the prop is currently active or inactive.
    private PropState propState;

    // Tracks the previous active state.
    // This lets us detect the exact moment the prop changes from inactive to active,
    // or from active to inactive.
    private bool wasActiveLastFrame;

    void Awake()
    {
        // Find the MeshRenderer attached to this same GameObject.
        meshRenderer = GetComponent<MeshRenderer>();

        // Find the PropState attached to this same GameObject.
        propState = GetComponent<PropState>();

        // Start wasActiveLastFrame using the prop's current state.
        // This prevents the activate/deactivate sound from playing incorrectly on the first frame.
        if (propState != null)
        {
            wasActiveLastFrame = propState.propActive;
        }
    }

    /// <summary>
    /// Applies movement and active-state input to the prop.
    /// 
    /// InputManager calls this every frame and tells the prop:
    /// - where it should move
    /// - whether it should be active
    /// - which input mode is currently being used
    /// </summary>
    public void ApplyInput(Vector3 targetPosition, bool isActive, InputManager.InputMode inputMode)
    {
        // Move the visible prop to the current target position.
        transform.position = targetPosition;

        // Update the stored prop state so other scripts can check if the prop is active.
        if (propState != null)
        {
            propState.propActive = isActive;
        }

        // Only play activation/deactivation sounds when the state changes.
        // This prevents the sound from repeating every frame while SPACE is held.
        if (isActive != wasActiveLastFrame)
        {
            if (AudioManager.instance != null)
            {
                if (isActive)
                {
                    AudioManager.instance.PlayPropActivateSound();
                }
                else
                {
                    AudioManager.instance.PlayPropDeactivateSound();
                }
            }
        }

        // Save this frame's active state so we can compare it next frame.
        wasActiveLastFrame = isActive;

        // Update the prop's material so players can see the current mode/state.
        if (meshRenderer != null)
        {
            if (inputMode == InputManager.InputMode.DebugKeyboard)
            {
                // Keyboard mode uses keyboard-specific materials.
                meshRenderer.material = isActive ? keyboardActiveMaterial : keyboardInactiveMaterial;
            }
            else
            {
                // Both mocap modes use mocap-specific materials.
                meshRenderer.material = isActive ? mocapActiveMaterial : mocapInactiveMaterial;
            }
        }
    }

    /// <summary>
    /// Allows other scripts, such as ScoreZone and DangerZone, to check if the prop is active.
    /// </summary>
    public bool IsActive()
    {
        return propState != null && propState.propActive;
    }
}