using UnityEngine;

public class Stretching : MonoBehaviour
{
    public KeyCode keyBind = KeyCode.Space;

    [Header("Scale Limits")]
    public float minX = 1f;
    public float maxX = 1.5f;
    public float minY = 1f;
    public float maxY = 2f;

    [Header("Oscillation Speeds")]
    public float xSpeed = 2f;
    public float ySpeed = 1f;

    private float currentX;
    private float currentY;

    private bool xIncreasing = true;
    private bool yIncreasing = true;

    void Start()
    {
        Vector3 startScale = transform.localScale;
        currentX = startScale.x;
        currentY = startScale.y;
    }

    void Update()
    {
        if (Input.GetKey(keyBind))
        {
            float deltaX = xSpeed * Time.deltaTime;
            float deltaY = ySpeed * Time.deltaTime;

            // X scale logic
            if (xIncreasing)
            {
                currentX += deltaX;
                if (currentX >= maxX)
                {
                    currentX = maxX;
                    xIncreasing = false;
                }
            }
            else
            {
                currentX -= deltaX;
                if (currentX <= minX)
                {
                    currentX = minX;
                    xIncreasing = true;
                }
            }

            // Y scale logic
            if (yIncreasing)
            {
                currentY += deltaY;
                if (currentY >= maxY)
                {
                    currentY = maxY;
                    yIncreasing = false;
                }
            }
            else
            {
                currentY -= deltaY;
                if (currentY <= minY)
                {
                    currentY = minY;
                    yIncreasing = true;
                }
            }

            // Apply scale
            transform.localScale = new Vector3(currentX, currentY, transform.localScale.z);
        }
    }
}