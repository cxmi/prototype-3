using TMPro;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject[] Blocks;
    public int counter = 0;
    public int numberOfSame = 8;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    public AudioSource blockSound;
    public AudioClip[] blockSoundClips;
    
    public UpNextScript upNextScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TetrisMovement.score = 0;
        gameOverScreen.SetActive(false);
        upNextScript = FindFirstObjectByType<UpNextScript>();
        NewBlock();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = TetrisMovement.score.ToString();
    }

    public void NewBlock()
    {
        blockSound.PlayOneShot(blockSoundClips[Random.Range(0, blockSoundClips.Length-1)]); 
        int blockType = Random.Range(1, 5);
        
        if (counter < numberOfSame)
        {
            Instantiate(Blocks[0], transform.position, Quaternion.identity);
            counter++;
            upNextScript.ClearShape();
            upNextScript.ChangeShape();
        }
        else if (counter >= numberOfSame)
        {
            Instantiate(Blocks[blockType], transform.position, Quaternion.identity);
            counter = 0;
            upNextScript.ClearShape();
            upNextScript.ChangeShape();
        }
        //Instantiate(Blocks[4], transform.position, Quaternion.identity);
        //Instantiate(Blocks[Random.Range(0, Blocks.Length)], transform.position, Quaternion.identity);
    }
}
