using UnityEngine;
using UnityEngine.UI;

public class ShapeRotationSlider : MonoBehaviour
{
    public Slider rotationSlider;        // Reference to the slider
    public GameObject targetObject;     // The parent object that contains the children
    private bool isInitialized = false;  // Flag to check if the object has been initialized
    //private Rigidbody rb;
    void Start()
    {
        // Validate inputs
        if (rotationSlider == null)
        {
            Debug.LogError("Slider is not assigned.");
            return;
        }

        // Set the slider's min and max values
        rotationSlider.minValue = 0f;     // Min value for the slider (0 degrees)
        rotationSlider.maxValue = 1f;     // Max value for the slider (1 corresponds to 360 degrees)

        // Add a listener to the slider
        rotationSlider.onValueChanged.AddListener(OnSliderValueChanged);

        rotationSlider.value = 0f;

        // Initialize target object
        StartCoroutine(InitializeTargetObject());
        //rb = targetObject.GetComponent<Rigidbody>();
    }

    private System.Collections.IEnumerator InitializeTargetObject()
    {
        // Wait until targetObject is assigned
        while (targetObject == null)
        {
            yield return null; // Wait for the next frame
        }

        // Once loaded, assign the target object
        if (targetObject != null)
        {
            isInitialized = true;
        }
        else
        {
            Debug.LogError("Target object is not assigned.");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        // Apply the rotation only after initialization
        if (isInitialized && targetObject != null)
        {
            // Convert the slider value to an angle (0 -> 0° and 1 -> 360°)
            float angle = value * 360f;

            // Rotate the parent object around the world Y-axis
            //targetObject.transform.Rotate(0f, angle, 0f, Space.World);
            //targetObject.transform.Rotate=Quaternion.Euler(0f, angle, 0f);
            targetObject.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

    }

    void OnDestroy()
    {
        // Remove the listener to prevent memory leaks
        if (rotationSlider != null)
        {
            rotationSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
