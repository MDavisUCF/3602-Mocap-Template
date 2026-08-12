using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public int penaltyValue = 1;

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
            GameManager.instance.AddScore(-penaltyValue);
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayDangerSound();
        }
    }
}