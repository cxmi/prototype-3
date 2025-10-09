using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class LineConnector : MonoBehaviour
{
    public Transform pointA; // First GameObject
    public Transform pointB; // Second GameObject

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    public PlayerPickup playerPickup;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerPickup = player.GetComponent<PlayerPickup>();
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        pointB = this.gameObject.transform;
        pointA = GameObject.Find("Center").transform;
    }

    void Update()
    {
        if (pointA == null || pointB == null)
            return;

        // set positions for LineRenderer (world space)
        Vector3[] positions = new Vector3[2] { pointA.position, pointB.position };
        lineRenderer.SetPositions(positions);

        // convert world space to local space
        Vector2[] colliderPoints = new Vector2[2]
        {
            transform.InverseTransformPoint(pointA.position),
            transform.InverseTransformPoint(pointB.position)
        };
        edgeCollider.points = colliderPoints;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");
            
            // restart
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        
        if (other.CompareTag("Enemy"))
        {

            if (playerPickup.heldObject.CompareTag("Enemy"))
            {
                
                Debug.Log("enemy entered the trigger while holding");

                if (other.gameObject != playerPickup.heldObject.gameObject)
                {
                    Destroy(other.gameObject);

                }
                
      
            }
            
            Debug.Log("enemy entered the trigger normally");

            // do sth
            //if player is holding an emey, this is the only way enemy can be hurt
        }
    }
}