using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    public GameObject targetObject;
    public Camera mainCamera;
    public Vector3[] trackablePositions;
    public Vector3 positionOffset = new Vector3(0f, 0f, 0f);

    void Start()
    {
        if (targetObject == null || mainCamera == null)
        {
            Debug.LogError("Target object or main camera is not assigned.");
            return;
        }

        if (trackablePositions == null || trackablePositions.Length < 3)
        {
            Debug.LogError("Insufficient trackable positions provided.");
            return;
        }

        PositionAndScaleShape();
    }

    private void PositionAndScaleShape()
    {
        Vector3[] relevantPositions = { trackablePositions[0], trackablePositions[2] };

        Vector3 minBounds = Vector3.positiveInfinity;
        Vector3 maxBounds = Vector3.negativeInfinity;

        foreach (Vector3 pos in relevantPositions)
        {
            minBounds = Vector3.Min(minBounds, pos);
            maxBounds = Vector3.Max(maxBounds, pos);
        }

        Vector3 center = (minBounds + maxBounds) / 2 + positionOffset;
        Vector3 size = maxBounds - minBounds;

        targetObject.transform.position = center;
        ScaleShapeToFit(size);
        AdjustCameraToView(center, size);
    }

    private void ScaleShapeToFit(Vector3 targetSize)
    {
        Bounds shapeBounds = CalculateCombinedBounds(targetObject);
        if (shapeBounds.size == Vector3.zero)
        {
            Debug.LogWarning("Shape bounds are zero. Scaling skipped.");
            return;
        }

        float scaleX = targetSize.x / shapeBounds.size.x;
        float scaleY = targetSize.y / shapeBounds.size.y;
        float scaleZ = targetSize.z / shapeBounds.size.z;
        float uniformScale = Mathf.Min(scaleX, Mathf.Min(scaleY, scaleZ));

        targetObject.transform.localScale = Vector3.one * uniformScale;
    }

    private Bounds CalculateCombinedBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(Vector3.zero, Vector3.zero);

        Bounds combinedBounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }
        return combinedBounds;
    }

    private void AdjustCameraToView(Vector3 center, Vector3 size)
    {
        float maxSize = Mathf.Max(size.x, size.y, size.z);

        // Ensure the camera is far enough to see the entire object
        float distance = maxSize / (2 * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        distance *= 1.2f; // Add a buffer to ensure nothing is clipped

        Vector3 cameraDirection = (mainCamera.transform.position - center).normalized;

        // Calculate new camera position
        Vector3 newCameraPosition = center + cameraDirection * distance;

        // Move the camera and look at the center
        mainCamera.transform.position = newCameraPosition;
        mainCamera.transform.LookAt(center);

        // Adjust the near and far clipping planes
        mainCamera.nearClipPlane = 0.1f;
        mainCamera.farClipPlane = Mathf.Max(distance * 2f, 1000f);
    }
}
