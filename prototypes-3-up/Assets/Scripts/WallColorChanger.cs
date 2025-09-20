using UnityEngine;

public class WallColorChanger : MonoBehaviour
{
    
    public float transitionDuration = 2f; // Time (in seconds) between color changes

    private SpriteRenderer sr;
    private int currentIndex = 0;
    private float t = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = GetColor(currentIndex);

    }

    // Update is called once per frame
    void Update()
    {
        
            t += Time.deltaTime / transitionDuration;

            Color fromColor = GetColor(currentIndex);
            Color toColor = GetColor((currentIndex + 1) % 3);
            sr.color = Color.Lerp(fromColor, toColor, t);

            if (t >= 1f)
            {
                currentIndex = (currentIndex + 1) % 3;
                t = 0f;
            }

    }
    
    Color GetColor(int index)
    {
        switch (index)
        {
            case 0: return ColorRandomizer.fuchsia;
            case 1: return ColorRandomizer.red;
            case 2: return ColorRandomizer.blue;
            default: return Color.white;
        }
    }
}
