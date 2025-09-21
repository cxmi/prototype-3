using System.Collections;
using UnityEngine;

public class ContactLogic : MonoBehaviour
{
    public Color[] availableColors;
    public static int score = 0;
    public AudioSource musicSource;
 

    //public ColorRandomizer colorRandomizer;
    public PlanetColorRandomizer planetColor;

    //private float pauseDuration = 0.25f;

    private SpriteRenderer sr;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        //colorRandomizer = GetComponent<ColorRandomizer>();
        sr = GetComponent<SpriteRenderer>();
        planetColor = FindFirstObjectByType<PlanetColorRandomizer>();
        musicSource = FindFirstObjectByType<AudioSource>();  
   

        availableColors = new Color[]
        {
            ColorRandomizer.red, ColorRandomizer.fuchsia, ColorRandomizer.green,
            ColorRandomizer.fuchsia, ColorRandomizer.yellow, ColorRandomizer.aqua
        };

        
    }

    void Update()
    {
        Debug.Log(score);
    }
    // IEnumerator BriefPause()
    // {
    //     musicSource.Pause();
    //     yield return new WaitForSeconds(pauseDuration);
    //     musicSource.UnPause();
    // }
    
    


    
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        // Check if other object is tagged "planet"
        if (other.CompareTag("Planet"))
        {
            // Get our color and the other's SpriteRenderer
            Color ourColor = sr.color;
            SpriteRenderer planetSR = other.GetComponent<SpriteRenderer>();

            if (planetSR == null) return; // Planet has no sprite renderer

            // Check if our color is planet color
            if (ourColor == planetColor.currentPlanetColor)
            {
                // Increase score
                score += 1;
                Debug.Log("Score: " + score);

                // Change planet color to a new random color from the array
                planetColor.ChangePlanetColor();
                FindFirstObjectByType<ScreenShake>().Shake();
                //musicSource.pitch += 0.025f;
                
                FindFirstObjectByType<LowPassScript>().TriggerLowPassEffect();

                //StartCoroutine(BriefPause());

                //debug - comment out below
                Destroy(gameObject);
            }
        }
    }


    
}
