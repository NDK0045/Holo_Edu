using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements like buttons
using WebSocketSharp; // Namespace for WebSocket
using WebSocketSharp.Server; // Optional if using server-side WebSocketSharp
using System.Xml.Linq;
using System.IO; 
using System.Linq; // Optional, but useful for LINQ queries if needed
using System.Xml;
using System.Reflection;
using TMPro;


public class WebSocketClient : MonoBehaviour
{
	public string msg = "";
    public string serverAddress = "127.0.0.1";
    public int sendPort = 8080;
    public int receivePort = 8081;

    public Button retryButton; // Assign this button in the Unity Inspector
	public TMP_InputField tmpInputField;
    public GameObject go;

    private WebSocket wsSend;
    private WebSocket wsReceive;
    private readonly object queueLock = new object();
    private readonly ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
    private bool isConnecting = false;
    private PointManager pointManager;
    public GameObject slider;

    public BoxCollider targetBoxCollider;
    public ARMeshRenderer arMeshRenderer;

    void Start()
    {
		pointManager = gameObject.AddComponent<PointManager>();
        if (retryButton != null)
        {
            // Disable the retry button initially
            retryButton.interactable = false;

            // Assign the retry method to the button's onClick event

			tmpInputField.text = serverAddress;
            retryButton.onClick.AddListener(() => StartCoroutine(AttemptConnection()));
        }

        // Start the initial connection attempt
        StartCoroutine(AttemptConnection());
    }

    void Update()
    {
			ResizeMesh(); 
        // Process incoming messages in the Unity main thread
        while (messageQueue.TryDequeue(out string message))
        {
            Debug.Log($"Processing message: {message}");
            // Handle the message as needed
			msg = message.Trim();


			pointManager.Clear();
			ClearAllObjects();
   			XMLHandler.ReadXML(message.Trim(), pointManager, go);
			//arMeshRenderer.ClearExistingMesh();
			Mesh();
			//TriggerAll();

        }
    }

	void Mesh()
    {
        if (arMeshRenderer != null)
        {
            arMeshRenderer.InitializeMeshData(pointManager.GetPoints(), pointManager.GetLines());
            arMeshRenderer.RespawnMesh(go); // Use the parent transform for spawning
        }
        else
        {
            Debug.LogError("ARMeshRenderer is not assigned!");
        }
    }

public void ResizeMesh()
{
    if (go == null || targetBoxCollider == null)
    {
        //Debug.LogError("Parent object or Target BoxCollider is not assigned.");
        return;
    }

    Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
    if (renderers.Length == 0)
    {
        //Debug.LogError("No renderers found in the parent or its children.");
        return;
    }

    Bounds combinedBounds = renderers[0].bounds;
    foreach (var renderer in renderers)
    {
        combinedBounds.Encapsulate(renderer.bounds);
    }

    Bounds targetBounds = targetBoxCollider.bounds;

    Vector3 combinedSize = combinedBounds.size;
    Vector3 targetSize = targetBounds.size;

    float scaleX = targetSize.x / combinedSize.x;
    float scaleY = targetSize.y / combinedSize.y;
    float scaleZ = targetSize.z / combinedSize.z;

    float uniformScale = Mathf.Min(scaleX, scaleY, scaleZ);

    go.transform.localScale *= uniformScale;
    go.transform.position = targetBoxCollider.bounds.center;
}

	private void ClearAllObjects()
    {
		if (go.transform.childCount > 0)
    	{
        	// Loop through each child of the current GameObject
        	foreach (Transform child in go.transform)
        	{
            	Debug.Log("[CLEANER] Destroying child: " + child.gameObject.name);
            	Destroy(child.gameObject); // Destroy the child GameObject
        	}
    	}
    	else
  	  	{
        	Debug.Log("[CLEANER] No child objects to clear.");
    	}
    }

	private void TriggerAll()
    {
        MonoBehaviour[] scripts = slider.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != null)
            {
                MethodInfo methodInfo = script.GetType().GetMethod("trigger");
                if (methodInfo != null)
                {
                    methodInfo.Invoke(script, new object[] { go });
                }
            }
        }
    }


    


    private IEnumerator AttemptConnection()
    {
        if (isConnecting) yield break;

        isConnecting = true;

		if (tmpInputField != null)
        {
            serverAddress = tmpInputField.text;
		}

        Debug.Log($"Attempting to connect to WebSocket at {serverAddress}...");

        retryButton.interactable = false; // Disable the button during connection attempts

        yield return AttemptConnectionAsync();

        if (wsSend != null && wsSend.IsAlive && wsReceive != null && wsReceive.IsAlive)
        {
            Debug.Log($"WebSocket Client successfully connected at {serverAddress}!");
        }
        else
        {
            Debug.LogWarning($"WebSocket connection failed at {serverAddress}.");
            retryButton.interactable = true; // Enable the button if connection fails
        }

        isConnecting = false;
    }

    private async Task AttemptConnectionAsync()
    {
        try
        {
            // Initialize WebSocket connections
            wsSend = new WebSocket($"ws://{serverAddress}:{sendPort}/Send");
            wsReceive = new WebSocket($"ws://{serverAddress}:{receivePort}/Receive");

            // Setup event handlers
            wsSend.OnMessage += (sender, e) =>
            {
                Debug.Log("[Send Channel] Message received: " + e.Data);
                lock (queueLock)
                {
                    messageQueue.Enqueue(e.Data);
                }
            };
            wsReceive.OnMessage += (sender, e) =>
            {
                Debug.Log("[Receive Channel] Message received: " + e.Data);
            };

            wsSend.OnError += (sender, e) =>
            {
                Debug.LogError("[Send Channel] Error: " + e.Message);
            };
            wsReceive.OnError += (sender, e) =>
            {
                Debug.LogError("[Receive Channel] Error: " + e.Message);
            };

            // Connect asynchronously
            await Task.Run(() =>
            {
                Debug.Log("Connecting to Send WebSocket...");
                wsSend.Connect();

                Debug.Log("Connecting to Receive WebSocket...");
                wsReceive.Connect();
            });

            // Confirm connection state
            Debug.Log($"Send WebSocket IsAlive: {wsSend.IsAlive}");
            Debug.Log($"Receive WebSocket IsAlive: {wsReceive.IsAlive}");
        }
        catch (Exception ex)
        {
            Debug.LogError("WebSocket connection error: " + ex.Message);
        }
    }

    private void OnApplicationQuit()
    {
        // Ensure connections are closed properly
        Debug.Log("Closing WebSocket connections...");
        wsSend?.Close();
        wsReceive?.Close();
    }
}
