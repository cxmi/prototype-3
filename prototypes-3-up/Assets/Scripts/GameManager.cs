using CameraScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool IsPaused;
    
    [Header("Game Settings")]
    [SerializeField] private int targetFrameRate = 60;
    
    [Header("Input")]
    public InputActionReference reset;
    
    //init references
    private Camera _camera;
    private CameraShake _cameraShake;
    private Animator _cameraAnimator;

    
    #region Singleton
    
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    #endregion

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    private void Update()
    {
        if (reset.action.triggered)
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
