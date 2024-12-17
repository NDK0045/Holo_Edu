using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour
{
	

    [Header("Navigation Buttons")]
    public Button homeButton;
    public Button viewButton;
    public Button configButton;

    [Header("Panels")]
    public GameObject homePanel;
    public GameObject configPanel; 
	public GameObject grid;

    [Header("Navigator UI Settings")]
    public RectTransform navigatorRect;
    public float navigatorHeight = 100f; // Default height

    private void Start()
    {
        // Ensure navigation setup
        SetupNavigation();

        // Set bottom responsive layout
        SetupBottomResponsiveLayout();

    }

    private void SetupNavigation()
    {
        homeButton.onClick.AddListener(() => NavigateToHome());
        viewButton.onClick.AddListener(() => NavigateToView());
        configButton.onClick.AddListener(() => NavigateToConfig());

        // Initial state
        NavigateToHome();
    }

    private void SetupBottomResponsiveLayout()
    {
        if (navigatorRect == null)
        {
            Debug.LogError("Navigator RectTransform not assigned!");
            return;
        }

        // Anchor to bottom of screen
        navigatorRect.anchorMin = new Vector2(0, 0);
        navigatorRect.anchorMax = new Vector2(1, 0);

        // Pin to bottom with fixed height
        navigatorRect.anchoredPosition = new Vector2(0, navigatorHeight / 2);
        navigatorRect.sizeDelta = new Vector2(0, navigatorHeight);
    }

    private void Update()
    {
        // Optional: Adjust for different screen orientations or dynamic resizing
        AdjustForScreenSize();
    }

    private void AdjustForScreenSize()
    {
        // Responsive adjustment based on screen size
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Example: Adjust navigator height based on screen size
        navigatorHeight = Mathf.Clamp(screenHeight * 0.1f, 80f, 120f);
        navigatorRect.sizeDelta = new Vector2(screenWidth, navigatorHeight);
    }

    public void NavigateToHome()
    {
		grid.SetActive(false);
        homePanel.SetActive(true);
        configPanel.SetActive(false); 

    }

    public void NavigateToView()
    {
        homePanel.SetActive(false);
        configPanel.SetActive(false); 
		grid.SetActive(true);

        
    }

    public void NavigateToConfig()
    {
		grid.SetActive(false);
        homePanel.SetActive(false); 
        configPanel.SetActive(true);

    }
}