using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Linq;
using System.IO; 
using System.Linq;  

public class XMLHandler
{
    public void ReadXml(string str) { 
        var xml = new XmlDocument();
        xml.LoadXml(str);

        foreach(XmlNode node in xml.DocumentElement) {
            if(node.Name != "feature") continue;
            var type = node.Attributes["type"].Value;
            Debug.Log("Node is " + node);
            Debug.Log("Type is " + type);
        }
    }
    
    
    public static void ReadXML(string xmlContent, PointManager pointManager, GameObject go)
    {
        try
        {
            
            //string xmlContent = File.ReadAllText(filePath, System.Text.Encoding.UTF8).Trim();
			// Debug log to verify content (remove or comment out after testing)
            // Debug.Log("XML Content:\n" + xmlContent);

            var xml = new XmlDocument();
            try
            {
                // Load the XML string
                xml.LoadXml(xmlContent);

                // Iterate through the <feature> nodes
                foreach (XmlNode node in xml.DocumentElement.ChildNodes)
                {
                    if (node.Name == "feature" && node.Attributes != null)
                    {
                        string type = node.Attributes["type"]?.Value;
                        Debug.Log($"Node: {node.OuterXml}");
                        Debug.Log($"Type: {type}");
                    }
                }
            }
            catch (XmlException ex)
            {
                //Debug.LogError($"XML Parsing Error: {ex.Message}");
            }
            
            // Parse the XML content
            XDocument xmlDoc = XDocument.Parse(xmlContent);

            XElement detail = xmlDoc.Element("detail");
            if (detail != null)
            {
                // Parse each feature and its entities
                foreach (XElement feature in detail.Elements("feature"))
                {
                    XElement entities = feature.Element("entities");
                    if (entities != null)
                    {
                        // Iterate through each <entity> element
                        foreach (XElement xe in entities.Elements("entity"))
                        {
                            
                            
                            Debug.LogError($"This type: {xe}");
                            
                            
                            // Check if the "type" attribute is "PointEntity"
                            if (xe.Attribute("type")?.Value == "PointEntity")
                            {

                            }

                            
                            
                            else if (xe.Attribute("type")?.Value == "LineEntity")
                            {

                                foreach (XElement pEntity in xe.Elements("entity"))
                                {
                                    if (pEntity.Attribute("type")?.Value == "PointEntity")
                                    { 

                                        float px = float.Parse(pEntity.Attribute("x")?.Value ?? "0");
                                        float py = float.Parse(pEntity.Attribute("y")?.Value ?? "0");
                                        float pz = float.Parse(pEntity.Attribute("z")?.Value ?? "0");
                                        pointManager.AddPoint(pEntity.Attribute("id")?.Value, px, py, pz, go);
                                    }
                                }
                                XElement startPointElement = xe.Elements("entity").ElementAt(0);
                                XElement endPointElement = xe.Elements("entity").ElementAt(1);

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
                                    pointManager.AddLine(xe.Attribute("id")?.Value, startPoint, endPoint, go);
                                }
                            }
                            
                            
                            else if (xe.Attribute("type")?.Value == "ArcEntity")
                            {

                            }
                            else if (xe.Attribute("type")?.Value == "CircleEntity")
                            {

                            }
                            else if (xe.Attribute("type")?.Value == "SplineEntity")
                            {

                            }
                            else if (xe.Attribute("type")?.Value == "FunctionEntity")
                            {

                            }
                        }

                        /*foreach (XElement entity in entities.Elements("entity"))
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
                        }*/
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to read XML: " + e.Message);
        }
    }
    
    public static void ReadXML(PointManager pointManager, GameObject go)
    {
        try
        {
            
            //string xmlContent = File.ReadAllText(filePath, System.Text.Encoding.UTF8).Trim();
			string xmlContent = "<?xml version=\"1.0\" encoding=\"utf-16\"?><detail id=\"0\" viewPos=\"58.85891 10.10138 17.91676\" viewRot=\"-0.3425799 0.7354281 0.1256912 -0.5709522\" viewSize=\"16.00001\" activeFeature=\"2\">	<feature type=\"SketchFeature\" id=\"1\" solveParent=\"False\" alwaysHover=\"True\">		<entities>			<entity type=\"PointEntity\" id=\"1\" x=\"0\" y=\"0\" z=\"0\" />		</entities>	</feature>	<feature type=\"SketchFeature\" id=\"2\" solveParent=\"False\">		<entities>			<entity type=\"LineEntity\" id=\"1\">				<entity type=\"PointEntity\" id=\"2\" x=\"-5.72501468658447\" y=\"11.025444984436\" z=\"-1.3427734375\" />				<entity type=\"PointEntity\" id=\"3\" x=\"-18.1873969545622\" y=\"-2.83850513725079\" z=\"0.387234146230476\" />			</entity>			<entity type=\"LineEntity\" id=\"4\">				<entity type=\"PointEntity\" id=\"5\" x=\"-18.1873969545622\" y=\"-2.83850513725079\" z=\"0.387234146230476\" />				<entity type=\"PointEntity\" id=\"6\" x=\"-0.408093271305316\" y=\"-5.8148182617661\" z=\"0.640037351351109\" />			</entity>			<entity type=\"LineEntity\" id=\"8\">				<entity type=\"PointEntity\" id=\"9\" x=\"-0.408093271305316\" y=\"-5.8148182617661\" z=\"0.640037351351109\" />				<entity type=\"PointEntity\" id=\"A\" x=\"-5.72501468658447\" y=\"11.025444984436\" z=\"-1.3427734375\" />			</entity>			<entity type=\"LineEntity\" id=\"E\">				<entity type=\"PointEntity\" id=\"F\" x=\"-18.1873969545622\" y=\"-2.83850513725079\" z=\"0.387234146230476\" />				<entity type=\"PointEntity\" id=\"10\" x=\"-8.91705322265625\" y=\"0.272705078125\" z=\"-11.09765625\" />			</entity>			<entity type=\"LineEntity\" id=\"12\">				<entity type=\"PointEntity\" id=\"13\" x=\"-8.91705322265625\" y=\"0.272705078125\" z=\"-11.09765625\" />				<entity type=\"PointEntity\" id=\"14\" x=\"-5.72501468658447\" y=\"11.025444984436\" z=\"-1.3427734375\" />			</entity>			<entity type=\"LineEntity\" id=\"17\">				<entity type=\"PointEntity\" id=\"18\" x=\"-8.91705322265625\" y=\"0.272705078125\" z=\"-11.09765625\" />				<entity type=\"PointEntity\" id=\"19\" x=\"-0.408093271305316\" y=\"-5.8148182617661\" z=\"0.640037351351109\" />			</entity>			<entity type=\"LineEntity\" id=\"1C\">				<entity type=\"PointEntity\" id=\"1D\" x=\"-9.01540177194665\" y=\"-4.3739268909001\" z=\"0.517650377418415\" />				<entity type=\"PointEntity\" id=\"1E\" x=\"-5.72501468658447\" y=\"11.025444984436\" z=\"-1.3427734375\" />			</entity>			<entity type=\"LineEntity\" id=\"32\">				<entity type=\"PointEntity\" id=\"33\" x=\"-0.236328125\" y=\"3.8526611328125\" z=\"1.12744140625\" />				<entity type=\"PointEntity\" id=\"34\" x=\"-1.0400390625\" y=\"3.22607421875\" z=\"2.62744140625\" />			</entity>			<entity type=\"LineEntity\" id=\"3B\">				<entity type=\"PointEntity\" id=\"3C\" x=\"-1.07177734375\" y=\"2.501220703125\" z=\"-0.13232421875\" />				<entity type=\"PointEntity\" id=\"3D\" x=\"-0.4169921875\" y=\"0.818603515625\" z=\"0.03271484375\" />			</entity>			<entity type=\"ArcEntity\" id=\"48\">				<entity type=\"PointEntity\" id=\"49\" x=\"0.8330078125\" y=\"3.4581298828125\" z=\"2.646484375\" />				<entity type=\"PointEntity\" id=\"4A\" x=\"1.8603515625\" y=\"-3.8670654296875\" z=\"2.38818359375\" />				<entity type=\"PointEntity\" id=\"4B\" x=\"1.3466796875\" y=\"-0.2044677734375\" z=\"2.517333984375\" />			</entity>			<entity type=\"CircleEntity\" id=\"46\" r=\"5.58601951599121\">				<entity type=\"PointEntity\" id=\"47\" x=\"6.9580078125\" y=\"3.0797119140625\" z=\"-10.244140625\" />			</entity>			<entity type=\"FunctionEntity\" id=\"51\" x=\"t\" y=\"cos(t * pi)\" t0=\"0\" t1=\"1\" t0fix=\"False\" t1fix=\"False\" subdiv=\"16\" basis=\"1 0 0 1 0 0 \">				<entity type=\"PointEntity\" id=\"52\" x=\"2.1038818359375\" y=\"-1.3349609375\" z=\"-1.94677734375\" />				<entity type=\"PointEntity\" id=\"53\" x=\"-3.877685546875\" y=\"-4.625\" z=\"-9.2734375\" />				<entity type=\"PointEntity\" id=\"54\" x=\"2.1038818359375\" y=\"-1.3349609375\" z=\"-1.94677734375\" />			</entity>			<entity type=\"FunctionEntity\" id=\"55\" x=\"t\" y=\"cos(t * pi)\" t0=\"0\" t1=\"1\" t0fix=\"False\" t1fix=\"False\" subdiv=\"16\" basis=\"1 0 0 1 0 0 \">				<entity type=\"PointEntity\" id=\"56\" x=\"3.4390869140625\" y=\"1.0751953125\" z=\"2.73095703125\" />				<entity type=\"PointEntity\" id=\"57\" x=\"1.0361328125\" y=\"-2.6611328125\" z=\"-4.59521484375\" />				<entity type=\"PointEntity\" id=\"58\" x=\"3.4390869140625\" y=\"1.0751953125\" z=\"2.73095703125\" />			</entity>			<entity type=\"SplineEntity\" id=\"59\">				<entity type=\"PointEntity\" id=\"5A\" x=\"1.74609375\" y=\"3.08447265625\" z=\"-6.963623046875\" />				<entity type=\"PointEntity\" id=\"5B\" x=\"2.48291015625\" y=\"0.6490478515625\" z=\"-6.0374755859375\" />				<entity type=\"PointEntity\" id=\"5C\" x=\"3.10124492645264\" y=\"-0.412461638450623\" z=\"-6.27621412277222\" />				<entity type=\"PointEntity\" id=\"5D\" x=\"3.2197265625\" y=\"-1.786376953125\" z=\"-5.111328125\" />			</entity>			<entity type=\"SplineEntity\" id=\"5E\">				<entity type=\"PointEntity\" id=\"5F\" x=\"3.2197265625\" y=\"-1.786376953125\" z=\"-5.111328125\" />				<entity type=\"PointEntity\" id=\"60\" x=\"3.30918884277344\" y=\"-2.82378196716309\" z=\"-4.23175525665283\" />				<entity type=\"PointEntity\" id=\"61\" x=\"2.15059804916382\" y=\"-2.60715484619141\" z=\"-1.95084965229034\" />				<entity type=\"PointEntity\" id=\"62\" x=\"2.6025390625\" y=\"-3.8544921875\" z=\"-1.63818359375\" />			</entity>			<entity type=\"SplineEntity\" id=\"65\">				<entity type=\"PointEntity\" id=\"66\" x=\"2.6025390625\" y=\"-3.8544921875\" z=\"-1.63818359375\" />				<entity type=\"PointEntity\" id=\"67\" x=\"2.87516093254089\" y=\"-4.6069164276123\" z=\"-1.4495757818222\" />				<entity type=\"PointEntity\" id=\"68\" x=\"3.60605001449585\" y=\"-4.61251401901245\" z=\"-3.02427291870117\" />				<entity type=\"PointEntity\" id=\"69\" x=\"3.96875\" y=\"-5.35009765625\" z=\"-3.045654296875\" />			</entity>			<entity type=\"SplineEntity\" id=\"6C\">				<entity type=\"PointEntity\" id=\"6D\" x=\"3.96875\" y=\"-5.35009765625\" z=\"-3.045654296875\" />				<entity type=\"PointEntity\" id=\"6E\" x=\"4.80255889892578\" y=\"-7.04572439193726\" z=\"-3.09480762481689\" />				<entity type=\"PointEntity\" id=\"6F\" x=\"5.47925615310669\" y=\"-9.72034549713135\" z=\"-1.79055535793304\" />				<entity type=\"PointEntity\" id=\"70\" x=\"4.845703125\" y=\"-10.082275390625\" z=\"-0.046875\" />			</entity>			<entity type=\"SplineEntity\" id=\"73\">				<entity type=\"PointEntity\" id=\"74\" x=\"3.9248046875\" y=\"-10.103759765625\" z=\"1.964599609375\" />				<entity type=\"PointEntity\" id=\"75\" x=\"3.40117335319519\" y=\"-10.4028940200806\" z=\"3.40575122833252\" />				<entity type=\"PointEntity\" id=\"76\" x=\"1.91148293018341\" y=\"-8.69424438476563\" z=\"4.85896348953247\" />				<entity type=\"PointEntity\" id=\"77\" x=\"1.328125\" y=\"-7.261962890625\" z=\"4.637939453125\" />			</entity>		</entities>		<constraints>			<constraint type=\"PointsCoincident\" id=\"7\">				<link path=\"2/-1/3\" />				<link path=\"2/-1/5\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"B\">				<link path=\"2/-1/6\" />				<link path=\"2/-1/9\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"C\">				<link path=\"2/-1/A\" />				<link path=\"2/-1/2\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"11\">				<link path=\"2/-1/F\" />				<link path=\"2/-1/5\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"15\">				<link path=\"2/-1/10\" />				<link path=\"2/-1/13\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"16\">				<link path=\"2/-1/14\" />				<link path=\"2/-1/A\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"1A\">				<link path=\"2/-1/18\" />				<link path=\"2/-1/13\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"1B\">				<link path=\"2/-1/19\" />				<link path=\"2/-1/9\" />			</constraint>			<constraint type=\"PointOn\" id=\"1F\" x=\"-9.015402\" y=\"-4.373927\" z=\"0\" value=\"0.515880449876842\" reference=\"True\">				<link path=\"2/-1/1D\" />				<link path=\"2/-1/4\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"20\">				<link path=\"2/-1/1E\" />				<link path=\"2/-1/14\" />			</constraint>			<constraint type=\"Perpendicular\" chirality=\"LeftHand\" id=\"29\">				<link path=\"2/-1/1C\" />				<link path=\"2/-1/1C\" />			</constraint>			<constraint type=\"Perpendicular\" chirality=\"LeftHand\" id=\"2A\">				<link path=\"2/-1/4\" />				<link path=\"2/-1/4\" />			</constraint>			<constraint type=\"Perpendicular\" chirality=\"LeftHand\" id=\"2B\">				<link path=\"2/-1/1C\" />				<link path=\"2/-1/1C\" />			</constraint>			<constraint type=\"Parallel\" chirality=\"Antidirected\" id=\"43\">				<link path=\"2/-1/8\" />				<link path=\"2/-1/17\" />			</constraint>			<constraint type=\"Perpendicular\" chirality=\"LeftHand\" id=\"42\">				<link path=\"2/-1/E\" />				<link path=\"2/-1/17\" />			</constraint>			<constraint type=\"PointOn\" id=\"35\" x=\"-3.066554\" y=\"2.605313\" z=\"0\" value=\"0.5\" reference=\"True\">				<link path=\"2/-1/33\" />				<link path=\"2/-1/8\" />			</constraint>			<constraint type=\"PointOn\" id=\"36\" x=\"-7.370208\" y=\"3.325759\" z=\"0\" value=\"0.5\" reference=\"True\">				<link path=\"2/-1/34\" />				<link path=\"2/-1/1C\" />			</constraint>			<constraint type=\"Length\" id=\"44\" x=\"-6.468899\" y=\"5.396112\" z=\"0\" value=\"14.8650013183366\" reference=\"False\">				<link path=\"2/-1/12\" />			</constraint>			<constraint type=\"AngleConstraint\" id=\"45\" x=\"-8.702125\" y=\"-4.374695\" z=\"-0.00172446\" value=\"90\" reference=\"False\" supplementary=\"False\">				<link path=\"2/-1/1D\" />				<link path=\"2/-1/1E\" />				<link path=\"2/-1/5\" />				<link path=\"2/-1/6\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"63\">				<link path=\"2/-1/5D\" />				<link path=\"2/-1/5F\" />			</constraint>			<constraint type=\"Tangent\" chirality=\"Codirected\" id=\"64\" t0=\"0\" t1=\"1\">				<link path=\"2/-1/5E\" />				<link path=\"2/-1/59\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"6A\">				<link path=\"2/-1/62\" />				<link path=\"2/-1/66\" />			</constraint>			<constraint type=\"Tangent\" chirality=\"Codirected\" id=\"6B\" t0=\"0\" t1=\"1\">				<link path=\"2/-1/65\" />				<link path=\"2/-1/5E\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"71\">				<link path=\"2/-1/69\" />				<link path=\"2/-1/6D\" />			</constraint>			<constraint type=\"Tangent\" chirality=\"Codirected\" id=\"72\" t0=\"0\" t1=\"1\">				<link path=\"2/-1/6C\" />				<link path=\"2/-1/65\" />			</constraint>			<constraint type=\"PointsCoincident\" id=\"78\">				<link path=\"2/-1/70\" />				<link path=\"2/-1/74\" />			</constraint>			<constraint type=\"Tangent\" chirality=\"Codirected\" id=\"79\" t0=\"0\" t1=\"1\">				<link path=\"2/-1/73\" />				<link path=\"2/-1/6C\" />			</constraint>		</constraints>	</feature></detail>".Trim();
            // Debug log to verify content (remove or comment out after testing)
            // Debug.Log("XML Content:\n" + xmlContent);

            var xml = new XmlDocument();
            try
            {
                // Load the XML string
                xml.LoadXml(xmlContent);

                // Iterate through the <feature> nodes
                foreach (XmlNode node in xml.DocumentElement.ChildNodes)
                {
                    if (node.Name == "feature" && node.Attributes != null)
                    {
                        string type = node.Attributes["type"]?.Value;
                        Debug.Log($"Node: {node.OuterXml}");
                        Debug.Log($"Type: {type}");
                    }
                }
            }
            catch (XmlException ex)
            {
                //Debug.LogError($"XML Parsing Error: {ex.Message}");
            }
            
            // Parse the XML content
            XDocument xmlDoc = XDocument.Parse(xmlContent);

            XElement detail = xmlDoc.Element("detail");
            if (detail != null)
            {
                // Parse each feature and its entities
                foreach (XElement feature in detail.Elements("feature"))
                {
                    XElement entities = feature.Element("entities");
                    if (entities != null)
                    {
                        // Iterate through each <entity> element
                        foreach (XElement xe in entities.Elements("entity"))
                        {
                            
                            
                            Debug.LogError($"This type: {xe}");
                            
                            
                            // Check if the "type" attribute is "PointEntity"
                            if (xe.Attribute("type")?.Value == "PointEntity")
                            {

                            }

                            
                            
                            else if (xe.Attribute("type")?.Value == "LineEntity")
                            {

                                foreach (XElement pEntity in xe.Elements("entity"))
                                {
                                    if (pEntity.Attribute("type")?.Value == "PointEntity")
                                    { 

                                        float px = float.Parse(pEntity.Attribute("x")?.Value ?? "0");
                                        float py = float.Parse(pEntity.Attribute("y")?.Value ?? "0");
                                        float pz = float.Parse(pEntity.Attribute("z")?.Value ?? "0");
                                        pointManager.AddPoint(pEntity.Attribute("id")?.Value, px, py, pz, go);
                                    }
                                }
                                XElement startPointElement = xe.Elements("entity").ElementAt(0);
                                XElement endPointElement = xe.Elements("entity").ElementAt(1);

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
                                    pointManager.AddLine(xe.Attribute("id")?.Value, startPoint, endPoint, go);
                                }
                            }
                            
                            
                            else if (xe.Attribute("type")?.Value == "ArcEntity")
                            {

                            }
                            else if (xe.Attribute("type")?.Value == "CircleEntity")
                            {

                            }
                            else if (xe.Attribute("type")?.Value == "SplineEntity")
                            {

                            }
                            else if (xe.Attribute("type")?.Value == "FunctionEntity")
                            {

                            }
                        }

                        /*foreach (XElement entity in entities.Elements("entity"))
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
                        }*/
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