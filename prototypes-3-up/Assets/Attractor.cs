using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float gravityStrength = 9.8f;
    public float attractionRadius = 5f;
    public LayerMask attractableLayer;

    void FixedUpdate()
    {
        // Get all colliders in radius on the "Attractable" layer
        Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(transform.position, attractionRadius, attractableLayer);

        foreach (Collider2D col in affectedObjects)
        {
            Attractable attractable = col.GetComponent<Attractable>();
            if (attractable != null)
            {
                attractable.Attract(transform.position, gravityStrength);
            }
        }
    }
    
    
    void OnDrawGizmosSelected()
    {
        // Draw gravity radius in editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
