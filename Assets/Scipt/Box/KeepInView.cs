using UnityEngine;

public class KeepInView : MonoBehaviour
{
    private Camera mainCamera;
    private BoxCollider boxCollider;

    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;

        // Get the BoxCollider attached to this GameObject
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("No BoxCollider found on the object! Please attach one.");
        }
    }

    void LateUpdate()
    {
        if (boxCollider == null || mainCamera == null) return;

        // Get the object's current position in world space
        Vector3 objectPosition = transform.position;

        // Get the bounds of the BoxCollider
        Vector3 colliderSize = boxCollider.size;
        Vector3 colliderExtent = colliderSize * 0.5f;

        // Get the corners of the collider in world space
        Vector3 minWorldPoint = objectPosition - colliderExtent;
        Vector3 maxWorldPoint = objectPosition + colliderExtent;

        // Convert the world bounds to viewport space (0 to 1, where 0.5 is the center of the screen)
        Vector3 minViewportPoint = mainCamera.WorldToViewportPoint(minWorldPoint);
        Vector3 maxViewportPoint = mainCamera.WorldToViewportPoint(maxWorldPoint);

        // Clamp the object's position to keep it inside the camera's view
        if (minViewportPoint.x < 0f)
        {
            objectPosition.x += (0f - minViewportPoint.x) * mainCamera.pixelWidth / mainCamera.pixelHeight;
        }
        else if (maxViewportPoint.x > 1f)
        {
            objectPosition.x -= (maxViewportPoint.x - 1f) * mainCamera.pixelWidth / mainCamera.pixelHeight;
        }

        if (minViewportPoint.y < 0f)
        {
            objectPosition.y += (0f - minViewportPoint.y) * mainCamera.pixelWidth / mainCamera.pixelHeight;
        }
        else if (maxViewportPoint.y > 1f)
        {
            objectPosition.y -= (maxViewportPoint.y - 1f) * mainCamera.pixelWidth / mainCamera.pixelHeight;
        }

        // Apply the clamped position
        transform.position = objectPosition;
    }
}
