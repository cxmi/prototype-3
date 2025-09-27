using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject[] Blocks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewBlock()
    {
        Instantiate(Blocks[2], transform.position, Quaternion.identity);
        //Instantiate(Blocks[Random.Range(0, Blocks.Length)], transform.position, Quaternion.identity);
    }
}
