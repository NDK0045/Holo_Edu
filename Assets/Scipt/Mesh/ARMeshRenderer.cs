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
        CreateMeshFromData(go);
    }

    private void ClearExistingMesh()
    {
        if (currentMeshObject != null)
        {
            Destroy(currentMeshObject);
        }
    }

    private void CreateMeshFromData(GameObject go)
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

        // Create the parent object for the spheres and lines
        //currentMeshObject = new GameObject("MeshObject");
        //currentMeshObject.transform.SetParent(go.transform, false);

        // Create spheres for each point
        foreach (var point in uniquePoints.Values)
        {
            point.CreateGameObject(go); // Create the GameObject for the point
			
        }

        // Create LineRenderers for each line
        foreach (var line in lines)
        {
            line.CreateGameObject(go); // Create the GameObject for the line
        }
    }
}
