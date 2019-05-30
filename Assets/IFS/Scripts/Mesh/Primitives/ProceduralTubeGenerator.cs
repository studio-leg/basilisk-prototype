using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralTubeGenerator : MonoBehaviour
{
    public bool regenerate = true;
    public float radius = 1f;
    public float length = 1f;
    public int radiusRes = 24;
    public int lengthRes = 24;
    public bool debugDraw = false;
    Mesh mesh;

    void Start()
    {
    }

    void Update()
    {
        if (regenerate)
        {
            regenerate = false;
            mesh = ProceduralTube.Generate(radius, length, radiusRes, lengthRes, false);
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }
    }
    

    void OnDrawGizmosSelected()
    {
        if (mesh && debugDraw)
        {
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            var tangents = mesh.tangents;
            for (int i = 0; i < vertices.Length && i < normals.Length; i++)
            {
                DrawArrow.ForGizmo(vertices[i], normals[i]);
            }

        }
    }
}