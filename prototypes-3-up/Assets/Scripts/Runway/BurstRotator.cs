using UnityEngine;
using UnityEngine.InputSystem;

public class BurstRotator : MonoBehaviour
{
    public float maxRotationSpeed = 180f;  // Max degrees per second
    public float acceleration = 100f;      // Speed increase per second
    public float deceleration = 150f;      // Speed decrease per second

    private float currentSpeed = 0f;
    
    public float pingPongSpeed = .5f; // How fast the value oscillates
    
    public float cutoffSpeed = 0.5f; 
    public float minCutoff = 0.3f;
    public float maxCutoff = 0.8f;
    private float currentCutoff;
    private bool goingUp = true;

    private SpriteMask spriteMask;
    
    //InputSystem
    public InputActionReference inputAction;
    public KeyCode keyBind = KeyCode.W;
    

    
    void Start()
    {
        inputAction.action.Enable();
   
        spriteMask = GetComponent<SpriteMask>();
        currentCutoff = minCutoff;             // Start from the min value
        spriteMask.alphaCutoff = currentCutoff;
        
    }
    void Update()
    {
        Vector2 input = inputAction.action.ReadValue<Vector2>();

        float rotatingSpeed = 0;
        
        switch (keyBind)
        {
            case KeyCode.W:
                if (input.y > 0f)
                {
                    rotatingSpeed = Mathf.Abs(input.y);
                }
                break;
            case KeyCode.S:
                if (input.y < 0f)
                {
                    rotatingSpeed = Mathf.Abs(input.y);
                }
                break;
            case KeyCode.D:
                if (input.x > 0f)
                {
                    rotatingSpeed = Mathf.Abs(input.x);
                }
                break;
            case KeyCode.A:
                if (input.y < 0f)
                {
                    rotatingSpeed = Mathf.Abs(input.x);
                }
                break;
            default:
                break;
        }
    
        
        if (rotatingSpeed > 0 || Input.GetKey(keyBind))
        {
            // Increase rotation speed
            currentSpeed += acceleration * Time.deltaTime;
            
            float delta = cutoffSpeed * Time.deltaTime;

            if (goingUp)
            {
                currentCutoff += delta;
                if (currentCutoff >= maxCutoff)
                {
                    currentCutoff = maxCutoff;
                    goingUp = false;
                }
            }
            else
            {
                currentCutoff -= delta;
                if (currentCutoff <= minCutoff)
                {
                    currentCutoff = minCutoff;
                    goingUp = true;
                }
            }

            spriteMask.alphaCutoff = currentCutoff;
        }
        else
        {
            // Decrease rotation speed
            currentSpeed -= deceleration * Time.deltaTime;
        }

        // Clamp speed between 0 and max
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxRotationSpeed * rotatingSpeed);

        // Apply rotation on Y-axis
        transform.Rotate(0f, 0f, currentSpeed * Time.deltaTime);
    }
}