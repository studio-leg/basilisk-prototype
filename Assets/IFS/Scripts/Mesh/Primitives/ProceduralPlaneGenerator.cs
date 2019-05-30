using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class ProceduralPlaneGenerator : MonoBehaviour
{
    public bool regenerate = true;
    public float length = 1f;
    public float width = 1f;
    public int resX = 2;
    public int resZ = 2;

    void Start()
    {
    }

    void Update()
    {
        if (regenerate)
        {
            regenerate = false;
            var mesh = ProceduralPlane.Generate(length, width, resX, resZ);
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }
    }
}