using UnityEngine;
using UnityEngine.UI;

public class ShapeRotationSlider : MonoBehaviour
{
    public Slider rotationSlider;       // Reference to the slider
    public GameObject targetObject;    // The parent object that contains the children
    private float previousSliderValue;  // To track the last slider value
    private bool isInitialized = false; // Flag to check if the object has been initialized

    [Range(0.1f, 10f)] public float sensitivityMultiplier = 1f; // Control rotation sensitivity

    void Start()
    {
        if (rotationSlider == null)
        {
            Debug.LogError("Slider is not assigned.");
            return;
        }

        rotationSlider.minValue = 0f;
        rotationSlider.maxValue = 1f;
        rotationSlider.onValueChanged.AddListener(OnSliderValueChanged);

        rotationSlider.value = 0f;
        previousSliderValue = 0f;

        StartCoroutine(InitializeTargetObject());
    }

    private System.Collections.IEnumerator InitializeTargetObject()
    {
        while (targetObject == null)
        {
            yield return null;
        }

        isInitialized = true;
    }

    private void OnSliderValueChanged(float value)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Slider value changed, but the target object is not initialized yet.");
            return;
        }

        if (targetObject == null)
        {
            Debug.LogError("Target object is null. Cannot rotate.");
            return;
        }

        // Calculate the change in slider value
        float delta = value - previousSliderValue;
        previousSliderValue = value;

        // Convert delta to degrees with sensitivity multiplier
        float deltaAngle = delta * 360f * sensitivityMultiplier;

        // Calculate the bounds center in world space
        Vector3 boundsCenter = CalculateBoundsCenter();

        // Rotate the object around the bounds center
        targetObject.transform.RotateAround(boundsCenter, Vector3.up, deltaAngle);

        Debug.Log($"Rotated around {boundsCenter} by {deltaAngle} degrees.");
    }

    private Vector3 CalculateBoundsCenter()
    {
        Transform[] childTransforms = targetObject.GetComponentsInChildren<Transform>();

        if (childTransforms.Length == 1)
        {
            Debug.LogWarning("Target object has no children. Defaulting to its position.");
            return targetObject.transform.position;
        }

        Vector3 totalPosition = Vector3.zero;
        int childCount = 0;

        foreach (Transform child in childTransforms)
        {
            if (child != targetObject.transform)
            {
                totalPosition += child.position;
                childCount++;
            }
        }

        if (childCount == 0)
        {
            Debug.LogWarning("No valid child objects found. Defaulting to target object's position.");
            return targetObject.transform.position;
        }

        return totalPosition / childCount;
    }

    void OnDestroy()
    {
        if (rotationSlider != null)
        {
            rotationSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
