using UnityEngine;

namespace Interactables
{
    public class Draggable : MonoBehaviour
    {
        public float weight;
        [SerializeField] private float friction;

        [Header("Dragging Settings")] 
        [SerializeField] private float startingSpeed = 13f;
        [SerializeField] private float distanceThreshold = 0.2f;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;

        private Transform _target;
        private bool beingDragged;
    
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            
            //make body static
            _rigidbody.bodyType = RigidbodyType2D.Static;
        }

        private void Update() 
        {
            if (beingDragged && Vector3.Distance(transform.position, _target.position) > distanceThreshold)
            {
                Vector3 direction = _target.position - transform.position;
                _rigidbody.AddRelativeForce(direction.normalized * (startingSpeed - weight), ForceMode2D.Force);
            }    
        }

        public void GrabOnTo(Transform target)
        {
            _target = target;
            beingDragged = true;
            
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        public void Release()
        {
            beingDragged = false;
            
            _rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }
}
