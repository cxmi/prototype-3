using System.Collections;
using UnityEngine;

public class LowPassScript : MonoBehaviour
{
    private AudioLowPassFilter lowPass;

    public float lowFrequency = 500f;
    public float highFrequency = 5000f;
    public float effectDuration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lowPass = GetComponent<AudioLowPassFilter>();
        lowPass.cutoffFrequency = highFrequency;
    }

    public void TriggerLowPassEffect()
    {
        StopAllCoroutines(); // Cancel if already running
        StartCoroutine(LowPassCoroutine());
    }

    IEnumerator LowPassCoroutine()
    {
        float halfDuration = effectDuration / 2f;

        // Downward transition
        yield return StartCoroutine(ChangeCutoffFrequency(highFrequency, lowFrequency, halfDuration));

        // Upward transition
        yield return StartCoroutine(ChangeCutoffFrequency(lowFrequency, highFrequency, halfDuration));
    }

    IEnumerator ChangeCutoffFrequency(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            lowPass.cutoffFrequency = Mathf.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        lowPass.cutoffFrequency = to;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
