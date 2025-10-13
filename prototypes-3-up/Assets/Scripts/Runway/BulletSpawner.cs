using UnityEngine;
using System.Collections.Generic;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float spawnRate = 0.1f;
    public int poolSize = 100;

    [Header("Spawner Rotation")]
    public float rotationSpeed = 90f;

    private float spawnTimer;
    private List<GameObject> bulletPool;
    private int nextBulletIndex;

    void Start()
    {
        // Initialize pool
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f); // Y-axis for XZ plane

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            SpawnBullet();
            spawnTimer = 0f;
        }
    }

    void SpawnBullet()
    {
        GameObject bullet = GetPooledBullet();
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * bulletSpeed;
            }
        }
    }

    GameObject GetPooledBullet()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int index = (nextBulletIndex + i) % poolSize;
            if (!bulletPool[index].activeInHierarchy)
            {
                nextBulletIndex = (index + 1) % poolSize;
                return bulletPool[index];
            }
        }

        // All bullets in use — optional: expand pool or skip
        return null;
    }
}