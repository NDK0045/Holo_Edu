using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System.Xml;

public class WebSocketClient : MonoBehaviour
{
    public string serverAddress = "localhost";
    public string sendPort = "8000";
    public string receivePort = "8001";
    public GameObject go;

    private WebSocket wsSend;
    private WebSocket wsReceive;
    private bool isConnecting = false;
    private PointManager pointManager;
    
    private readonly Queue<string> messageQueue = new Queue<string>();
    private readonly object queueLock = new object();

    private void Start()
    {
        StartCoroutine(AttemptConnection());
        pointManager = gameObject.AddComponent<PointManager>();
        StartCoroutine(ProcessMessageQueue());
    }

    private void ClearAllObjects()
    {
        // Destroy all child objects of 'go'
        while (go.transform.childCount > 0)
        {
            DestroyImmediate(go.transform.GetChild(0).gameObject);
        }
        
        // Clear the point manager's tracking collections by creating a new instance
        DestroyImmediate(pointManager);
        pointManager = gameObject.AddComponent<PointManager>();
    }

    private IEnumerator ProcessMessageQueue()
    {
        while (true)
        {
            string message = null;
            lock (queueLock)
            {
                if (messageQueue.Count > 0)
                {
                    message = messageQueue.Dequeue();
                }
            }

            if (message != null)
            {
                ProcessXMLMessage(message);
            }

            yield return null;
        }
    }

    private void ProcessXMLMessage(string data)
    {
        try
        {
            string xmlContent = data.Trim();
            XDocument xmlDoc = XDocument.Parse(xmlContent);

            XElement detail = xmlDoc.Element("detail");
            if (detail != null)
            {
                // Clear existing objects before creating new ones
                ClearAllObjects();

                foreach (XElement feature in detail.Elements("feature"))
                {
                    XElement entities = feature.Element("entities");
                    if (entities != null)
                    {
                        foreach (XElement entity in entities.Elements("entity"))
                        {
                            string entityType = entity.Attribute("type")?.Value;
                            string entityId = entity.Attribute("id")?.Value;

                            if (entityType == "PointEntity")
                            {
                                float x = float.Parse(entity.Attribute("x")?.Value ?? "0");
                                float y = float.Parse(entity.Attribute("y")?.Value ?? "0");
                                float z = float.Parse(entity.Attribute("z")?.Value ?? "0");
                                pointManager.AddPoint(entityId, x, y, z, go);
                            }
                            else if (entityType == "LineEntity")
                            {
                                foreach (XElement pEntity in entity.Elements("entity"))
                                {
                                    float px = float.Parse(pEntity.Attribute("x")?.Value ?? "0");
                                    float py = float.Parse(pEntity.Attribute("y")?.Value ?? "0");
                                    float pz = float.Parse(pEntity.Attribute("z")?.Value ?? "0");
                                    pointManager.AddPoint(pEntity.Attribute("id")?.Value, px, py, pz, go);
                                }

                                XElement startPointElement = entity.Elements("entity").ElementAt(0);
                                XElement endPointElement = entity.Elements("entity").ElementAt(1);

                                if (startPointElement != null && endPointElement != null)
                                {
                                    string startId = startPointElement.Attribute("id")?.Value;
                                    float startX = float.Parse(startPointElement.Attribute("x")?.Value ?? "0");
                                    float startY = float.Parse(startPointElement.Attribute("y")?.Value ?? "0");
                                    float startZ = float.Parse(startPointElement.Attribute("z")?.Value ?? "0");

                                    string endId = endPointElement.Attribute("id")?.Value;
                                    float endX = float.Parse(endPointElement.Attribute("x")?.Value ?? "0");
                                    float endY = float.Parse(endPointElement.Attribute("y")?.Value ?? "0");
                                    float endZ = float.Parse(endPointElement.Attribute("z")?.Value ?? "0");

                                    Point startPoint = pointManager.AddPoint(startId, startX, startY, startZ, go);
                                    Point endPoint = pointManager.AddPoint(endId, endX, endY, endZ, go);
                                    pointManager.AddLine(entityId, startPoint, endPoint, go);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to process XML: " + e.Message);
        }
    }

    private IEnumerator AttemptConnection()
    {
        while (true)
        {
            if (!isConnecting)
            {
                isConnecting = true;

                wsSend = new WebSocket($"ws://{serverAddress}:{sendPort}/Send");
                wsReceive = new WebSocket($"ws://{serverAddress}:{receivePort}/Receive");

                wsSend.OnMessage += (sender, e) =>
                {
                    Debug.Log("[Send Channel] Received from server: " + e.Data);
                    lock (queueLock)
                    {
                        messageQueue.Enqueue(e.Data);
                    }
                };

                wsReceive.OnMessage += (sender, e) =>
                {
                    Debug.Log("[Receive Channel] Received from server: " + e.Data);
                };

                wsSend.OnError += (sender, e) =>
                {
                    Debug.LogError("[Send Channel] WebSocket Error: " + e.Message);
                };
                wsReceive.OnError += (sender, e) =>
                {
                    Debug.LogError("[Receive Channel] WebSocket Error: " + e.Message);
                };

                wsSend.ConnectAsync();
                wsReceive.ConnectAsync();

                yield return new WaitForSeconds(1);

                if (wsSend.IsAlive && wsReceive.IsAlive)
                {
                    Debug.Log("WebSocket Client connected to Send and Receive channels.");
                    break;
                }
                else
                {
                    Debug.LogWarning("WebSocket Client failed to connect, retrying...");
                    wsSend.Close();
                    wsReceive.Close();
                }

                isConnecting = false;
            }

            yield return new WaitForSeconds(3);
        }
    }

    public void SendString(string message)
    {
        if (wsSend != null && wsSend.IsAlive)
        {
            wsSend.Send(message);
            Debug.Log("Sent to server (send channel): " + message);
        }
        else
        {
            Debug.LogWarning("Send channel WebSocket is not connected.");
        }
    }

    private void OnDestroy()
    {
        wsSend?.Close();
        wsReceive?.Close();
        Debug.Log("WebSocket Client disconnected.");
    }
}