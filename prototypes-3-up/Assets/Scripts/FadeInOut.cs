using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public float fadeSpeed = 0.3f; // How fast to fade in and out

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // PingPong returns a value between 0 and 1 over time
        float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f);
        
        // Apply new alpha to sprite
        Color newColor = originalColor;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
    }
}
