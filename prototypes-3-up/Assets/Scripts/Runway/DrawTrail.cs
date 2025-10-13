using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class DrawTrail : MonoBehaviour
{
    public float pointSpacing = 0.1f;  // Minimum distance between points
    public int maxPoints = 1000;       // Optional: max trail length

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();

    private Vector3 lastPoint;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        points.Add(transform.position);
        lastPoint = transform.position;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, lastPoint);

        if (dist >= pointSpacing)
        {
            points.Add(transform.position);
            lastPoint = transform.position;

            if (points.Count > maxPoints)
                points.RemoveAt(0);

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
    }
}