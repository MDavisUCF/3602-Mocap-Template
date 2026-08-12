using UnityEngine;

/// <summary>
/// A scoring zone that adds points when the active tracked prop enters it.
/// 
/// This script only responds if the object entering the trigger belongs to the tracked prop
/// and the prop is currently active.
/// </summary>
public class ScoreZone : MonoBehaviour
{
    [Header("Score Zone Settings")]

    // How many points should be added when the active prop enters this zone.
    public int scoreValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Try to find a TrackedPropController on the object that entered the zone,
        // or on one of its parent objects.
        //
        // This is useful because the collider might be on a child object,
        // while the main TrackedPropController script is on the parent.
        TrackedPropController prop = other.GetComponentInParent<TrackedPropController>();

        // If the object that entered is not the tracked prop, ignore it.
        if (prop == null)
        {
            return;
        }

        // If the prop is not active, ignore it.
        // Inactive props should not affect score or trigger sounds.
        if (!prop.IsActive())
        {
            return;
        }

        // Add points to the score.
        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(scoreValue);
        }

        // Play the score sound after confirming the prop is active.
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayScoreSound();
        }
    }
}