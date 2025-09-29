using UnityEngine;

public class UpNextScript : MonoBehaviour
{
    public GameObject[] NextBlock;
    public Color blockColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blockColor = new Color32(0, 34, 77, 255);
        Instantiate(NextBlock[Random.Range(0, NextBlock.Length)], transform.position, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeShape()
    {
        
        Instantiate(NextBlock[Random.Range(0, NextBlock.Length)], transform.position, Quaternion.identity);

    }
}
