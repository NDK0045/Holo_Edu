using UnityEngine;

public class CameraDoubleTapZoom : MonoBehaviour
{
    public float zoomDistance = 10f; // Distance to move forward/backward
    public float zoomSpeed = 5f;     // Speed of the zoom movement

    private Vector3 originalPosition; // The starting position of the camera
    private Vector3 targetPosition;   // The position to zoom into
    private bool isZoomedIn = false;  // Whether the camera is zoomed in or not

    private float lastTapTime = 0f;   // Time of the last tap
    private float doubleTapThreshold = 0.3f; // Max time between taps to register as a double tap

    void Start()
    {
        // Store the original position of the camera
        originalPosition = transform.position;
        targetPosition = originalPosition;
    }

    void Update()
    {
        // Detect double-tap
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (Time.time - lastTapTime < doubleTapThreshold)
            {
                ToggleZoom();
            }
            lastTapTime = Time.time;
        }

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * zoomSpeed);
    }

    void ToggleZoom()
    {
        if (isZoomedIn)
        {
            // Zoom out
            targetPosition = originalPosition;
        }
        else
        {
            // Zoom in
            targetPosition = transform.position + transform.forward * zoomDistance;
        }

        // Toggle zoom state
        isZoomedIn = !isZoomedIn;
    }
}
