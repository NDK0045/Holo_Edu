using UnityEngine;
using UnityEngine.UI;

public class ShapeSizeSlider : MonoBehaviour
{
    public Slider sizeSlider;           // Reference to the slider
    public float alteredValue = 1;
    //public XMLLoader xmlLoader;         // Reference to the XMLLoader
    public GameObject targetObject;    // The shape being scaled
    private Vector3 initialScale;       // The shape's initial scale
    private bool isInitialized = false; // Flag to check if the object has been initialized
	public BoxCollider box;

    void Start()
    {
        // Validate inputs
        if (sizeSlider == null)
        {
            Debug.LogError("sizeSlider is not assigned in the Inspector.");
            return;
        }

        if (targetObject == null)
        {
            Debug.LogError("targetObject is not assigned in the Inspector.");
            return;
        }

        Debug.Log("Initialization successful!");

        // Set the range of the slider to allow scaling up and down
        sizeSlider.minValue = 0.1f; // Scale down to half the size
        sizeSlider.maxValue = 2f;  // Scale up to twice the size
        sizeSlider.value = 1f;     // Start at the original size

        // Add a listener to the slider
        sizeSlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Wait for the object to load before initializing
        StartCoroutine(InitializeTargetObject());
    }
    public void trigger(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError("Target object is null. Cannot rotate.");
            return;
        }

        Debug.Log(this + " " + alteredValue);
        //targetObject.transform.localScale = initialScale * alteredValue;
        sizeSlider.value = 1f;     // Start at the original size
    }

    private System.Collections.IEnumerator InitializeTargetObject()
    {
        Debug.Log("Waiting for targetObject to load the target object...");

        // Wait for targetObject to finish loading
        while (targetObject == null)
        {
            yield return null; // Wait for the next frame
        }

        Debug.Log("targetObject finished loading. Assigning target object...");

        // Once loaded, assign the target object and record its initial scale
        //targetObject = xmlLoader.TargetObject;

        if (targetObject != null)
        {
            initialScale = box.transform.localScale;
            isInitialized = true;
            Debug.Log($"Target object initialized. Initial scale: {initialScale}");
        }
        else
        {
            Debug.LogError("targetObject.TargetObject is null after loading.");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        // Scale the object only after initialization
        if (!isInitialized)
        {
            Debug.LogWarning("Slider value changed, but the target object is not initialized yet.");
            return;
        }

        if (targetObject == null)
        {
            Debug.LogError("Target object is null. Cannot scale.");
            return;
        }

        alteredValue = value;
        // Dynamically scale the object relative to its current initial scale
        targetObject.transform.localScale = targetObject.transform.localScale * value;
		sizeSlider.value = 1f; 
        Debug.Log($"Scale value {value}");
        //Debug.Log($"Scaled target object to {targetObject.transform.localScale}");
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