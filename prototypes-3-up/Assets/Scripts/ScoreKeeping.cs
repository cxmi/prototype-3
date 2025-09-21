using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreKeeping : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public InputActionAsset inputActions;
    public InputAction anyButtonAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        anyButtonAction = inputActions.FindAction("AnyButton");
        if (anyButtonAction != null)
        {
            anyButtonAction.Enable();
        }
    }

    void OnDisable()
    {
        anyButtonAction.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        scoreText.text = ContactLogic.score.ToString();
        if (anyButtonAction != null && anyButtonAction.triggered &&
            !Keyboard.current.wKey.wasPressedThisFrame &&
            !Keyboard.current.aKey.wasPressedThisFrame &&
            !Keyboard.current.sKey.wasPressedThisFrame &&
            !Keyboard.current.dKey.wasPressedThisFrame)
        {
            Restart();
        }
    }
    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
