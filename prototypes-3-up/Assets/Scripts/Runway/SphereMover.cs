using UnityEngine;

public class SphereMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public KeyCode wKey = KeyCode.W;
    public KeyCode aKey = KeyCode.A;
    public KeyCode dKey = KeyCode.D;
    public KeyCode sKey = KeyCode.S;
    
    public float maxRotationSpeed = 180f;  // Max degrees per second
    public float acceleration = 100f;      // Speed increase per second
    public float deceleration = 150f;      // Speed decrease per second

    private float currentSpeed = 0f;
    
    public AudioSource mainSong;

    public float maxPitch = 1f;
    public float pitchAcceleration = 50f;
    public float pitchDeceleration = 50f;
    private float currentPitch = 0.1f;
    public float minPitch = 0.1f;
    
    //private SpriteRenderer spriteRenderer;



    void Update()
    {
        // Get input axes (WASD or arrow keys)
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down

        // Create movement vector
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        // Normalize and apply speed
        transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);
        
        
        
        if (Input.GetKey(wKey) || Input.GetKey(sKey) || Input.GetKey(aKey) || Input.GetKey(dKey))
        {
            // Increase rotation speed
            currentSpeed += acceleration * Time.deltaTime;
            
            currentPitch += pitchAcceleration * Time.deltaTime;

        }
        
        
        else
        {
            // Decrease rotation speed
            currentSpeed -= deceleration * Time.deltaTime;
            currentPitch -= pitchDeceleration * Time.deltaTime;
        }

        // Clamp speed between 0 and max
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxRotationSpeed);
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        

        // Apply rotation on Y-axis
        transform.Rotate(0f, currentSpeed * Time.deltaTime, 0f );
        
        mainSong.pitch = currentPitch;
    }
}


