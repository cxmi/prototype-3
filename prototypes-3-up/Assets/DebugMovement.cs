using UnityEngine;

public class DebugMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private SpriteRenderer spriteRenderer;
    public PlanetColorRandomizer planetColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        spriteRenderer.color = planetColor.currentPlanetColor;

        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float moveY = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        Vector2 moveDir = new Vector2(moveX, moveY).normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
}
