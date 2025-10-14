using UnityEngine;

public class BulletAudio : MonoBehaviour
{
    
    public int playInt ;
    public int maxValue;
    
    public GameObject bulletPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playInt = 0;
        maxValue = bulletPrefab.GetComponent<Bullet>().audioClips.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void IncrementClip()
    {
        playInt = (playInt + 1) % maxValue;
    }

}
