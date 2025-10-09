using System.Collections;
using UnityEngine;

public class PickupableObject : MonoBehaviour
{
    [HideInInspector] public bool isHeld = false;

    private FateEnemyMovement movementScript;
    private Transform holder;
    private LineRenderer lineRenderer;
    private Color startingLineColor;
    public float maxHoldTime;

    void Start()
    {
        movementScript = GetComponent<FateEnemyMovement>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        startingLineColor = lineRenderer.startColor;
        maxHoldTime = 1.5f;
    }

    void Update()
    {
        if (isHeld && holder != null)
        {
            transform.position = holder.position;
        }
    }

    public void PickUp(Transform holderTransform)
    {
        isHeld = true;
        transform.SetParent(holderTransform);

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        if (movementScript != null)
            movementScript.enabled = false;
        
        StartCoroutine(DropAfterDelay(maxHoldTime));

    }


    public void Drop()
    {
        isHeld = false;
        transform.SetParent(null);

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        if (movementScript != null)
            movementScript.enabled = true;
        
        
        lineRenderer.startColor = startingLineColor;
        lineRenderer.endColor = startingLineColor;
    }

    IEnumerator DropAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Drop(); // Call your Drop function here
    }
}