using UnityEngine;

public class MoveHeadScript : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public float moveForce = 10f;
    public bool limitDiagonalSpeed = true;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get WASD input
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow
        moveInput.y = Input.GetAxisRaw("Vertical"); // W/S or Up/Down Arrow
  
        if (limitDiagonalSpeed)
            moveInput = Vector2.ClampMagnitude(moveInput, 1f); 
}
    
    void FixedUpdate()
    {
        rb.AddForce(moveInput * moveForce);

    }
}
