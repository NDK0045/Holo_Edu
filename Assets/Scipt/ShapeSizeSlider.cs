using UnityEngine;
using UnityEngine.UI;

public class ShapeSizeSlider : MonoBehaviour
{
    public Slider sizeSlider;           // Reference to the slider
    public XMLLoader xmlLoader;         // Reference to the XMLLoader
    private GameObject targetObject;    // The shape being scaled
    private Vector3 initialScale;       // The shape's initial scale
    private bool isInitialized = false; // Flag to check if the object has been initialized
    //private Rigidbody rb;

    void Start()
    {
        // Validate inputs
        if (sizeSlider == null || xmlLoader == null)
        {
            Debug.LogError("Slider or XMLLoader is not assigned.");
            return;
        }

        // Add a listener to the slider
        sizeSlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Optionally set the slider's default value to "1" (original scale)
        sizeSlider.value = 0.01f;

        // Wait for the object to load before initializing
        StartCoroutine(InitializeTargetObject());

        //rb = targetObject.GetComponent<Rigidbody>();
    }

    private System.Collections.IEnumerator InitializeTargetObject()
    {
        // Wait for XMLLoader to finish loading
        while (xmlLoader.TargetObject == null)
        {
            yield return null; // Wait for the next frame
        }

        // Once loaded, assign the target object and record its initial scale
        targetObject = xmlLoader.TargetObject;
        if (targetObject != null)
        {
            // Use the current scale of the object as the initial scale
            initialScale = targetObject.transform.localScale;
            isInitialized = true;
        }
        else
        {
            Debug.LogError("Target object is not set in XMLLoader.");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        // Scale the object only after initialization
        if (isInitialized && targetObject != null)
        {
            // Dynamically scale the object relative to its current initial scale
            targetObject.transform.localScale = initialScale * value;

            //targetObject.transform.RotateAround(targetObject.transform.TransformPoint(targetObject.GetComponent<BoxCollider>().center), Vector3.up, value);
        }
    }

    void OnDestroy()
    {
        // Remove the listener to prevent memory leaks
        if (sizeSlider != null)
        {
            sizeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
