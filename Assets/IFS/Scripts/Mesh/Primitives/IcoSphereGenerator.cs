using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IcoSphereGenerator : MonoBehaviour
{
    public bool regenerate = true;
    public int detail = 3;
    public float radius = 1f;
    
    void Start()
    {
        regenerate = true;
    }
    
    void Update()
    {
        if (regenerate)
        {
            regenerate = false;
            var mesh = IcoSphere.GetMesh(detail, radius);
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }
    }
}