using UnityEngine;

/// <summary>
/// Stores whether the tracked prop is currently active or inactive.
/// 
/// This script does not control movement, audio, scoring, or visuals.
/// It only stores the prop's current interaction state so other scripts can check it.
/// </summary>
public class PropState : MonoBehaviour
{
    // If true, the prop is active and can interact with score/danger zones.
    // If false, the prop is inactive and should not affect gameplay zones.
    public bool propActive;
}