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
        uniquePoints = points;
        lines = lineData;
    }

    // Method to trigger mesh respawning
    public void RespawnMesh(Transform parentTransform)
    {
        // Clear the previous mesh if it exists
        ClearExistingMesh();

        // Create the mesh with the current data
        CreateMeshFromData(parentTransform);
    }

    private void ClearExistingMesh()
    {
        if (currentMeshObject != null)
        {
            Destroy(currentMeshObject);
        }
    }
    
    private void CreateMeshFromData(Transform parentTransform, float lineThickness = 0.1f)
    {
        if (uniquePoints == null || lines == null || uniquePoints.Count == 0 || lines.Count == 0)
        {
            Debug.Log("Mesh data is not initialized or empty!");
            return;
        }

        List<Vector3> vertices = new List<Vector3>(uniquePoints.Keys);

        // Map each Point's position to its index in the vertices list
        Dictionary<Vector3, int> vertexIndices = new Dictionary<Vector3, int>();
        for (int i = 0; i < vertices.Count; i++)
        {
            vertexIndices[vertices[i]] = i;
        }

        // Create indices for triangles (to form a tube)
        List<int> indices = new List<int>();
        List<Vector3> finalVertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        foreach (var line in lines)
        {
            if (line.StartPoint != null && line.EndPoint != null)
            {
                Vector3 start = line.StartPoint.Position;
                Vector3 end = line.EndPoint.Position;

                // Direction of the line
                Vector3 direction = (end - start).normalized;

                // Perpendicular vector for creating the tube
                Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;
                if (perpendicular.magnitude < 0.1f)
                    perpendicular = Vector3.Cross(direction, Vector3.right).normalized;

                // Add vertices along the line to form a cylinder-like shape
                for (int i = 0; i < 2; i++)
                {
                    Vector3 offset = perpendicular * lineThickness * (i == 0 ? 1 : -1); // Offset for two sides of the line
                    Vector3 vertex1 = start + offset;
                    Vector3 vertex2 = end + offset;

                    finalVertices.Add(vertex1);
                    finalVertices.Add(vertex2);

                    // Normals for lighting (point in the direction of the line)
                    normals.Add(direction);
                    normals.Add(direction);
                }

                int vertexIndexStart = finalVertices.Count - 4;

                // Create triangles for the tube
                int[] tubeIndices = {
                    vertexIndexStart, vertexIndexStart + 1, vertexIndexStart + 2,
                    vertexIndexStart + 1, vertexIndexStart + 3, vertexIndexStart + 2
                };
                indices.AddRange(tubeIndices);
            }
        }

        // Create the mesh
        Mesh mesh = new Mesh
        {
            vertices = finalVertices.ToArray(),
            normals = normals.ToArray() // Add normals
        };
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

        currentMeshObject = new GameObject("MeshObject");
        currentMeshObject.transform.SetParent(parentTransform, false);

        MeshFilter meshFilter = currentMeshObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = currentMeshObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;

        Material glowMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        glowMaterial.EnableKeyword("_EMISSION");
        glowMaterial.SetColor("_BaseColor", Color.green); // Base color for the mesh
        glowMaterial.SetColor("_EmissionColor", Color.green * 2.0f); // Glow intensity

// Disable backface culling to render both sides
        glowMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);

// Apply the material to the MeshRenderer
        meshRenderer.material = glowMaterial;

    }


}
