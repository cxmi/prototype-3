using UnityEngine;

public class SphereMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // Get input axes (WASD or arrow keys)
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down

        // Create movement vector
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        // Normalize and apply speed
        transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);
    }
}