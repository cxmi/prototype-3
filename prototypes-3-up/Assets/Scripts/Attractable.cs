using UnityEngine;

public class Attractable : MonoBehaviour
{
    private Rigidbody2D rb;

    public float alignmentSpeed = 5f; // How quickly it rotates to align with planet "surface"

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Attract(Vector2 attractorPosition, float gravityStrength)
    {
        Vector2 directionToCenter = (attractorPosition - (Vector2)transform.position);
        Vector2 forceDirection = directionToCenter.normalized;

        // Apply force toward the center
        rb.AddForce(forceDirection * gravityStrength);

        // Optional: Rotate so object's "up" is away from planet center
        float angle = Mathf.Atan2(-forceDirection.y, -forceDirection.x) * Mathf.Rad2Deg + 90f; // Object's 'up' is Y+
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, alignmentSpeed * Time.fixedDeltaTime);
    }
}
