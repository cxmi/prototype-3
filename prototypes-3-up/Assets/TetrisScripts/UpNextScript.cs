using UnityEngine;
using UnityEngine.SceneManagement;

public class UpNextScript : MonoBehaviour
{
    public GameObject[] NextBlock;
    public Color blockColor;

    private int blockTracker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blockColor = new Color32(0, 34, 77, 255);
        //Instantiate(NextBlock[Random.Range(0, NextBlock.Length)], transform.position, Quaternion.identity);

        NextBlock[0].SetActive(true);
        blockTracker = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeShape()
    {
        int random = Random.Range(0, NextBlock.Length);

        if (blockTracker < 2)
        {
            GameObject block = NextBlock[Random.Range(0,4)];
            block.SetActive(true);
            blockTracker++;
        }
        else if (blockTracker >= 2)
        {
            GameObject newBlock = NextBlock[random];
            newBlock.SetActive(true);
        }

        
       // Instantiate(NextBlock[Random.Range(0, NextBlock.Length)], transform.position, Quaternion.identity);

    }

    public void ClearShape()
    {
        foreach (GameObject obj in NextBlock)
        {
            obj.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
