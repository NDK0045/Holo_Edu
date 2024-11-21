using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    public GameObject targetObject; // The shape to position and scale
    public Camera mainCamera;       // The camera to ensure visibility

    // The positions of the trackable objects
    public Vector3[] trackablePositions = new Vector3[]
    {
        new Vector3(-0.057934761f, 0.00599999726f, 0.065199852f), // Ground floor
        new Vector3(-0.6673784f, 0.5292873f, 0.8120029f),   // Trackable 2
        new Vector3(-1.24400032f, 0.531644821f, 0.0279531479f)    // Foreground background
    };

    // Offset to apply to the shape's position
    public Vector3 positionOffset = new Vector3(0f, 0f, -0.2f); // Move slightly to the left

    void Start()
    {
        if (targetObject == null || mainCamera == null)
        {
            Debug.LogError("Target object or main camera is not assigned.");
            return;
        }

        PositionAndScaleShape();
    }

    private void PositionAndScaleShape()
    {
        // Manually specify only the positions of the ground and wall planes
        Vector3[] relevantTrackablePositions = new Vector3[]
        {
        trackablePositions[0], // Ground plane
        trackablePositions[2]  // Foreground background (wall plane)
        };

        // Calculate the bounding box of the relevant planes
        Vector3 minBounds = Vector3.positiveInfinity;
        Vector3 maxBounds = Vector3.negativeInfinity;

        foreach (Vector3 pos in relevantTrackablePositions)
        {
            minBounds = Vector3.Min(minBounds, pos);
            maxBounds = Vector3.Max(maxBounds, pos);
        }

        // Calculate the center of the bounding box
        Vector3 center = (minBounds + maxBounds) / 2;

        // Apply the offset to the center position
        center += positionOffset;

        // Calculate the size of the bounding box
        Vector3 size = maxBounds - minBounds;

        // Position the shape at the center
        targetObject.transform.position = center;

        // Ensure scaling is done consistently based on the original scale
        ScaleShapeToFit(size);

        // Adjust camera position and field of view if necessary
        AdjustCameraToView(center, size);
    }

    private void ScaleShapeToFit(Vector3 targetSize)
    {
        // Get the original bounds of the object
        Bounds shapeBounds = CalculateCombinedBounds(targetObject);

        if (shapeBounds.size == Vector3.zero)
        {
            Debug.LogWarning("Shape bounds are zero. Scaling will not be applied.");
            return;
        }

        // Calculate scaling factors while preserving the original aspect ratio
        //float minScaleFactor = 0.5f;
        float scaleX = targetSize.x / shapeBounds.size.x;
        float scaleY = targetSize.y / shapeBounds.size.y;
        float scaleZ = targetSize.z / shapeBounds.size.z;
        float uniformScale = Mathf.Min(scaleX, Mathf.Min(scaleY, scaleZ));

        // Apply uniform scaling to the shape, relative to its original size
        //float desiredScale = 1.5f; // Desired fixed size for the shape
        //targetObject.transform.localScale = Vector3.one * desiredScale;

        targetObject.transform.localScale = Vector3.one * uniformScale;
    }



    private Bounds CalculateCombinedBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("No renderers found in target object.");
            return new Bounds(Vector3.zero, Vector3.zero);
        }

        Bounds combinedBounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }
        return combinedBounds;
    }

    private void AdjustCameraToView(Vector3 center, Vector3 size)
    {
        // Position the camera to ensure visibility
        float maxSize = Mathf.Max(size.x, size.y, size.z);
        float distance = maxSize / (2 * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));

        Vector3 cameraDirection = (mainCamera.transform.position - center).normalized;
        mainCamera.transform.position = center + cameraDirection * distance;

        mainCamera.transform.LookAt(center);
    }

 

}
