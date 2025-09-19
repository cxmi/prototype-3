using UnityEngine;

public class SpawnLogic : MonoBehaviour
{
    //public Transform spawnPoint;
    private Vector2 randomPosition;
    public GameObject spawnObject;
  
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        randomPosition = new Vector2(
            Random.Range(-18, 18),
            Random.Range(-12, 12)
        );
        
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
        
        //pick which of 4 quadrants to spawn in
        //spawn there
    }
}
