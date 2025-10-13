using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class SpiralTrail : MonoBehaviour
{
    [Header("Spiral Settings")]
    public float rotationSpeed = 180f;
    public float pointSpacing = 0.1f;
    public float radius = 0.05f;
    public int maxPoints = 300;

    [Header("Fade Out")]
    public float fadeOutSpeed = 100f; // Points removed per second

    private float angle = 0f;
    private float distanceTraveled = 0f;

    private List<Vector3> points = new List<Vector3>();
    private LineRenderer lineRenderer;

    private bool fadingOut = false;
    private float fadeTimer = 0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (!fadingOut)
        {
            // Draw spiral
            angle += rotationSpeed * Time.deltaTime;
            distanceTraveled += pointSpacing;

            Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * distanceTraveled * radius;
            Vector3 newPoint = transform.position + offset;

            points.Add(newPoint);

            if (points.Count >= maxPoints)
            {
                fadingOut = true;
                return;
            }

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
        else
        {
            // Fade out: remove from beginning (bottom to top)
            fadeTimer += Time.deltaTime * fadeOutSpeed;

            int pointsToRemove = Mathf.FloorToInt(fadeTimer);
            fadeTimer -= pointsToRemove;

            if (pointsToRemove > 0)
            {
                points.RemoveRange(0, Mathf.Min(pointsToRemove, points.Count));
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());

                // If empty, reset spiral
                if (points.Count == 0)
                {
                    ResetSpiral();
                }
            }
        }
    }

    void ResetSpiral()
    {
        angle = 0f;
        distanceTraveled = 0f;
        fadingOut = false;
        fadeTimer = 0f;
        points.Clear();
        lineRenderer.positionCount = 0;
    }
}
