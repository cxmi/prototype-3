using UnityEngine;

public class ColorAdjuster : MonoBehaviour
{
    public KeyCode keyBind = KeyCode.W; // Key to hold for animation
    public float speed = 1f;                 // Speed of green channel change
    public float minG = 0.2f;                // Min green value
    public float maxG = 1f;                  // Max green value

    private SpriteRenderer spriteRenderer;
    private float currentG;
    private bool increasing = true;

    public bool changeG = false;
    public bool changeR = false;
    public bool changeB = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (changeG)
        {
            currentG = spriteRenderer.color.g; 
        }

        else if (changeR)
        {
            currentG = spriteRenderer.color.r; // Start from current green channel

        }
        
        else if (changeB)
        {
            currentG = spriteRenderer.color.b;
        }
    }

    void Update()
    {
        if (Input.GetKey(keyBind))
        {
            float delta = speed * Time.deltaTime;

            if (increasing)
            {
                currentG += delta;
                if (currentG >= maxG)
                {
                    currentG = maxG;
                    increasing = false;
                }
            }
            else
            {
                currentG -= delta;
                if (currentG <= minG)
                {
                    currentG = minG;
                    increasing = true;
                }
            }

            Color c = spriteRenderer.color;

            if (changeG)
            {
                c.g = currentG;
            }
            else if (changeR)
            {
                c.r = currentG;

            }
            else if (changeB)
            {
                c.b = currentG;

            }
            spriteRenderer.color = c;
        }
    }
}
