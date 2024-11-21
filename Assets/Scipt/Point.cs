using UnityEngine;

public class Point
{
    public string Id { get; private set; }
    public Vector3 Position { get; private set; }
    public GameObject GameObject { get; private set; }

    public Point(string id, float x, float y, float z)
    {
        Id = id;
        Position = new Vector3(x, y, z);
    }

    // Creates a GameObject for the point in Unity
    public void CreateGameObject(GameObject parentObject)
    {
        GameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.name = $"Point_{Id}";
        GameObject.transform.position = Position;
        GameObject.transform.localScale = Vector3.one * 0.2f;  // Adjust the size of the sphere
   		GameObject.transform.SetParent(parentObject.transform);
    }

    public override bool Equals(object obj)
    {
        if (obj is Point other)
        {
            return Position == other.Position;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }
}