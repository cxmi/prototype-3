using UnityEngine;

public class WallLogic : MonoBehaviour
{
    
    public ScalingWalls scalingWalls;
    public GameObject wall;

    public GameObject newWall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wall = GameObject.FindGameObjectWithTag("AllWalls");
        scalingWalls = FindFirstObjectByType<ScalingWalls>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scalingWalls.wallsScaled == true)
        {
            Destroy(wall);
            Instantiate(newWall, transform.position, Quaternion.identity);
        }
    }
}
