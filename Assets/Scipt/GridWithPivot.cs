using UnityEngine;

public class GridWithPivot : MonoBehaviour
{
    public int gridSize = 10; // Number of lines extending in both directions
    public float gridSpacing = 1.0f; // Distance between grid lines
    public Material lineMaterial; // Material for grid lines

    // Colors for each axis grid
    public Color colorXY = new Color(0.7f, 0.7f, 0.7f, 0.2f);
    public Color colorXZ = new Color(0.7f, 0.7f, 1.0f, 0.2f);
    public Color colorYZ = new Color(0.7f, 1.0f, 0.7f, 0.2f);

    private GameObject gridParentXY;
    private GameObject gridParentXZ;
    private GameObject gridParentYZ;

    void Start()
    {
        // Create grids for each plane
        gridParentXY = CreateGrid("GridXY", colorXY, Vector3.right, Vector3.up);
        gridParentXZ = CreateGrid("GridXZ", colorXZ, Vector3.right, Vector3.forward);
        gridParentYZ = CreateGrid("GridYZ", colorYZ, Vector3.forward, Vector3.up);
    }

    private GameObject CreateGrid(string name, Color color, Vector3 axis1, Vector3 axis2)
    {
        // Create a parent object for the grid
        GameObject gridParent = new GameObject(name);
        gridParent.transform.SetParent(transform);
        gridParent.transform.localPosition = Vector3.zero;
        gridParent.transform.localRotation = Quaternion.identity;
        gridParent.transform.localScale = Vector3.one;

        // Draw grid lines
        for (int i = -gridSize; i <= gridSize; i++)
        {
            float positionOffset = i * gridSpacing;

            // Line parallel to axis1
            CreateLine(gridParent.transform,
                positionOffset * axis1 + (-gridSize * gridSpacing * axis2),
                positionOffset * axis1 + (gridSize * gridSpacing * axis2),
                color
            );

            // Line parallel to axis2
            CreateLine(gridParent.transform,
                (-gridSize * gridSpacing * axis1) + positionOffset * axis2,
                (gridSize * gridSpacing * axis1) + positionOffset * axis2,
                color
            );
        }

        return gridParent;
    }

    private void CreateLine(Transform parent, Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("Line");
        line.transform.SetParent(parent);

        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial ? lineMaterial : new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false; // Use local space for line rendering
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
