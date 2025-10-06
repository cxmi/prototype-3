using UnityEngine;

public class CameraAttempt : MonoBehaviour
{
    public Transform player1;      // Player to follow
    public Transform corn;         // The object the camera should look at
    public Vector3 localOffset = new Vector3(0, 2, -5);  // Local space offset from Player1
    public float followSmoothness = 5f;

    void LateUpdate()
    {
        if (player1 == null || corn == null) return;

        // Calculate desired position in world space, based on Player1's local offset
        Vector3 desiredPosition = player1.TransformPoint(localOffset);

        // Smooth camera movement
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmoothness * Time.deltaTime);

        // Always look at Corn
        transform.LookAt(corn.position);
    }
}
