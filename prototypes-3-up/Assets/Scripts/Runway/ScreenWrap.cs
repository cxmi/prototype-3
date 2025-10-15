using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private Camera cam;

    [Header("Bounce Settings")]
    public float bounceStrength = 2f;  // How strongly to push the object back
    public float margin = 0.01f;       // Small buffer to detect "hitting" the edge

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // Convert world position to screen-space viewport coordinates (0 to 1)
        Vector3 viewportPos = cam.WorldToViewportPoint(pos);

        bool bounced = false;
        Vector3 bounceDir = Vector3.zero;

        // Horizontal (X) edges
        if (viewportPos.x < margin)
        {
            bounceDir.x = 1f;
            bounced = true;
        }
        else if (viewportPos.x > 1f - margin)
        {
            bounceDir.x = -1f;
            bounced = true;
        }

        // Vertical (Z) edges via Y in Viewport space (camera sees X-Y)
        if (viewportPos.y < margin)
        {
            bounceDir.z = 1f;
            bounced = true;
        }
        else if (viewportPos.y > 1f - margin)
        {
            bounceDir.z = -1f;
            bounced = true;
        }

        if (bounced)
        {
            // Apply a small push back in the opposite direction
            transform.position += bounceDir.normalized * bounceStrength * Time.deltaTime;

            // Optional: clamp back inside screen to prevent stuck edges
            viewportPos.x = Mathf.Clamp(viewportPos.x, margin, 1f - margin);
            viewportPos.y = Mathf.Clamp(viewportPos.y, margin, 1f - margin);

            // Re-convert to world position, keeping Y unchanged
            Vector3 clampedWorldPos = cam.ViewportToWorldPoint(viewportPos);
            transform.position = new Vector3(clampedWorldPos.x, transform.position.y, clampedWorldPos.z);
        }
    }
}