using UnityEngine;
using System.Collections;

public class Deformer : InteractiveMesh
{
    public float timeScale = 10f;
    public float vertexScale = 0.001f;
    public float noiseScale = 0.9f;
    public bool useY = true;
    public bool drawMesh = false;
    public bool isBaseMeshDynamic = true;

    Mesh meshBase;
    Vector3[] baseVertices;
    Vector3[] baseNormals;

    Material material;
    SkinnedMeshRenderer skinnedMesh;
    MeshFilter meshFilter;
    bool isInited = false;


    override protected void Start()
    {
        base.Start();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        Init();
    }
    
    private void OnEnable()
    {
        Init();
    }

    /// <summary>
    /// TODO: optimise this, move to compute shader?
    /// </summary>
    override protected void Update()
    {
        base.Update();
        if (isBaseMeshDynamic) UpdateBaseMesh();
        var vertices = mesh.vertices;
        worldVertices = mesh.vertices;
        meshTriangles = mesh.triangles;
        int i = 0;
        float time = Time.time * timeScale;
        while (i < vertices.Length)
        {
            Vector3 noiseIn = baseVertices[i] * vertexScale;
            float noise;
            if (useY)
                noise = Mathf.PerlinNoise(noiseIn.x + time, noiseIn.y + time);
            else
                noise = Mathf.PerlinNoise(noiseIn.x + time, noiseIn.z + time);
            vertices[i] = baseVertices[i] + (baseNormals[i] * (noise * noiseScale));

            //worldVertices[i] = transform.localToWorldMatrix.MultiplyPoint(vertices[i]);
            worldVertices[i] = transform.localPosition + (transform.localRotation * vertices[i]);

            i++;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();


        if (drawMesh) Graphics.DrawMesh(mesh, transform.position, transform.rotation, material, 0);
    }


    void Init()
    {
        if (isInited) return;
        if (skinnedMesh)
        {
            // Base mesh
            meshBase = new Mesh();
            skinnedMesh.BakeMesh(meshBase);
            baseVertices = meshBase.vertices;
            baseNormals = meshBase.normals;
            // dynamic mesh
            mesh = new Mesh();
            skinnedMesh.BakeMesh(mesh);
            meshFilter.mesh = mesh;
            material = skinnedMesh.sharedMaterial;
            isInited = true;
        }
        else if (meshFilter)
        {
            meshBase = meshFilter.mesh;
            baseVertices = meshBase.vertices;
            baseNormals = meshBase.normals;
            mesh = meshFilter.mesh;
            isInited = true;
        }
    }

    void UpdateBaseMesh()
    {
        if (skinnedMesh)
        {
            skinnedMesh.BakeMesh(meshBase);
            baseVertices = meshBase.vertices;
            baseNormals = meshBase.normals;
        }
    }


}