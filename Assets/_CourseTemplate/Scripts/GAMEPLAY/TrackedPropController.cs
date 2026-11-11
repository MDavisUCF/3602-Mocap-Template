
using UnityEngine;

/// <summary>
/// Controls the visible gameplay prop.
///
/// This script receives movement and activation information
/// from InputManager and applies it to Player_TrackedProp.
///
/// DebugKeyboard:
/// - The prop is always visible.
/// - SPACE determines whether the prop is active.
/// - Only active props can interact with gameplay zones.
///
/// MocapLive:
/// - Tracking detected = visible and active.
/// - Tracking lost = hidden and inactive.
/// - Hidden props cannot interact with gameplay zones.
///
/// This script also handles rotation, materials, and
/// activation/deactivation sound effects.
/// </summary>
public class TrackedPropController : MonoBehaviour
{
    [Header("Keyboard Mode Materials")]

    // Materials used when testing with keyboard controls.
    public Material keyboardInactiveMaterial;
    public Material keyboardActiveMaterial;

    [Header("Live Mocap Materials")]

    // Materials used when controlling the prop with OptiTrack.
    public Material mocapInactiveMaterial;
    public Material mocapActiveMaterial;

    [Header("Mocap Rotation")]

    // When enabled, the visible prop rotates to match
    // the physical prop tracked by Motive.
    [Tooltip("Rotate the visible prop to match the physical prop.")]
    public bool followMocapRotation = true;

    // The MeshRenderer attached to the main gameplay prop.
    // Used to change its material based on input mode and state.
    private MeshRenderer meshRenderer;

    // Stores whether the prop is active or inactive.
    // Other scripts use PropState to check if interaction is allowed.
    private PropState propState;

    // Remembers the previous activation state.
    // Prevents activation sounds from repeating every frame.
    private bool wasActiveLastFrame;

    // Stores all renderers and colliders belonging to the prop.
    // This includes custom 3D models added as child objects.
    private Renderer[] propRenderers;
    private Collider[] propColliders;

    // Remembers which renderers and colliders were originally enabled.
    // This prevents the script from enabling components
    // that were intentionally disabled in the Inspector.
    private bool[] originalRendererStates;
    private bool[] originalColliderStates;

    void Awake()
    {
        // Find the components attached to the main prop.
        meshRenderer = GetComponent<MeshRenderer>();
        propState = GetComponent<PropState>();

        // Remember the starting activation state.
        if (propState != null)
        {
            wasActiveLastFrame = propState.propActive;
        }

        // Find all visual components and colliders,
        // including those on child objects.
        propRenderers = GetComponentsInChildren<Renderer>(true);
        propColliders = GetComponentsInChildren<Collider>(true);

        // Create arrays to remember their starting states.
        originalRendererStates = new bool[propRenderers.Length];
        originalColliderStates = new bool[propColliders.Length];

        // Remember which renderers were originally enabled.
        for (int i = 0; i < propRenderers.Length; i++)
        {
            originalRendererStates[i] = propRenderers[i].enabled;
        }

        // Remember which colliders were originally enabled.
        for (int i = 0; i < propColliders.Length; i++)
        {
            originalColliderStates[i] = propColliders[i].enabled;
        }
    }

    /// <summary>
    /// Applies movement, rotation, and activation information
    /// received from InputManager.
    ///
    /// targetPosition = Where the prop should move.
    /// isActive = Whether the prop can interact with gameplay.
    /// inputMode = Keyboard or live mocap.
    /// targetRotation = Rotation received from the input target.
    ///
    /// InputManager calls this method every frame.
    /// </summary>
    public void ApplyInput(
        Vector3 targetPosition,
        bool isActive,
        InputManager.InputMode inputMode,
        Quaternion targetRotation)
    {
        // Move the gameplay prop to the target position.
        transform.position = targetPosition;

        // Follow the physical prop's rotation only when
        // using MocapLive and rotation tracking is enabled.
        if (followMocapRotation &&
            inputMode == InputManager.InputMode.MocapLive)
        {
            transform.rotation = targetRotation;
        }

        // Update the prop's active/inactive state.
        // ScoreZone and DangerZone check this value.
        if (propState != null)
        {
            propState.propActive = isActive;
        }

        // Check whether activation changed since the last frame.
        // Sounds only play when the state actually changes.
        if (isActive != wasActiveLastFrame)
        {
            if (AudioManager.instance != null)
            {
                if (isActive)
                {
                    // The prop just became active.
                    AudioManager.instance.PlayPropActivateSound();
                }
                else
                {
                    // The prop just became inactive.
                    AudioManager.instance.PlayPropDeactivateSound();
                }
            }
        }

        // Remember the current state for the next frame.
        wasActiveLastFrame = isActive;

        // Update the original debug prop's material.
        // Custom child models can keep their own materials.
        if (meshRenderer != null)
        {
            if (inputMode == InputManager.InputMode.DebugKeyboard)
            {
                // Keyboard mode uses keyboard-specific materials.
                meshRenderer.material = isActive
                    ? keyboardActiveMaterial
                    : keyboardInactiveMaterial;
            }
            else
            {
                // Live mocap mode uses mocap-specific materials.
                meshRenderer.material = isActive
                    ? mocapActiveMaterial
                    : mocapInactiveMaterial;
            }
        }

        // Determine whether the prop should be visible.
        //
        // DebugKeyboard:
        // Always visible, even when inactive.
        //
        // MocapLive:
        // Visible only when tracking is detected.
        //
        // InputManager automatically sets isActive based
        // on the tracking status in MocapLive.
        bool shouldShow =
            inputMode != InputManager.InputMode.MocapLive
            || isActive;

        // Show or hide the model and its gameplay colliders.
        SetPropVisibility(shouldShow);
    }

    /// <summary>
    /// Shows or hides the prop and enables/disables its colliders.
    ///
    /// When the prop is hidden:
    /// - Its 3D model is invisible.
    /// - Its colliders are disabled.
    /// - It cannot enter ScoreZone or DangerZone.
    ///
    /// When tracking returns, the original enabled states
    /// of the renderers and colliders are restored.
    /// </summary>
    private void SetPropVisibility(bool visible)
    {
        // Show or hide the prop's visual components.
        for (int i = 0; i < propRenderers.Length; i++)
        {
            if (propRenderers[i] != null)
            {
                bool desiredState =
                    visible && originalRendererStates[i];

                // Only change the renderer if needed.
                if (propRenderers[i].enabled != desiredState)
                {
                    propRenderers[i].enabled = desiredState;
                }
            }
        }

        // Enable or disable the prop's gameplay colliders.
        //
        // This prevents an invisible prop from affecting
        // scoring or danger zones when tracking is lost.
        for (int i = 0; i < propColliders.Length; i++)
        {
            if (propColliders[i] != null)
            {
                bool desiredState =
                    visible && originalColliderStates[i];

                // Only change the collider if needed.
                if (propColliders[i].enabled != desiredState)
                {
                    propColliders[i].enabled = desiredState;
                }
            }
        }
    }

    /// <summary>
    /// Allows other scripts to check whether the prop is active.
    ///
    /// Returns true when the prop can interact with gameplay.
    /// Returns false when the prop is inactive.
    ///
    /// Used by ScoreZone and DangerZone.
    /// </summary>
    public bool IsActive()
    {
        return propState != null && propState.propActive;
    }
}
