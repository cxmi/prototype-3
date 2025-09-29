using TMPro;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject[] Blocks;
    public int counter = 0;
    public int numberOfSame = 8;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverScreen.SetActive(false);
        NewBlock();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = TetrisMovement.score.ToString();
    }

    public void NewBlock()
    {
        
        int blockType = Random.Range(1, 5);
        
        if (counter < numberOfSame)
        {
            Instantiate(Blocks[0], transform.position, Quaternion.identity);
            counter++;
        }
        else if (counter >= numberOfSame)
        {
            Instantiate(Blocks[blockType], transform.position, Quaternion.identity);
            counter = 0;
        }
        //Instantiate(Blocks[4], transform.position, Quaternion.identity);
        //Instantiate(Blocks[Random.Range(0, Blocks.Length)], transform.position, Quaternion.identity);
    }
}
