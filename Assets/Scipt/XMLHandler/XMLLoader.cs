using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.IO; 
using System.Linq; // Optional, but useful for LINQ queries if needed
using System.Xml;

public class XMLLoader : MonoBehaviour
{
    public string filePath = "D:\\SCHOLL_STUFFS\\NCKH\\NoteCADFile.xml.xml";
    public GameObject go;
    public BoxCollider targetBoxCollider;
    private PointManager pointManager;
    public ARMeshRenderer arMeshRenderer;


    //
    public GameObject TargetObject
    {
        get { return go; }
    }

    //
    void Start()
    {
        pointManager = gameObject.AddComponent<PointManager>();
        //ReadXML();
        //ResizeMesh();

		XMLHandler.ReadXML(pointManager, go);
        Mesh();
		ResizeMesh();
    }


	void Mesh()
    {
        if (arMeshRenderer != null)
        {
            arMeshRenderer.InitializeMeshData(pointManager.GetPoints(), pointManager.GetLines());
            arMeshRenderer.RespawnMesh(go.transform); // Use the parent transform for spawning
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
            Debug.LogError("Parent object or Target BoxCollider is not assigned.");
            return;
        }

        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogError("No renderers found in the parent or its children.");
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

}
