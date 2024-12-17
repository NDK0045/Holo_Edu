using UnityEngine;

public class ObjectManipulation : MonoBehaviour
{
    private Vector2 _lastTouchPosition; // Track the last touch position for rotation
    private Vector2 _startPinchDistance; // Track the initial distance for scaling
    private Vector3 _startScale; // Track the initial scale of the object
    private bool _isScaling; // Track if the scaling gesture is active

    // Inertia for rotation
    private Vector3 _rotationVelocity = Vector3.zero; // Current rotational velocity (for inertia effect)
    public float _inertiaDamping = 2f; // How much the inertia slows down per frame
    public float _rotationSmoothness = 0.1f; // Rotation smoothness factor (lower is smoother)
    public float _rotationAcceleration = 0.1f; // Acceleration factor (higher means more sensitive to touch)

    private bool _isDragging; // To track mouse drag state
    private Vector2 _lastMousePosition; // Last mouse position for rotation

    void Update()
    {
        if (Input.touchCount == 1) // Single finger for rotation
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;

                // Rotate object based on finger movement
                float rotationSpeedX = -delta.x * _rotationSmoothness;
                float rotationSpeedY = delta.y * _rotationSmoothness;

                // Update rotation velocity (to simulate inertia)
                _rotationVelocity = new Vector3(rotationSpeedX, rotationSpeedY, 0);

                // Apply the rotation immediately
                transform.Rotate(Vector3.up, _rotationVelocity.x * _rotationAcceleration, Space.World);
                transform.Rotate(Vector3.right, _rotationVelocity.y * _rotationAcceleration, Space.World);
            }
        }
        else if (Input.touchCount == 2) // Two fingers for scaling
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                // Initialize scaling gesture
                _startPinchDistance = touch1.position - touch2.position;
                _startScale = transform.localScale;
                _isScaling = true;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                if (_isScaling)
                {
                    // Calculate the current distance between the two fingers
                    Vector2 currentDistance = touch1.position - touch2.position;
                    float scaleMultiplier = currentDistance.magnitude / _startPinchDistance.magnitude;

                    // Apply the scale multiplier to the object's initial scale
                    transform.localScale = _startScale * scaleMultiplier;

                    Debug.Log($"Scale up to {scaleMultiplier}");
                }
            }
            else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                // End scaling gesture
                _isScaling = false;
            }
        }
        else if (Input.GetMouseButtonDown(0)) // Left mouse button for rotation
        {
            _isDragging = true;
            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0)) // Rotate while dragging with the mouse
        {
            if (_isDragging)
            {
                Vector2 currentMousePosition = Input.mousePosition;
                Vector2 delta = currentMousePosition - _lastMousePosition;

                float rotationSpeedX = -delta.x * _rotationSmoothness;
                float rotationSpeedY = delta.y * _rotationSmoothness;

                // Update rotation velocity for inertia
                _rotationVelocity = new Vector3(rotationSpeedX, rotationSpeedY, 0);

                // Apply rotation
                transform.Rotate(Vector3.up, _rotationVelocity.x * _rotationAcceleration, Space.World);
                transform.Rotate(Vector3.right, _rotationVelocity.y * _rotationAcceleration, Space.World);

                _lastMousePosition = currentMousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0)) // Stop dragging on mouse release
        {
            _isDragging = false;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") != 0f) // Use mouse scroll for scaling
        {
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
            float scaleMultiplier = 1 + scrollAmount;

            transform.localScale *= scaleMultiplier;
        }

        // Apply inertia when touch or mouse ends
        if (Input.touchCount == 0 && !_isDragging)
        {
            // Gradually decrease the rotation velocity (simulate inertia)
            if (_rotationVelocity.magnitude > 0.01f) // If there's enough velocity to continue inertia
            {
                // Apply the inertia
                transform.Rotate(Vector3.up, _rotationVelocity.x * _rotationAcceleration, Space.World);
                transform.Rotate(Vector3.right, _rotationVelocity.y * _rotationAcceleration, Space.World);

                // Dampen the velocity to simulate friction (reduce speed over time)
                _rotationVelocity *= _inertiaDamping;
            }
            else
            {
                // Stop rotation if velocity is too small
                _rotationVelocity = Vector3.zero;
            }
        }
    }
}
