using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Type")] 
    [SerializeField] private GameObject enemyPrefab;
    
    [Header("Spawner Settings")]
    [SerializeField] [Range(0.05f, 1f)] private float frequency;
    [SerializeField] [Range(0f, 3f)] private float rotation;

    private float _timer;

    private void Start()
    {
        _timer = frequency;
    }
    
    private void Update()
    {
        if (GameManager.IsPaused) return;
        
        //rotate if rotating
        transform.Rotate(Vector3.forward, rotation);
        
        //increment timer for spawn frequency
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            //spawn if timer up
            _timer = frequency;
            Instantiate(enemyPrefab, transform.position, transform.rotation);
        }
    }
}
