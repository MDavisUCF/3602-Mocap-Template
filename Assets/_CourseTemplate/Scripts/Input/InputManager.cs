using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const string InputModePrefKey = "LastInputMode";

    public TrackedPropController trackedProp;

    [Header("Targets")]
    public Transform mocapTarget;        // The object your tracked prop follows
    public Transform liveMocapSource;    // The OptiTrack-driven rigid body object

    public enum InputMode
    {
        DebugKeyboard,
        MocapPlaceholder,
        MocapLive
    }

    public InputMode currentMode = InputMode.DebugKeyboard;

    [Header("Debug Keyboard")]
    public float moveSpeed = 5f;

    [Header("Mocap Placeholder")]
    public float placeholderRadius = 3f;
    public float placeholderSpeed = 1f;
    public float placeholderHeight = 1f;

    [Header("Live Mocap")]
    public Vector3 liveMocapPositionOffset = Vector3.zero;
    public Vector3 liveMocapRotationOffset = Vector3.zero;
    public float liveMocapScale = 1f;

    private bool propActive;

    void Start()
    {
        if (trackedProp != null && mocapTarget != null)
        {
            mocapTarget.position = trackedProp.transform.position;
        }

        if (PlayerPrefs.HasKey(InputModePrefKey))
        {
            currentMode = (InputMode)PlayerPrefs.GetInt(InputModePrefKey);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleInputMode();
        }

        if (trackedProp == null || mocapTarget == null)
        {
            return;
        }

        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            propActive = false;
            trackedProp.ApplyInput(mocapTarget.position, false, currentMode);
            return;
        }

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

        trackedProp.ApplyInput(mocapTarget.position, propActive, currentMode);
    }

    void UpdateDebugKeyboardInput()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) move += Vector3.back;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;

        mocapTarget.position += move.normalized * moveSpeed * Time.deltaTime;

        propActive = Input.GetKey(KeyCode.Space);
    }

    void UpdateMocapPlaceholderInput()
    {
        float x = Mathf.Sin(Time.time * placeholderSpeed) * placeholderRadius;
        float z = Mathf.Cos(Time.time * placeholderSpeed) * placeholderRadius;

        mocapTarget.position = new Vector3(x, placeholderHeight, z);

        propActive = Input.GetKey(KeyCode.Space);
    }

    void UpdateLiveMocapInput()
    {
        if (liveMocapSource == null)
        {
            Debug.LogWarning("MocapLive mode is active, but no liveMocapSource is assigned.");

            // Fallback so the object still does something visible.
            UpdateMocapPlaceholderInput();
            return;
        }

        Vector3 mappedPosition = liveMocapSource.position * liveMocapScale + liveMocapPositionOffset;

        mocapTarget.position = mappedPosition;
        mocapTarget.rotation = liveMocapSource.rotation * Quaternion.Euler(liveMocapRotationOffset);

        // For now, keep Space as the "active/use" button.
        // Later, this could come from a button, gesture, distance check, etc.
        propActive = Input.GetKey(KeyCode.Space);
    }

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

        PlayerPrefs.SetInt(InputModePrefKey, (int)currentMode);
        PlayerPrefs.Save();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayInputModeSound();
        }
    }

    public string GetInputModeName()
    {
        return currentMode.ToString();
    }
}