using UnityEngine;

public class CameraOrbitAroundPlayer : MonoBehaviour
{
    public Transform player;             // The player to stay near
    public Transform lookAtTarget;       // The target camera looks at (can be player or something else)

    public float radius = 8f;            // Distance from the player
    public float height = 5f;            // Height above the player
    public float adjustSpeed = 60f;      // How fast the camera rotates around player
    public float edgeThreshold = 0.3f;   // Distance from screen edge before adjustment
    public float smoothMoveSpeed = 5f;   // Smoothing for position
    public float baseZOffset = -3f;      // Offset behind the player’s forward direction

    private float currentAngle = 0f;     // Angle around the player
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Attach this script to a Camera object.");
        }

        // Start behind the player
        currentAngle = 180f;
    }

    void LateUpdate()
    {
        if (player == null || lookAtTarget == null || cam == null)
            return;

        // Step 1: Calculate base offset behind player
        Vector3 behindOffset = player.forward * baseZOffset;

        // Step 2: Calculate orbit offset based on current angle
        Vector3 orbitOffset = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0, Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * radius;

        // Step 3: Combine offsets and apply height
        Vector3 desiredPosition = player.position + orbitOffset + behindOffset + Vector3.up * height;

        // Step 4: Simulate camera at desired position to check player's screen position
        Vector3 simulatedViewportPos = cam.WorldToViewportPoint(player.position + Vector3.up * 1.0f);

        // Step 5: Adjust angle if player is near left or right screen edge
        if (simulatedViewportPos.x < edgeThreshold)
        {
            currentAngle += adjustSpeed * Time.deltaTime;
        }
        else if (simulatedViewportPos.x > 1f - edgeThreshold)
        {
            currentAngle -= adjustSpeed * Time.deltaTime;
        }

        // Normalize angle
        currentAngle %= 360f;

        // Step 6: Recalculate final position after angle adjustment
        orbitOffset = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0, Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * radius;
        desiredPosition = player.position + orbitOffset + behindOffset + Vector3.up * height;

        // Step 7: Smooth movement
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothMoveSpeed * Time.deltaTime);

        // Step 8: Always look at the lookAtTarget
        transform.LookAt(lookAtTarget);
    }
}