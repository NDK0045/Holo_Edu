using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public Dictionary<Vector3, Point> uniquePoints = new Dictionary<Vector3, Point>();
    public List<Line> lines = new List<Line>();
    
    public Dictionary<Vector3, Point> GetPoints() { return uniquePoints; }
    public List<Line> GetLines() { return lines; }

    public void Clear()
    {
        uniquePoints.Clear();
        lines.Clear();
    }

    // Method to add a point if its position is unique
    public Point AddPoint(string id, float x, float y, float z, GameObject go)
    {
        Vector3 position = new Vector3(x, y, z);
        
        if (!uniquePoints.ContainsKey(position))
        {
            Point point = new Point(id, x, y, z);
            uniquePoints[position] = point;
            //point.CreateGameObject(go);  // Create and deploy point GameObject in Unity
            return point;
        }
        else
        {
            return uniquePoints[position];  // Return the existing point if position is identical
        }
    }

    // Method to add a line between two points
    public void AddLine(string id, Point startPoint, Point endPoint, GameObject go)
    {
        if (startPoint != null && endPoint != null && startPoint != endPoint)
        {
            Line line = new Line(id, startPoint, endPoint);
            lines.Add(line);
            //line.CreateGameObject(go);  // Create and deploy line GameObject in Unity
        }
    }
}