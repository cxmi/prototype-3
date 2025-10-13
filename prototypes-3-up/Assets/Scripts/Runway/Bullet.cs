using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;
    private float lifeTimer;

    void OnEnable()
    {
        lifeTimer = 0f;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}