using System.Collections.Generic;
using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerHolding : MonoBehaviour
    {
        [Header("Picking Up")]
        public Pickupable objectInHand;
        [SerializeField] private List<Pickupable> pickupsInRange;
        public bool isHolding;
        
        [Header("Dragging")]
        public Draggable objectDragging;
        [SerializeField] private List<Draggable> dragsInRange;
        public bool isDragging;

        [Header("References")] 
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform dragPlacementTransform;
        [SerializeField] private Transform pickupPlacementTransform;

        [Header("Input")] 
        [SerializeField] private InputActionReference interact;

        private void Update()
        {
            if (interact.action.triggered)
            {
                if (NeedToDrop()) return;

                //handle pickup or drag
                HandlePickup();
                HandleDrag();
            }
        }
        
        #region Exports

        public float GetHeldWeight()
        {
            float heldWeight = 0;

            if (objectInHand != null) heldWeight += objectInHand.weight;
            if (objectDragging != null) heldWeight += objectDragging.weight;
            
            return heldWeight;
        }
        
        #endregion

        #region Trigger Enter/Exit
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Pickupable"))
            {
                //initialize pickup and check that it has appropriate script
                Pickupable pickup = collision.gameObject.GetComponent<Pickupable>();
                if (pickup == null)
                {
                    Debug.LogWarning("PICKUP DOES NOT HAVE A PICKUPABLE SCRIPT");
                    return;
                }
            
                pickupsInRange.Add(pickup);
            }
            else if (collision.gameObject.CompareTag("Draggable"))
            {
                Draggable draggable = collision.gameObject.GetComponent<Draggable>();
                if (draggable == null)
                {
                    Debug.LogWarning("DRAGGABLE DOES NOT HAVE A DRAGGABLE SCRIPT");
                    return;
                }
                
                dragsInRange.Add(draggable);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Pickupable"))
            {
                //initialize pickup and check that it has appropriate script
                Pickupable pickup = collision.gameObject.GetComponent<Pickupable>();
                if (pickup == null)
                {
                    Debug.LogWarning("PICKUP DOES NOT HAVE A PICKUPABLE SCRIPT");
                    return;
                }
            
                pickupsInRange.Remove(pickup);
            }
            else if (collision.gameObject.CompareTag("Draggable"))
            {
                Draggable draggable = collision.gameObject.GetComponent<Draggable>();
                if (draggable == null)
                {
                    Debug.LogWarning("DRAGGABLE DOES NOT HAVE A DRAGGABLE SCRIPT");
                    return;
                }

                dragsInRange.Remove(draggable);
            }
        }
        
        #endregion
        
        #region Picking Up
        
        private void HandlePickup()
        {
            //check if any objects in range
            if (pickupsInRange.Count == 0) return;
            
            //put object in hand, remove from space
            objectInHand = GetClosestPickupInRange();
            if (objectInHand == null) return;
            pickupsInRange.Remove(objectInHand);
        
            PickUp(objectInHand);
        }

        private void PickUp(Pickupable pickup)
        {
            pickup.PutInHand(pickupPlacementTransform);
            isHolding = true;
        }

        private void Drop()
        {
            if (objectInHand == null)
            {
                Debug.LogWarning("NO OBJECT IN HAND TO DROP");
                return;
            }

            isHolding = false;
            objectInHand.DropFromHand();
            objectInHand = null;
        }

        private Pickupable GetClosestPickupInRange()
        {
            if (pickupsInRange.Count == 0) return null;
            //just return first if one
            if (pickupsInRange.Count == 1) return pickupsInRange[0];

            //make first closest to start
            Pickupable closest = pickupsInRange[0];
            float closestDistance = Vector3.Distance(playerTransform.position, closest.transform.position);
        
            foreach (Pickupable pickup in pickupsInRange)
            {
                float currentDistance = Vector3.Distance(playerTransform.position, pickup.transform.position);
                if (currentDistance < closestDistance)
                {
                    closest = pickup;
                    closestDistance = currentDistance;
                }
            }
        
            return closest;
        }
        
        #endregion

        #region Dragging
        
        private void HandleDrag()
        {
            if (objectInHand != null) return;
            
            //get closest draggable
            objectDragging = GetClosestDraggableInRange();
            if (objectDragging == null) return;
            dragsInRange.Remove(objectDragging);

            GrabOnTo();
        }
        
        private void GrabOnTo()
        {
            objectDragging.GrabOnTo(dragPlacementTransform);
            dragPlacementTransform.position = objectDragging.transform.position;
            isDragging = true;
        }

        private void Release()
        {
            if (objectDragging == null)
            {
                Debug.LogWarning("NO OBJECT IN HAND TO RELEASE");
                return;
            }

            dragsInRange.Add(objectDragging);
            objectDragging.Release();
            objectDragging = null;
            isDragging = false;
        }

        private Draggable GetClosestDraggableInRange()
        {
            if (dragsInRange.Count == 0) return null;
            if (dragsInRange.Count == 1) return dragsInRange[0];
            
            Draggable closest = dragsInRange[0];
            float closestDistance = Vector3.Distance(playerTransform.position, closest.transform.position);

            foreach (Draggable drag in dragsInRange)
            {
                float currentDistance = Vector3.Distance(playerTransform.position, drag.transform.position);
                if (currentDistance < closestDistance)
                {
                    closest = drag;
                    closestDistance = currentDistance;
                }
            }

            return closest;
        }

        #endregion

        #region Dropping
        
        private bool NeedToDrop()
        {
            bool usingHands = (objectInHand != null) || (objectDragging != null);

            if (objectInHand != null) Drop();
            else if (objectDragging != null) Release();
            
            return usingHands;
        }
        
        #endregion
    }
}
