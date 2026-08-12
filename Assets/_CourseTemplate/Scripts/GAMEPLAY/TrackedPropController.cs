using UnityEngine;

public class TrackedPropController : MonoBehaviour
{
    public Material keyboardInactiveMaterial;
    public Material keyboardActiveMaterial;
    public Material mocapInactiveMaterial;
    public Material mocapActiveMaterial;

    private MeshRenderer meshRenderer;
    private PropState propState;

    private bool wasActiveLastFrame;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        propState = GetComponent<PropState>();

        if (propState != null)
        {
            wasActiveLastFrame = propState.propActive;
        }
    }

    public void ApplyInput(Vector3 targetPosition, bool isActive, InputManager.InputMode inputMode)
    {
        transform.position = targetPosition;

        if (propState != null)
        {
            propState.propActive = isActive;
        }

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

        wasActiveLastFrame = isActive;

        if (meshRenderer != null)
        {
            if (inputMode == InputManager.InputMode.DebugKeyboard)
            {
                meshRenderer.material = isActive ? keyboardActiveMaterial : keyboardInactiveMaterial;
            }
            else
            {
                meshRenderer.material = isActive ? mocapActiveMaterial : mocapInactiveMaterial;
            }
        }
    }

    public bool IsActive()
    {
        return propState != null && propState.propActive;
    }
}