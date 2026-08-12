using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public int scoreValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        TrackedPropController prop = other.GetComponentInParent<TrackedPropController>();

        if (prop == null)
        {
            return;
        }

        if (!prop.IsActive())
        {
            return;
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(scoreValue);
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayScoreSound();
        }
    }
}