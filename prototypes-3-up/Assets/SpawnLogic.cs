using UnityEngine;

public class SpawnLogic : MonoBehaviour
{
    //public Transform spawnPoint;
    private Vector2 randomPosition;
    public GameObject spawnObject;
    
    //left quadrant - x: -40 to -36, y: -26 to 26
    public int minXLeft = -40; 
    public int maxXLeft  = -36;
    
    public int minYAll = -26;
    public int maxYAll  = 26;
    public int minXAll = -38;
    public int maxXAll = 38;
    
    //right quadrant - x: 36 to 40, same y
    
    public int minXRight = 36;
    public int maxXRight  = 40;
    
    //top quadrant - x: -38 to 38, y: 22 to 27
    
    public int minYTop = 22;
    public int maxYTop = 27;
    
    //bottom quadrant - x: -38 to 38, y: -21 to -26
    
    public int minYBottom = -26;
    public int maxYBottom = -21;
    
    
    public int minY = -12;
    public int maxY = 12;

    public float spawnRate = 0.5f;
    public float stopAfter = 20f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      InvokeRepeating(nameof(Spawn), 0, spawnRate);
      Invoke(nameof(StopSpawning), stopAfter);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        
        //choose quadrant first
        
        int randQuadrant = Random.Range(0, 4);
        switch (randQuadrant)
        {
            case 0: //LEFT QUADRANT
                randomPosition = new Vector2(
                    Random.Range(minXLeft, maxXLeft),
                    Random.Range(minYAll, maxYAll)
                );
                break;
            case 1: //RIGHT QUADRANT
                randomPosition = new Vector2(
                    Random.Range(minXRight, maxXRight),
                    Random.Range(minYAll, maxYAll)
                );
                break;
            case 2: //TOP QUADRANT
                randomPosition = new Vector2(
                    Random.Range(minXAll, maxXAll),
                    Random.Range(minYTop, maxYTop)
                );
                break;
            case 3: //BOTTOM QUADRANT
                randomPosition = new Vector2(
                    Random.Range(minXAll, maxXAll),
                    Random.Range(minYBottom, maxYBottom)
                );
                break;
            default:
                break;
            
        }
        {
            
        }
        
        // randomPosition = new Vector2(
        //     Random.Range(minX, maxX),
        //     Random.Range(minY, maxY)
        // );
        
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
        
        //pick which of 4 quadrants to spawn in
        //spawn there
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(Spawn));
        Debug.Log("Spawning stopped at " + Time.time);
    }
}
