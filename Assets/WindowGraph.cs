using System.Collections.Generic;
using UnityEngine;

public class WindowGraph : MonoBehaviour
{
    public GameObject pointPrefab;
    public int backgroundSortingOrder = 0;
    public int lineSortingOrder = 1;
    public int pointSortingOrder = 2;
    public float xAxisLength;
    public float yAxisLength;
    public float xAxisScale = 0.1f;
    public float yAxisScale = 0.1f;
    public int xInterval = 1;
    public int yInterval = 1;
    public int curveResolution = 20;

    private float[] spawnXPositions = {  0.0001f, 0.0001f, 0.0002f, 0.0002f, 0.0004f, 0.0006f, 0.001f, 0.002f,0.01f };
    private float[] spawnYPositions = { 15.957f, 12.670f,9.818f, 7.401f, 5.419f, 3.872f, 2.761f, 2.085f, 1.844f};


    void Start()
    {
        for (int i = 0; i < spawnXPositions.Length-1; i++)
        {
            spawnXPositions[i] *= 10000f;
            spawnYPositions[i] *= 10f;
        }
        CalculateAxisLength();
        DrawAxes();
        DrawContinuousCurvedGraph();
        SpawnPoints();
    }

    void CalculateAxisLength()
    {
        float maxX = Mathf.Max(spawnXPositions);
        float maxY = Mathf.Max(spawnYPositions);
        float minX = Mathf.Min(spawnXPositions);
        float minY = Mathf.Min(spawnYPositions);

        // Calculate the axis lengths based on the maximum and minimum values
        xAxisLength = Mathf.Ceil((Mathf.Max(maxX, 0f)) / xInterval) * xInterval * xAxisScale;
        yAxisLength = Mathf.Ceil((Mathf.Max(maxY, 0f)) / yInterval) * yInterval * yAxisScale;

        // If there are negative values, adjust the axis lengths and positions
        if (minX < 0f)
        {
            xAxisLength += Mathf.Ceil((-minX + 10f) / xInterval) * xInterval * xAxisScale;

            // Extend the x-axis line to the left
            DrawLine(new Vector3(0f, 0f, 0f), new Vector3(-minX * xAxisScale, 0f, 0f), backgroundSortingOrder, Color.black);
            DrawScaleMarkingsX(minX);
        }

        if (minY < 0f)
        {
            yAxisLength += Mathf.Ceil((-minY + 10f) / yInterval) * yInterval * yAxisScale;
        }
    }

    void DrawScaleMarkingsX(float minX)
    {
        for (float i = 0f; i >= minX; i -= xInterval)
        {
            Vector3 position = new Vector3(i * xAxisScale, -0.5f, 0f);
            DrawText(i.ToString(), position);
        }
    }



    void DrawAxes()
    {
        DrawLine(new Vector3(0f, 0f, 0f), new Vector3(xAxisLength, 0f, 0f), backgroundSortingOrder, Color.black);
        DrawScaleMarkingsX();

        DrawLine(new Vector3(0f, 0f, 0f), new Vector3(0f, yAxisLength, 0f), backgroundSortingOrder, Color.black);
        DrawScaleMarkingsY();
    }

    void DrawScaleMarkingsX()
    {
        for (float i = 0; i <= xAxisLength; i += xInterval)
        {
            Vector3 position = new Vector3(i * xAxisScale, -0.5f, 0f);
            DrawText((i / 10f).ToString(), position);
        }
    }

    void DrawScaleMarkingsY()
    {
        for (float i = 0; i <= yAxisLength; i += yInterval)
        {
            Vector3 position = new Vector3(-1.5f, i * yAxisScale, 0f);
            DrawText((i / 10f).ToString(), position);
        }
    }

    void DrawText(string text, Vector3 position)
    {
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(transform);
        textObject.transform.position = position;

        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = 10;
        textMesh.color = Color.black;
    }

    void DrawContinuousCurvedGraph()
    {
        if (spawnXPositions.Length != spawnYPositions.Length || spawnXPositions.Length < 2)
        {
            Debug.LogError("The length of spawnXPositions and spawnYPositions should be at least 2 and equal.");
            return;
        }

        // Draw curved connections between intermediate points
        for (int i = 0; i < spawnXPositions.Length - 2; i++)
        {
            Vector3 p0 = i > 0 ? new Vector3(spawnXPositions[i - 1] * xAxisScale, spawnYPositions[i - 1] * yAxisScale, 0f) : new Vector3(spawnXPositions[i] * xAxisScale, spawnYPositions[i] * yAxisScale, 0f);
            Vector3 p1 = new Vector3(spawnXPositions[i] * xAxisScale, spawnYPositions[i] * yAxisScale, 0f);
            Vector3 p2 = new Vector3(spawnXPositions[i + 1] * xAxisScale, spawnYPositions[i + 1] * yAxisScale, 0f);
            Vector3 p3 = i < spawnXPositions.Length - 2 ? new Vector3(spawnXPositions[i + 2] * xAxisScale, spawnYPositions[i + 2] * yAxisScale, 0f) : new Vector3(spawnXPositions[i + 1] * xAxisScale, spawnYPositions[i + 1] * yAxisScale, 0f);

            Vector3[] splinePoints = CalculateCatmullRomSplinePoints(p0, p1, p2, p3, curveResolution);
            DrawContinuousCurvedLine(splinePoints, lineSortingOrder, Color.black);
        }
    }

    void DrawContinuousCurvedLine(Vector3[] splinePoints, int sortingOrder, Color color)
    {
        GameObject lineObject = new GameObject("ContinuousCurvedLine");
        lineObject.transform.SetParent(transform);

        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.sortingOrder = sortingOrder;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.positionCount = splinePoints.Length;
        lineRenderer.SetPositions(splinePoints);
    }

    Vector3[] CalculateCatmullRomSplinePoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int resolution)
    {
        List<Vector3> splinePoints = new List<Vector3>();

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;

            // Catmull-Rom interpolation formula
            float t2 = t * t;
            float t3 = t2 * t;

            Vector3 point = 0.5f * ((2.0f * p1) +
                                    (-p0 + p2) * t +
                                    (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t2 +
                                    (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3);

            splinePoints.Add(point);
        }

        return splinePoints.ToArray();
    }

    Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    void DrawLine(Vector3 startPoint, Vector3 endPoint, int sortingOrder, Color color)
    {
        GameObject lineObject = new GameObject("Line");
        lineObject.transform.SetParent(transform);

        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.sortingOrder = sortingOrder;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    void SpawnPoints()
    {
        for (int i = 0; i < spawnXPositions.Length-1; i++)
        {
            // Scale and position the points using the array values
            Vector3 spawnPoint = new Vector3(spawnXPositions[i] * xAxisScale, spawnYPositions[i] * yAxisScale, 0f);

            // Instantiate the point prefab at each point
            GameObject newPoint = Instantiate(pointPrefab, spawnPoint, Quaternion.identity);

            // Set the sorting order to make sure the points are rendered in front
            SpriteRenderer pointRenderer = newPoint.GetComponent<SpriteRenderer>();
            if (pointRenderer != null)
            {
                pointRenderer.sortingOrder = pointSortingOrder;
                pointRenderer.color = Color.black;
            }

            // Attach the new point to the canvas or any other desired parent
            newPoint.transform.SetParent(transform);

            DrawText($"({spawnXPositions[i]}, {spawnYPositions[i]})", spawnPoint + new Vector3(0f, 0.5f, 0f));
        }
    }



}

