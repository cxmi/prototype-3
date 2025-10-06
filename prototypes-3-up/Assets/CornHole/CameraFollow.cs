using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;         // The object the rig follows (e.g. player)
    public Transform lookAtTarget;         // The object the camera always looks at
    public Vector3 baseFollowOffset = new Vector3(0f, 5f, -10f); // Base offset
    public float followSpeed = 5f;         // Speed for rig following
    public float offsetAdjustSpeed = 3f;  // Speed for offset adjustment

    public float viewportEdgeThreshold = 0.25f; // When player gets closer than this to screen edge, shift camera
    public float maxXOffset = 3f;          // Max horizontal offset applied to camera offset.x

    public Transform cameraTransform;      // The actual Camera GameObject (child of rig)

    private float currentXOffset = 0f;     // Current dynamic offset

    void LateUpdate()
    {
        if (followTarget == null || cameraTransform == null || lookAtTarget == null)
            return;

        // Step 1: Follow the player smoothly
        Vector3 desiredRigPosition = followTarget.position;
        transform.position = Vector3.Lerp(transform.position, desiredRigPosition, followSpeed * Time.deltaTime);

        // Step 2: Get player's viewport position relative to the camera
        Vector3 viewportPos = cameraTransform.GetComponent<Camera>().WorldToViewportPoint(followTarget.position);

        // Step 3: Calculate desired X offset based on viewport position
        float targetXOffset = 0f;

        if (viewportPos.x < viewportEdgeThreshold)
        {
            // Player is near the left edge → shift camera offset right (positive X offset)
            float t = Mathf.InverseLerp(0f, viewportEdgeThreshold, viewportPos.x);
            targetXOffset = Mathf.Lerp(maxXOffset, 0f, t);
        }
        else if (viewportPos.x > 1f - viewportEdgeThreshold)
        {
            // Player is near the right edge → shift camera offset left (negative X offset)
            float t = Mathf.InverseLerp(1f, 1f - viewportEdgeThreshold, viewportPos.x);
            targetXOffset = Mathf.Lerp(-maxXOffset, 0f, t);
        }

        // Step 4: Smoothly interpolate current X offset toward target
        currentXOffset = Mathf.Lerp(currentXOffset, targetXOffset, offsetAdjustSpeed * Time.deltaTime);

        // Step 5: Apply dynamic offset to camera position
        Vector3 dynamicOffset = baseFollowOffset + new Vector3(currentXOffset, 0f, 0f);
        cameraTransform.position = transform.position + dynamicOffset;

        // Step 6: Look at the lookAtTarget
        cameraTransform.LookAt(lookAtTarget);
    }
}
