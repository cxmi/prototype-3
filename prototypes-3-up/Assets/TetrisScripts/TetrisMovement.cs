using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TetrisMovement : MonoBehaviour
{
    private float previousTime;

    public float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    public Vector3 rotationPoint;
    public static int score = 0;
    private static Transform[,] grid = new Transform[width, height];
    public SpawnScript spawnScript;
    
    public GameObject quadObject;
    private float duration = 0.6f; // Duration of animation in seconds
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        spawnScript = FindFirstObjectByType<SpawnScript>();
        quadObject = GameObject.FindGameObjectWithTag("Weirdo");

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(score);
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left;
            if(!ValidMove())
                transform.position -= Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right;
            if (!ValidMove())
                transform.position -= Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90f);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90f);
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.S) ? fallTime / 10 : fallTime))
        {
            transform.position += Vector3.down;
            if (!ValidMove())
            {
                //this is when block touches the ground
                transform.position -= Vector3.down;
                AddToGrid();
                
                //check for game over
                
                if (IsGameOver())
                {
                    //Debug.Log("Game Over!");
                    // Stop the game
                    Time.timeScale = 0;
                    spawnScript.gameOverScreen.SetActive(true);
                    return;
                }

                CheckForLines();
                this.enabled = false;
                FindFirstObjectByType<SpawnScript>().NewBlock();

            }
            previousTime = Time.time;
        }
        
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
                return false;
        }

        return true;
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            
            grid[roundedX,roundedY] = children;
        }
    }

    void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                
                DeleteLine(i);
                RowDown(i);
                score++;
                StartCoroutine(ExpandAndFadeIn(quadObject, duration));
                spawnScript.blockSound.PlayOneShot(spawnScript.blockSoundClips[4]);
                        StartCoroutine(ExpandAndFadeIn(quadObject, duration));

            }
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }

        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j,i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y-1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
    
    bool IsGameOver()
    {
        foreach (Transform child in transform)
        {
            int y = Mathf.RoundToInt(child.transform.position.y);
            if (y >= height - 1) // or height, depending on your grid definition
            {
                return true;
            }
        }
        return false;
    }
    
    public IEnumerator ExpandAndFadeIn(GameObject quadObject, float duration)
    {
        if (quadObject == null)
            yield break;

        Renderer renderer = quadObject.GetComponent<Renderer>();
        if (renderer == null || renderer.material == null)
            yield break;

        // Initial setup
        quadObject.transform.localScale = Vector3.zero;

        // Set initial transparent color
        Color originalColor = renderer.material.color;
        Color startColor = originalColor;
        startColor.a = 0f;
        renderer.material.color = startColor;

        // Target scale
        Vector3 targetScale = new Vector3(30f, 18f, 11f);
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // Scale
            quadObject.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);

            // Fade in alpha
            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(0f, originalColor.a, t);
            renderer.material.color = newColor;

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final state
        quadObject.transform.localScale = targetScale;
        renderer.material.color = originalColor;

        // Optional: Wait briefly before hiding
        yield return new WaitForSeconds(0.4f);

        // Hide the object
        quadObject.SetActive(false);
    }
}
