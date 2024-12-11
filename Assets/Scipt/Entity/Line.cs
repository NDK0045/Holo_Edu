using UnityEngine;

public class Line
{
    public string Id { get; private set; }
    public Point StartPoint { get; private set; }
    public Point EndPoint { get; private set; }
    public GameObject GameObject { get; private set; }

    public Line(string id, Point startPoint, Point endPoint)
    {
        Id = id;
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    // Creates a GameObject with LineRenderer to display the line in Unity
    public void CreateGameObject(GameObject parentObject)
    {
        GameObject = new GameObject($"Line_{Id}");
        LineRenderer lineRenderer = GameObject.AddComponent<LineRenderer>();

		LineRendererScript lineScript = GameObject.AddComponent<LineRendererScript>();
        lineScript.SetPivotPoints(StartPoint.GameObject, EndPoint.GameObject);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, StartPoint.Position);
        lineRenderer.SetPosition(1, EndPoint.Position);

        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
            // Assign glow material
        Material glowMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        glowMaterial.EnableKeyword("_EMISSION");
        glowMaterial.SetColor("_BaseColor", Color.cyan);
        glowMaterial.SetColor("_EmissionColor", Color.cyan * 2.0f); // Adjust intensity
        lineRenderer.material = glowMaterial;

   		GameObject.transform.SetParent(parentObject.transform);
    }
}
