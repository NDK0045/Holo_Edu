using UnityEngine;

public class RayCast : MonoBehaviour
{
    private Camera mainCamera;

    public GameObject go;  // The object to move
    public GameObject go2; // The second object to move

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check if there is at least one touch input
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Check if the touch is in the "began" phase (similar to mouse button press)
            if (touch.phase == TouchPhase.Began)
            {
                // Create a ray from the touch position
                Ray ray = mainCamera.ScreenPointToRay(touch.position);

                // Perform the raycast
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Hit Object: " + hit.collider.name);
                    Debug.Log("Hit Position: " + hit.point);

                    // Move the GameObjects to the hit position
                    go.transform.position = hit.point;
                    go2.transform.position = hit.point;
                }
            }
        }
    }
}
