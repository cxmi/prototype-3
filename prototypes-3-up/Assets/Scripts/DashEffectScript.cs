using UnityEngine;

public class DashEffectScript : MonoBehaviour
{
    public float timer = 1f;
    private SpriteRenderer _spriteRenderer;
    public Color color;

    private float _timer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _timer = timer;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        _spriteRenderer.color = new Color(color.r, color.g, color.b, _timer / timer);
        
        if (_timer <= 0) Destroy(gameObject);
    }
}
