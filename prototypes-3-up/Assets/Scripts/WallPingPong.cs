using UnityEngine;

public class WallPingPong : MonoBehaviour
{
    
    
    public float minScaleY = 0.5f;
    public float maxScaleY = 1.5f;
    public float speed = 0.5f;

    private Vector3 originalScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        float pingPong = Mathf.PingPong(Time.time * speed, 1f); // Moves between 0 and 1
        float newScaleY = Mathf.Lerp(minScaleY, maxScaleY, pingPong);

        transform.localScale = new Vector3(originalScale.x, newScaleY, originalScale.z);
    }
}
