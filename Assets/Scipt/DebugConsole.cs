using UnityEngine;
using UnityEngine.UI; // For UI components
using TMPro; // For TextMeshPro components
using UnityEngine.InputSystem; // For the new Input System
using System.Collections.Generic;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] private GameObject consolePanel; // Panel for the Debug Console
    [SerializeField] private GameObject greetingPrompt; // Panel for the Greeting Prompt
    [SerializeField] private GameObject content; // Content GameObject under Viewport
    [SerializeField] private GameObject logTextPrefab; // Prefab for individual TMP log entries
    [SerializeField] private int maxLogHistory = 500; // Maximum log entries
    [SerializeField] private Button clearButton; // Clear button

    private Queue<GameObject> logEntries = new Queue<GameObject>();

    private void Awake()
    {
        // Assign the Clear button functionality
        if (clearButton != null)
        {
            clearButton.onClick.AddListener(ClearLogs);
        }

        // Subscribe to Unity log messages
        Application.logMessageReceived += HandleLog;

        // Initialize visibility based on the Greeting Prompt
        UpdateConsoleVisibility();
    }

    private void Start()
    {
        Debug.Log("Debug Console Initialized!");
    }

    private void OnDestroy()
    {
        // Unsubscribe from Unity log messages
        Application.logMessageReceived -= HandleLog;

        // Clean up any remaining log entries
        ClearLogs();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Space key pressed!");
        }

        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            ToggleConsoleVisibility();
        }

        UpdateConsoleVisibility();
    }

    private void UpdateConsoleVisibility()
    {
        if (greetingPrompt != null && consolePanel != null)
        {
            // Hide Debug Console if Greeting Prompt is visible
            consolePanel.SetActive(!greetingPrompt.activeSelf);
        }
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logTextPrefab == null || content == null)
        {
            Debug.LogWarning("Log Text Prefab or Content is missing.");
            return;
        }

        // Create a new TMP log entry
        GameObject logEntry = Instantiate(logTextPrefab, content.transform);
        TMP_Text logText = logEntry.GetComponent<TMP_Text>();

        if (logText != null)
        {
            logText.text = $"[{type}] {logString}";
        }

        // Add the log entry to the queue
        logEntries.Enqueue(logEntry);

        // Remove the oldest log if exceeding max history
        if (logEntries.Count > maxLogHistory)
        {
            GameObject oldestLog = logEntries.Dequeue();
            if (oldestLog != null)
            {
                Destroy(oldestLog);
            }
        }
    }

    public void ClearLogs()
    {
        // Clear all log entries from the Content
        foreach (GameObject logEntry in logEntries)
        {
            if (logEntry != null)
            {
                Destroy(logEntry);
            }
        }
        logEntries.Clear();
    }

    public void ToggleConsoleVisibility()
    {
        if (consolePanel != null)
        {
            consolePanel.SetActive(!consolePanel.activeSelf);
        }
    }
}
