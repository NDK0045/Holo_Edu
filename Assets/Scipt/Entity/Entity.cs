using UnityEngine;

public abstract class Entity
{
    public string Id { get; private set; }
    public Vector3 Position { get; protected set; }
    public GameObject GameObject { get; protected set; }

    public Entity self { get; private set; }
    public Entity(string id, Vector3 position)
    {
        Id = id;
        Position = position;
        self = this;
    }

    // Abstract method to create the GameObject specific to the entity type
    public abstract void CreateGameObject(GameObject parentObject);

    // Abstract method to update the GameObject's position
    public virtual void UpdatePosition(Vector3 newPosition)
    {
        Position = newPosition;
        if (GameObject != null)
        {
            GameObject.transform.position = Position;
        }
    }
}