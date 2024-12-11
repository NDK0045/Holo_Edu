using System.Collections.Generic;
using UnityEngine;

public class ARMeshRenderer : MonoBehaviour
{
    private Dictionary<Vector3, Point> uniquePoints;
    private List<Line> lines;

    private GameObject currentMeshObject;

    // Method to receive data
    public void InitializeMeshData(Dictionary<Vector3, Point> points, List<Line> lineData)
    {
        if (points == null || lineData == null)
        {
            Debug.LogError("Error: Null data provided to InitializeMeshData.");
            return;
        }

        if (points.Count == 0 || lineData.Count == 0)
        {
            Debug.LogError("Error: Empty data provided to InitializeMeshData.");
            return;
        }

        uniquePoints = points;
        lines = lineData;
    }


    // Method to trigger mesh respawning
    public void RespawnMesh(GameObject go)
    {
        // Clear the previous mesh if it exists
        ClearExistingMesh();

        // Create the mesh with the current data
        CreateMeshFromData(go.transform);
    }

    private void ClearExistingMesh()
    {
        if (currentMeshObject != null)
        {
            Destroy(currentMeshObject);
        }
    }
    
private void CreateMeshFromData(Transform parentTransform, float lineThickness = 0.2f)
{
    // Check for null or uninitialized collections
    if (uniquePoints == null || lines == null)
    {
        Debug.LogError("Error: uniquePoints or lines is null. Ensure InitializeMeshData is called with valid data.");
        return;
    }

    // Check for empty collections
    if (uniquePoints.Count == 0 || lines.Count == 0)
    {
        Debug.LogError("Error: uniquePoints or lines are empty. Cannot create mesh.");
        return;
    }

    List<Vector3> vertices = new List<Vector3>(uniquePoints.Keys);

    // Map each Point's position to its index in the vertices list
    Dictionary<Vector3, int> vertexIndices = new Dictionary<Vector3, int>();
    for (int i = 0; i < vertices.Count; i++)
    {
        vertexIndices[vertices[i]] = i;

		Debug.Log($"Point number {i} has {vertices[i]}");
    }

    // Create indices for triangles (to form a tube)
    List<int> indices = new List<int>();
    List<Vector3> finalVertices = new List<Vector3>();

    foreach (var line in lines)
    {
        // Check for null StartPoint or EndPoint
        if (line.StartPoint == null || line.EndPoint == null)
        {
            Debug.LogWarning("Warning: Line contains a null StartPoint or EndPoint. Skipping this line.");
            continue;
        }

        Vector3 start = line.StartPoint.Position;
        Vector3 end = line.EndPoint.Position;

        // Direction of the line
        Vector3 direction = (end - start).normalized;

        // Two perpendicular vectors for creating a rounded tube
        Vector3 perpendicular1 = Vector3.Cross(direction, Vector3.up).normalized * lineThickness;
        if (perpendicular1.magnitude < 0.1f)
            perpendicular1 = Vector3.Cross(direction, Vector3.right).normalized * lineThickness;

        Vector3 perpendicular2 = Vector3.Cross(direction, perpendicular1).normalized * lineThickness;

        // Add vertices around the line's start and end points
        Vector3[] tubeVertices = {
            start + perpendicular1, start - perpendicular1,
            start + perpendicular2, start - perpendicular2,
            end + perpendicular1, end - perpendicular1,
            end + perpendicular2, end - perpendicular2
        };

        int startIndex = finalVertices.Count;
        finalVertices.AddRange(tubeVertices);

        // Create triangles for the tube
        int[] tubeIndices = {
            startIndex, startIndex + 1, startIndex + 4,
            startIndex + 1, startIndex + 5, startIndex + 4,
            startIndex + 2, startIndex + 3, startIndex + 6,
            startIndex + 3, startIndex + 7, startIndex + 6,
            startIndex + 0, startIndex + 2, startIndex + 4,
            startIndex + 2, startIndex + 6, startIndex + 4,
            startIndex + 1, startIndex + 3, startIndex + 5,
            startIndex + 3, startIndex + 7, startIndex + 5
        };

        indices.AddRange(tubeIndices);
    }

    // Create the mesh
    Mesh mesh = new Mesh
    {
        vertices = finalVertices.ToArray()
    };
    mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

    currentMeshObject = new GameObject("MeshObject");
    currentMeshObject.transform.SetParent(parentTransform, false);

    MeshFilter meshFilter = currentMeshObject.AddComponent<MeshFilter>();
    MeshRenderer meshRenderer = currentMeshObject.AddComponent<MeshRenderer>();

    meshFilter.mesh = mesh;

    // Use a simple Unlit material
    Material simpleMaterial = new Material(Shader.Find("Unlit/Color"));
    simpleMaterial.color = Color.red; // Set the color of the material

    // Apply the material to the MeshRenderer
    meshRenderer.material = simpleMaterial;
}
}