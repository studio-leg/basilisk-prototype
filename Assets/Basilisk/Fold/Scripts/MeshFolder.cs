using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshFolder : MonoBehaviour
{
    public MeshFilter baseMeshFilter;
    public MeshFold[] folds;
    public FoldableProp[] props;
    public bool doUpdateMesh = false;
    public bool doReset = false;

    private MeshFilter meshFilter;
    private Mesh meshBase;
    private Mesh meshLive;

    void Start()
    {
        Init();
    }
    
    void Update()
    {
        if (doUpdateMesh)
        {
            UpdateMesh();
        }
        if (doReset)
        {
            doReset = false;
            Init();
        }
    }

    private void Init()
    {
        folds = GetComponentsInChildren<MeshFold>();
        meshFilter = GetComponent<MeshFilter>();
        meshBase = baseMeshFilter.sharedMesh;
        meshLive = new Mesh();
        meshLive.vertices = meshBase.vertices;
        meshLive.triangles = meshBase.triangles;
        meshLive.normals = meshBase.normals;
        meshLive.uv = meshBase.uv;
        InitProps();
        InitMesh();
    }

    void InitProps()
    {
        for (int foldI = 0; foldI < folds.Length; foldI++)
        {
            var fold = folds[foldI];
            var propList = new List<FoldableProp>();
            for (int propI = 0; propI < props.Length; propI++)
            {
                var prop = props[propI];
                if (fold.ContainsPoint(prop.transform.position))
                {
                    propList.Add(prop);
                }
            }
            fold.props = propList.ToArray();
        }
    }

    void InitMesh()
    {
        var vertices = meshBase.vertices;
        for (int foldI = 0; foldI < folds.Length; foldI++)
        {
            var fold = folds[foldI];
            fold.Init();
            fold.InitMesh(ref vertices);
        }
    }

    void UpdateMesh()
    {
        if (meshBase)
        {
            var liveVertices = meshBase.vertices;
            for (int foldI = 0; foldI < folds.Length; foldI++)
            {
                var fold = folds[foldI];
                fold.FoldVertices(ref liveVertices);
            }
            meshLive.vertices = liveVertices;
            meshLive.RecalculateBounds();
            meshLive.RecalculateNormals();
            meshFilter.mesh = meshLive;

            // Fold Props
            for (int propI = 0; propI < props.Length; propI++)
            {
                var prop = props[propI];
                var position = prop.BasePosition;
                var transform = prop.transform;
                transform.position = position;
                transform.rotation = prop.BaseRotation;
            }
            for (int foldI = 0; foldI < folds.Length; foldI++)
            {
                var fold = folds[foldI];
                fold.FoldProps();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (meshBase)
        {
            var baseVertices = meshBase.vertices;
            for (int vertI = 0; vertI < baseVertices.Length; vertI++)
            {
                for (int foldI = 0; foldI < folds.Length; foldI++)
                {
                    var baseVertex = baseVertices[vertI];
                    var fold = folds[foldI];
                    if (fold.ContainsPoint(baseVertex))
                    {
                        Gizmos.color = fold.colour;
                        Gizmos.DrawSphere(baseVertex, 0.1f);
                    }
                }
            }
        }
    }
}