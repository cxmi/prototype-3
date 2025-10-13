using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float spawnRate = 0.1f;         // Seconds between bullets

    [Header("Spawner Rotation")]
    public float rotationSpeed = 90f;      // Degrees per second

    private float timer = 0f;

    void Update()
    {
        // Rotate around Y to stay on XZ plane
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        // Spawn bullets on timer
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnBullet();
            timer = 0f;
        }
    }

    void SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * bulletSpeed;
        }
    }
}