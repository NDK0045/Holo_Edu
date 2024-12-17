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
    // Create the GameObject for the line
    GameObject = new GameObject($"Line_{Id}");
    LineRenderer lineRenderer = GameObject.AddComponent<LineRenderer>();

    // Attach the LineRendererScript and set the pivot points
    LineRendererScript lineScript = GameObject.AddComponent<LineRendererScript>();
    lineScript.SetPivotPoints(StartPoint.GameObject, EndPoint.GameObject);

    // Set the line renderer's position count and positions
    lineRenderer.positionCount = 2;
    lineRenderer.SetPosition(0, StartPoint.Position + parentObject.transform.position);
    lineRenderer.SetPosition(1, EndPoint.Position + parentObject.transform.position);
 

    // Create a glowing material for the line
    Material glowingMaterial = new Material(Shader.Find("Unlit/Color"));
    glowingMaterial.SetColor("_Color", Color.red);  // Set color to green (or any color you prefer)
    glowingMaterial.EnableKeyword("_EMISSION");
    glowingMaterial.SetColor("_EmissionColor", Color.red * 2.0f);  // Increase the intensity for a glowing effect

    // Apply the material to the line renderer
    lineRenderer.material = glowingMaterial;

    // Set the parent of the line's GameObject
    GameObject.transform.SetParent(parentObject.transform);
}

}