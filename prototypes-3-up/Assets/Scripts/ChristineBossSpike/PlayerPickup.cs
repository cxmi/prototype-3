using System.Collections;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 1.5f;
    public KeyCode pickupKey = KeyCode.P;

    public PickupableObject heldObject = null;

    public Color originalLineColor;

    void Start()
    {
        originalLineColor = Color.red;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            if (heldObject == null)
            {
                TryPickupObject();
                //StartCoroutine(DropAfterDelay(3f));

            }
            else
            {
                heldObject.Drop();
                heldObject = null;
            }
        }
    }

    void TryPickupObject()
    {
        // Find all pickupable objects nearby
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange);
        foreach (var hit in hits)
        {
            PickupableObject pickup = hit.GetComponent<PickupableObject>();
            if (pickup != null && !pickup.isHeld)
            {
                heldObject = pickup;
                pickup.PickUp(transform);

                if (heldObject.CompareTag("Enemy"))
                {
                    Color color;
                    if (ColorUtility.TryParseHtmlString("#48F7F7", out color))
                    {
                        heldObject.GetComponentInChildren<LineRenderer>().startColor = color;
                        heldObject.GetComponentInChildren<LineRenderer>().endColor = color;
                        
                    }
                }
                    
                break;
            }
        }
    }

    // visualization of pickup in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
    

}