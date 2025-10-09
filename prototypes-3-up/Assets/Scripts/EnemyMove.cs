using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float speed;


    private void Update()
    {
        if (GameManager.IsPaused) return;
        
        //move enemy
        transform.position += speed * Time.deltaTime * transform.up;
        
        //destroy if out of bounds
        if (Mathf.Abs(transform.position.y) > 30 || Mathf.Abs(transform.position.x) > 40) Destroy(gameObject); 
    }
}
