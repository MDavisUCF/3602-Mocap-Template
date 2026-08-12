using UnityEngine;

public class MocapInputController : MonoBehaviour
{
    [Header("Mode")]
    public bool useMocap = false;

    [Header("Keyboard")]
    public float keyboardSpeed = 5f;

    [Header("Mocap")]
    public Transform mocapSource;
    public Vector3 mocapPositionOffset;
    public Vector3 mocapRotationOffset;
    public float mocapScale = 1f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            useMocap = !useMocap;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetController();
        }

        if (useMocap)
        {
            UpdateFromMocap();
        }
        else
        {
            UpdateFromKeyboard();
        }
    }

    void UpdateFromKeyboard()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        transform.position += movement * keyboardSpeed * Time.deltaTime;
    }

    void UpdateFromMocap()
    {
        if (mocapSource == null)
        {
            return;
        }

        Vector3 mappedPosition = mocapSource.position * mocapScale + mocapPositionOffset;

        transform.position = mappedPosition;
        transform.rotation = mocapSource.rotation * Quaternion.Euler(mocapRotationOffset);
    }

    void ResetController()
    {
        if (useMocap)
        {
            mocapPositionOffset = startPosition - transform.position;
        }
        else
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
    }
}
