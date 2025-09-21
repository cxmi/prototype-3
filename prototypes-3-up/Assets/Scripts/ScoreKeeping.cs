using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreKeeping : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    
    public InputAction anyButtonAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        anyButtonAction.Enable();
    }

    void OnDisable()
    {
        anyButtonAction.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        scoreText.text = ContactLogic.score.ToString();
        if (anyButtonAction.triggered)
        {
            Restart();
        }
    }
    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
