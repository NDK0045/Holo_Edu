using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.IO; 
using System.Linq; // Optional, but useful for LINQ queries if needed
using System.Xml;

public class XMLLoader : MonoBehaviour
{
    public string filePath = "H:/DoAn/NoteCAD/HardShape.xml";
    public GameObject go;
    private PointManager pointManager;

    void Start()
    {
        pointManager = gameObject.AddComponent<PointManager>();
        ReadXML();
    }

    void ReadXML()
    {
        try
        {
            string xmlContent = File.ReadAllText(filePath, System.Text.Encoding.UTF8).Trim();

            // Debug log to verify content (remove or comment out after testing)
            // Debug.Log("XML Content:\n" + xmlContent);

            // Parse the XML content
            XDocument xmlDoc = XDocument.Parse(xmlContent);

            XElement detail = xmlDoc.Element("detail");
            if (detail != null)
            { 
                // Parse each feature and entity
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

                                Debug.Log((startPointElement==null) + " " + (endPointElement==null));
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

                                    // Add the two points to the manager and retrieve their instances
                                    Point startPoint = pointManager.AddPoint(startId, startX, startY, startZ, go);
                                    Point endPoint = pointManager.AddPoint(endId, endX, endY, endZ, go);

                                    // Add the line between the two points
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
            Debug.LogError("Failed to read XML: " + e.Message);
        }
    }
}
