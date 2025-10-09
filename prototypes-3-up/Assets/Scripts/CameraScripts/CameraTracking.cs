using UnityEngine;

namespace CameraScripts
{
    public class CameraTracking : MonoBehaviour
    {
        [Header("Tracking")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] [Range(0.01f, 1f)] private float followSpeed;
    
        private void FixedUpdate()
        {
            Vector3 target = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, target, followSpeed);
        }
    }
}
