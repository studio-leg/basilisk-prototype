using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralTorusGenerator : MonoBehaviour
{
    public bool regenerate = true;
    public float radius1 = 1f;
    public float radius2 = 0.3f;
    public int nbRadSeg = 24;
    public int nbSides = 18;

    void Start()
    {
    }

    void Update()
    {
        if (regenerate)
        {
            regenerate = false;
            var mesh = ProceduralTorus.Generate(radius1, radius2, nbRadSeg, nbSides);
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }
    }
}