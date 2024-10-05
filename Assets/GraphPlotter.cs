using UnityEngine;
using System.Collections.Generic;

public class GraphPlotter : MonoBehaviour
{
    public GameObject graphPointPrefab;
    private float startX = -100f;
    private float endX = 65f;
    private float step = 10f;

    private float[] spawnXPositions = { 0.0001f, 0.0001f, 0.0002f, 0.0002f, 0.0004f, 0.0006f, 0.001f, 0.002f, 0.01f };
    private float[] spawnYPositions = { 15.957f, 12.670f, 9.818f, 7.401f, 5.419f, 3.872f, 2.761f, 2.085f, 1.844f };
    void Start()
    {
        if (graphPointPrefab == null)
        {
            Debug.LogError("Prefab not set.");
            return;
        }

        PlotGraph();
        DrawAxes();
    }

    void DrawAxes()
    {
        float startY = -5f;  // Adjust based on your new Y-axis length
        float endY = 55f;  // Adjust based on your new Y-axis length
        startX = -15f;  // Adjust based on your new origin

        // Draw x-axis
        DrawLine(new Vector3(startX, -5, 0), new Vector3(endX, -5, 0), Color.black);  // Move x-axis to y=-5
        DrawScaleMarkingsX(startX);

        // Draw y-axis
        DrawLine(new Vector3(startX, startY, 0), new Vector3(startX, endY, 0), Color.black);
        DrawScaleMarkingsY(startY, endY);
    }


    void DrawScaleMarkingsY(float minY, float maxY)
    {
        float yInterval = 5f;
        for (float i = minY; i <= maxY; i += yInterval)
        {
            Vector3 position = new Vector3(startX, i, 0);
           
        }
    }

    void DrawScaleMarkingsX(float minX)
    {
        float xInterval = 10f;
        for (float i = minX; i <= endX; i += xInterval)
        {
            Vector3 position = new Vector3(i, -0.5f, 0f);
           
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

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject lineObject = new GameObject("Line");
        lineObject.transform.SetParent(transform);
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = color };
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void InstantiateTextAbovePrefab(Vector3 position, float point_written_x, float point_written_y)
    {
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(transform);
        textObject.transform.position = position + new Vector3(0, 1, 0);

        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = $"({point_written_x},{point_written_y})";
        textMesh.fontSize = 10;
        textMesh.color = Color.black;
    }
    void PlotGraph()
    {
        List<Vector3> prefabPoints = new List<Vector3>();
        List<Vector3> splinePoints = new List<Vector3>();
        int point_x = -20, point_end = 65;
        int numPoints = 10;  // Number of points to be spawned
        step = (point_end - point_x) / (float)(numPoints - 1);
        int point_index_text = 0;

        // Create a new GameObject to hold the line segments
        GameObject lineObject = new GameObject("LineSegments");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = Color.blue };
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.positionCount = numPoints - 1;  // Set the number of positions to the number of points excluding the first one

        for (float x = point_x + step; x <= point_end; x += step)
        {
            float y = CalculateY(x);
            Vector3 spawnPosition = new Vector3(x, y, 0);
            prefabPoints.Add(spawnPosition);
            Instantiate(graphPointPrefab, spawnPosition, Quaternion.identity);  // Instantiate a prefab at each point

            if (point_index_text < 9)
            {
                float point_written_x = spawnXPositions[point_index_text];
                float point_written_y = spawnYPositions[point_index_text];
                InstantiateTextAbovePrefab(spawnPosition, point_written_x, point_written_y);
            }

            point_index_text++;
        }

        // Connect only the prefabs in the line renderer
        lineRenderer.SetPositions(prefabPoints.ToArray());
    


    // Create a new GameObject to hold the spline


    // Set the number of positions to the number of points on the spline
    /*
    lineRenderer.positionCount = (points.Count - 1) * 100;  // Adjust this value to change the smoothness of the spline

    int positionIndex = 0;
    for (int i = 0; i < 15; i++)  // Stop one point earlier
    {
        for (float t = 0; t < 0.02; t += 0.01f)
        {
            Vector3 pointOnSpline = CatmullRomSpline(points[i], points[i + 1], points[i + 2], points[i + 3], t);
            lineRenderer.SetPosition(positionIndex, pointOnSpline);
            positionIndex++;
        }
    }
    */



}




float CalculateY(float x)
    {
        return 19.6865f * Mathf.Exp(-0.0914f * x);
    }

    public static Vector3 CatmullRomSpline(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        return 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));
    }
}
