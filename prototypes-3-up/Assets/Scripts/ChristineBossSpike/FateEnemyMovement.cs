using UnityEngine;

public class FateEnemyMovement : MonoBehaviour
{
    public float moveSpeed = 1.5f; // Max speed
    public float directionChangeInterval = 2f; // Time between direction changes
    public float maxDirectionChangeAngle = 45f; // Limits sharpness of turns

    private Vector2 currentDirection;
    private float timeSinceLastChange;

    void Start()
    {
        PickNewDirection();
    }

    void Update()
    {
        timeSinceLastChange += Time.deltaTime;

        // Random direction change
        if (timeSinceLastChange >= directionChangeInterval)
        {
            PickNewDirection();
            timeSinceLastChange = 0f;
        }

        // Move the object
        transform.Translate(currentDirection * moveSpeed * Time.deltaTime);

        // Check if it's hitting the screen edge and reflect direction
        CheckScreenBounce();
    }
    void CheckScreenBounce()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        bool bounced = false;

        // Horizontal bounce
        if (viewportPos.x < 0f || viewportPos.x > 1f)
        {
            currentDirection.x = -currentDirection.x;
            bounced = true;
        }

        // Vertical bounce
        if (viewportPos.y < 0f || viewportPos.y > 1f)
        {
            currentDirection.y = -currentDirection.y;
            bounced = true;
        }

        if (bounced)
        {
            // Normalize to maintain consistent speed
            currentDirection = currentDirection.normalized;

            // Optional: prevent object from getting stuck out of bounds
            Vector3 clamped = viewportPos;
            clamped.x = Mathf.Clamp01(clamped.x);
            clamped.y = Mathf.Clamp01(clamped.y);
            transform.position = Camera.main.ViewportToWorldPoint(clamped);
        }
    }
    void PickNewDirection()
    {
        // If we already have a direction, pick a new one within a cone (not random 360°)
        if (currentDirection != Vector2.zero)
        {
            float angle = Random.Range(-maxDirectionChangeAngle, maxDirectionChangeAngle);
            currentDirection = Quaternion.Euler(0, 0, angle) * currentDirection;
        }
        else
        {
            // First time: pick a completely random normalized direction
            float randomAngle = Random.Range(0f, 360f);
            currentDirection =
                new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
        }
    }
}
