using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI propStateText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI inputModeText;

    public GameObject trackedProp;
    public InputManager inputManager;

    private PropState propState;

    void Start()
    {
        if (trackedProp != null)
        {
            propState = trackedProp.GetComponent<PropState>();
        }
    }

    void Update()
    {
        if (GameManager.instance == null) return;

        scoreText.text = "Score: " + GameManager.instance.score;

        propStateText.text = propState != null && propState.propActive
            ? "Prop: ACTIVE"
            : "Prop: Inactive";

        timerText.text = "Time: " + Mathf.CeilToInt(GameManager.instance.roundTime);

        if (inputModeText != null && inputManager != null)
        {
            inputModeText.text = "Input Mode: " + inputManager.GetInputModeName();
        }
    }
}