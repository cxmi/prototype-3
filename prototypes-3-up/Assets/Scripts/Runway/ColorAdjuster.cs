using UnityEngine;
using UnityEngine.InputSystem;

public class ColorAdjuster : MonoBehaviour
{
    //InputSystem
    public InputActionReference inputAction;
    public KeyCode keyBind = KeyCode.W; 
   
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
        Vector2 input = inputAction.action.ReadValue<Vector2>();
        
        float colorChangeSpeed = 0;
        
        switch (keyBind)
        {
            case KeyCode.W:
                if (input.y > 0f)
                {
                    colorChangeSpeed = Mathf.Abs(input.y);
                }
                break;
            case KeyCode.S:
                if (input.y < 0f)
                {
                    colorChangeSpeed = Mathf.Abs(input.y);
                }
                break;
            case KeyCode.D:
                if (input.x > 0f)
                {
                    colorChangeSpeed = Mathf.Abs(input.x);
                }
                break;
            case KeyCode.A:
                if (input.y < 0f)
                {
                    colorChangeSpeed = Mathf.Abs(input.x);
                }
                break;
            default:
                break;
        }
        
        if (colorChangeSpeed > 0)
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
