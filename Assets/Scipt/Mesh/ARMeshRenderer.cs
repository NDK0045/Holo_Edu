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
    
    private void CreateMeshFromData(GameObject go, float lineThickness = 0.2f)
    {
		Transform parentTransform = go.transform;
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

		foreach (var v3 in vertices) {
			if (uniquePoints.TryGetValue(v3, out Point p))
			{
    			p.CreateGameObject(go);
			}
			else
			{
   			Debug.LogWarning($"Warning: Key {v3} not found in uniquePoints dictionary.");
			}
		}
 
        foreach (var line in lines)
        {
            line.CreateGameObject(go);
        }

        // Create the mesh
        
    }




}
