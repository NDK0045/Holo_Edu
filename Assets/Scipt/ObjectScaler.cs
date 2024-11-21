using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    // Max and min boundaries for x, y, z
    public Vector3 minBoundary;
    public Vector3 maxBoundary;

    // The target object to scale and check bounds
    public GameObject targetObject;

    void Update()
    {
        // Check for a key press to trigger scaling and boundary printing
        
            ScaleAndPrintBounds(targetObject);
        
    }

    // Method to calculate bounds, scale the object, and print farthest points
    private void ScaleAndPrintBounds(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Target object is not assigned.");
            return;
        }

        // Get the object's combined bounds (including all children)
        Bounds combinedBounds = CalculateCombinedBounds(obj);

        // Calculate scaling factors for each axis based on boundaries
        float scaleX = (maxBoundary.x - minBoundary.x) / (combinedBounds.max.x - combinedBounds.min.x);
        float scaleY = (maxBoundary.y - minBoundary.y) / (combinedBounds.max.y - combinedBounds.min.y);
        float scaleZ = (maxBoundary.z - minBoundary.z) / (combinedBounds.max.z - combinedBounds.min.z);

        // Determine the smallest scale factor to maintain aspect ratio
        float minScale = Mathf.Min(scaleX, Mathf.Min(scaleY, scaleZ));

        // Apply scaling if the object exceeds the boundaries
        if (minScale < 1f)  // Only scale down, not up
        {
            
            obj.transform.localScale *= minScale;
        }

        Debug.LogWarning(minScale);
        // Print the farthest points in the x, y, and z directions
        PrintFarthestPoints(combinedBounds);
    }

    // Calculate combined bounds for the object and its children
    private Bounds CalculateCombinedBounds(GameObject obj)
    {
        // Initialize bounds
        Bounds combinedBounds = new Bounds(obj.transform.position, Vector3.zero);

        // Get all Renderer components in the object and its children
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        // Expand combined bounds to include each renderer's bounds
        foreach (Renderer renderer in renderers)
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }

        return combinedBounds;
    }

    // Print the farthest points in the x, y, and z directions
    private void PrintFarthestPoints(Bounds bounds)
    {
        Vector3 minPoint = bounds.min;
        Vector3 maxPoint = bounds.max;

        Debug.Log($"Farthest Min Point: X={minPoint.x}, Y={minPoint.y}, Z={minPoint.z}");
        Debug.Log($"Farthest Max Point: X={maxPoint.x}, Y={maxPoint.y}, Z={maxPoint.z}");
    }
}
