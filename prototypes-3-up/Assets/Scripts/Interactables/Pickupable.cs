using UnityEngine;

namespace Interactables
{
    public class Pickupable : MonoBehaviour
    {
        public float weight;
        [SerializeField] private bool throwable;
        [SerializeField] private float friction;
    
        [Header("Locked Position")]
        [SerializeField] private Transform lockedTransform;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (lockedTransform == null) return;
        
            transform.position = lockedTransform.position;
            transform.rotation = lockedTransform.rotation;
        }
    
        public void PutInHand(Transform t)
        {
            lockedTransform = t;
            _collider.enabled = false;
        }

        public void DropFromHand()
        {
            lockedTransform = null;
            _collider.enabled = true;
        }

        public void ThrowFromHand(Vector3 direction)
        {
            //
        }
    }
}
