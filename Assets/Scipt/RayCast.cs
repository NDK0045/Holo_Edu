using UnityEngine;
using UnityEngine.InputSystem;

public class RayCast : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit Object: " + hit.collider.name);
                Debug.Log("Hit Position: " + hit.point);
            }
        }
    }
}