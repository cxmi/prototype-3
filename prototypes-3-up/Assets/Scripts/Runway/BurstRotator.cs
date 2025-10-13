using UnityEngine;

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

    public KeyCode keyBind = KeyCode.W;
    void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
        currentCutoff = minCutoff;             // Start from the min value
        spriteMask.alphaCutoff = currentCutoff;
    }
    void Update()
    {
        if (Input.GetKey(keyBind))
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
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxRotationSpeed);

        // Apply rotation on Y-axis
        transform.Rotate(0f, 0f, currentSpeed * Time.deltaTime);
    }
}