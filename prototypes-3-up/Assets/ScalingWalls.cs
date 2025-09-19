using UnityEngine;

public class ScalingWalls : MonoBehaviour
{
    public float scaleAmount = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < 1.7)
        {
            transform.localScale += new Vector3(scaleAmount, scaleAmount, 0f);
        }
        else
        {
            transform.localScale = new Vector3(1,1,0f);
        }
    }
}
