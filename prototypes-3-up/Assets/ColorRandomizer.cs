using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
 
    private SpriteRenderer spriteRenderer;
    
    private Color yellow = new Color32(0xFE, 0xFC, 0x4E, 0xFF); // #FEFC4E
    private Color aqua   = new Color32(0x48, 0xF7, 0xF7, 0xFF); // #48F7F7
    private Color red    = new Color32(0xFF, 0x4F, 0x59, 0xFF); // #FF4F59
    private Color fuchsia= new Color32(0xF6, 0x30, 0xF9, 0xFF); // #F630F9
    private Color blue   = new Color32(0x52, 0x2A, 0xFC, 0xFF); // #522AFC
    private Color green  = new Color32(0x36, 0xFF, 0x3C, 0xFF); // #36FF3C
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // random color from the array
        Color[] allColors = { red, fuchsia, blue };
        spriteRenderer.color = allColors[Random.Range(0, allColors.Length)];

  
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            // Pick either yellow or red on collision with "wall"
            Color[] wallColors = { yellow, aqua, green };
            spriteRenderer.color = wallColors[Random.Range(0, wallColors.Length)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
