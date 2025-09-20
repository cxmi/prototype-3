using UnityEngine;

public class PlanetColorRandomizer : MonoBehaviour
{
    public Color[] planetColors;

    public ColorRandomizer colorRandomizer;
    
    private SpriteRenderer spriteRenderer;
    
    public Color currentPlanetColor;
    
    public float scaleAmount = 0.05f; // scale up size


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planetColors = new Color[]
        {
            ColorRandomizer.green, ColorRandomizer.yellow, ColorRandomizer.aqua
        };
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = planetColors[Random.Range(0, planetColors.Length)];

    }

    // Update is called once per frame
    void Update()
    {
        currentPlanetColor = spriteRenderer.color;
    }

    public void ChangePlanetColor()
    {
        spriteRenderer.color = planetColors[Random.Range(0, planetColors.Length)];
        transform.localScale += new Vector3(scaleAmount, scaleAmount, 0f);

    }
}
