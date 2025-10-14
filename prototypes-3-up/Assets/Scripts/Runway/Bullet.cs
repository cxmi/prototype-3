using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;
    private float lifeTimer;
    public SpriteRenderer spriteRenderer;
    public Material switchToMaterial;
    public Material startingMaterial;
    public AudioSource audioSource;
    public AudioClip audioClip;
    
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        startingMaterial = spriteRenderer.material;
        audioSource = GetComponent<AudioSource>();
        audioClip = audioSource.clip;
    }
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
            spriteRenderer.material = startingMaterial;
            spriteRenderer.sortingOrder = 10;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.material = switchToMaterial;
            spriteRenderer.sortingOrder = 15;
            audioSource.PlayOneShot(audioClip);
        }
    }
}