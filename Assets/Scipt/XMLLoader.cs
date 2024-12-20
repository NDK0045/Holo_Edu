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


    //
    public GameObject TargetObject
    {
        get { return go; }
    }

    //
    void Start()
    {
        pointManager = gameObject.AddComponent<PointManager>();
        ReadXML();
    }

    void ReadXML()
    {
        try
        {
            //string xmlContent = File.ReadAllText(filePath, System.Text.Encoding.UTF8).Trim();
			string xmlContent = "<?xml version=\"1.0\" encoding=\"utf-16\"?><detail id=\"0\" viewPos=\"72.1104 -4.141598 -2.223124\" viewRot=\"0.6004548 0.3718112 -0.6126265 -0.354823\" viewSize=\"10.24\" activeFeature=\"2\">	<feature type=\"SketchFeature\" id=\"1\" solveParent=\"False\" alwaysHover=\"True\">		<entities>			<entity type=\"PointEntity\" id=\"1\" x=\"0\" y=\"0\" z=\"0\" />		</entities>	</feature>	<feature type=\"SketchFeature\" id=\"2\" solveParent=\"False\">		<entities>			<entity type=\"LineEntity\" id=\"1\">				<entity type=\"PointEntity\" id=\"2\" x=\"-17.8831882476807\" y=\"-4.27139282226563\" z=\"0\" />				<entity type=\"PointEntity\" id=\"3\" x=\"-0.283550053834915\" y=\"-4.39364242553711\" z=\"0\" />			</entity>			<entity type=\"LineEntity\" id=\"4\">				<entity type=\"PointEntity\" id=\"5\" x=\"-0.283550053834915\" y=\"-4.39364242553711\" z=\"0\" />				<entity type=\"PointEntity\" id=\"6\" x=\"-0.161329045891762\" y=\"4.65281343460083\" z=\"0\" />			</entity>			<entity type=\"LineEntity\" id=\"8\">				<entity type=\"PointEntity\" id=\"9\" x=\"-0.161329045891762\" y=\"4.65281343460083\" z=\"0\" />				<entity type=\"PointEntity\" id=\"A\" x=\"-17.5165271759033\" y=\"4.65281343460083\" z=\"0\" />			</entity>			<entity type=\"LineEntity\" id=\"C\">				<entity type=\"PointEntity\" id=\"D\" x=\"-17.5165271759033\" y=\"4.65281343460083\" z=\"0\" />				<entity type=\"PointEntity\" id=\"E\" x=\"-17.8831882476807\" y=\"-4.27139282226563\" z=\"0\" />			</entity>			<entity type=\"LineEntity\" id=\"11\">				<entity type=\"PointEntity\" id=\"12\" x=\"-0.283550053834915\" y=\"-4.39364242553711\" z=\"0\" />				<entity type=\"PointEntity\" id=\"13\" x=\"1.599609375\" y=\"-3.166748046875\" z=\"-11.7283935546875\" />			</entity>			<entity type=\"LineEntity\" id=\"15\">				<entity type=\"PointEntity\" id=\"16\" x=\"1.599609375\" y=\"-3.166748046875\" z=\"-11.7283935546875\" />				<entity type=\"PointEntity\" id=\"17\" x=\"-15.0634765625\" y=\"-3.210693359375\" z=\"-14.599365234375\" />			</entity>			<entity type=\"LineEntity\" id=\"19\">				<entity type=\"PointEntity\" id=\"1A\" x=\"-15.0634765625\" y=\"-3.210693359375\" z=\"-14.599365234375\" />				<entity type=\"PointEntity\" id=\"1B\" x=\"-17.8831882476807\" y=\"-4.27139282226563\" z=\"0\" />			</entity>			<entity type=\"LineEntity\" id=\"1E\">				<entity type=\"PointEntity\" id=\"1F\" x=\"-15.0634765625\" y=\"-3.210693359375\" z=\"-14.599365234375\" />				<entity type=\"PointEntity\" id=\"20\" x=\"-16.021728515625\" y=\"5.698974609375\" z=\"-14.0525512695313\" />			</entity>			<entity type=\"LineEntity\" id=\"22\">				<entity type=\"PointEntity\" id=\"23\" x=\"-16.021728515625\" y=\"5.698974609375\" z=\"-14.0525512695313\" />				<entity type=\"PointEntity\" id=\"24\" x=\"-17.5165271759033\" y=\"4.65281343460083\" z=\"0\" />			</entity>			<entity type=\"LineEntity\" id=\"27\">				<entity type=\"PointEntity\" id=\"28\" x=\"-16.021728515625\" y=\"5.698974609375\" z=\"-14.0525512695313\" />				<entity type=\"PointEntity\" id=\"29\" x=\"4.086181640625\" y=\"4.65483093261719\" z=\"-12.7259826660156\" />			</entity>			<entity type=\"LineEntity\" id=\"2B\">				<entity type=\"PointEntity\" id=\"2C\" x=\"4.086181640625\" y=\"4.65483093261719\" z=\"-12.7259826660156\" />				<entity type=\"PointEntity\" id=\"2D\" x=\"1.599609375\" y=\"-3.166748046875\" z=\"-11.7283935546875\" />			</entity>			<entity type=\"LineEntity\" id=\"30\">				<entity type=\"PointEntity\" id=\"31\" x=\"4.086181640625\" y=\"4.65483093261719\" z=\"-12.7259826660156\" />				<entity type=\"PointEntity\" id=\"32\" x=\"-0.161329045891762\" y=\"4.65281343460083\" z=\"0\" />			</entity>		</entities>		<constraints>			<constraint type=\"PointsCoincident\" id=\"7\">				<link path=\"2/-1/3\" />				<link path=\"2/-1/5\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"B\">				<link path=\"2/-1/6\" />				<link path=\"2/-1/9\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"F\">				<link path=\"2/-1/A\" />				<link path=\"2/-1/D\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"10\">				<link path=\"2/-1/E\" />				<link path=\"2/-1/2\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"14\">				<link path=\"2/-1/12\" />				<link path=\"2/-1/5\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"18\">				<link path=\"2/-1/13\" />				<link path=\"2/-1/16\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"1C\">				<link path=\"2/-1/17\" />				<link path=\"2/-1/1A\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"1D\">				<link path=\"2/-1/1B\" />				<link path=\"2/-1/E\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"21\">				<link path=\"2/-1/1F\" />				<link path=\"2/-1/1A\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"25\">				<link path=\"2/-1/20\" />				<link path=\"2/-1/23\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"26\">				<link path=\"2/-1/24\" />				<link path=\"2/-1/D\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"2A\">				<link path=\"2/-1/28\" />				<link path=\"2/-1/23\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"2E\">				<link path=\"2/-1/29\" />				<link path=\"2/-1/2C\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"2F\">				<link path=\"2/-1/2D\" />				<link path=\"2/-1/16\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"33\">				<link path=\"2/-1/31\" />				<link path=\"2/-1/2C\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"34\">				<link path=\"2/-1/32\" />				<link path=\"2/-1/9\" />			</constraint>		</constraints>	</feature></detail>".Trim();
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
