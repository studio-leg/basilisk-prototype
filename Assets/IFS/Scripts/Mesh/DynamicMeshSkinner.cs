using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneVertexMatch
{
    public int boneIndex;
    public float distance;
    public float weight;
}

[ExecuteInEditMode]
public class DynamicMeshSkinner : MonoBehaviour
{
    [Header("Params")]
    public bool doInit = false;
    public bool doReset = false;
    public float maxDistance = 0.6f;
    public float fallOff = 0.1f;
    public int boneCount = 4;
    public Transform[] bones;
    public MeshFilter meshFilter;

    Mesh baseMesh;
    Mesh skinnedMeshBase;
    Mesh skinnedMeshLive;
    Vector3[] defaultBonePositions;
    SkinnedMeshRenderer skinnedMeshRenderer;
    BoneWeight[] weights;
    Vector3[] vertices;

    void Start()
    {
        Init();
    }

    void Init()
    {
        baseMesh = meshFilter.mesh;
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        var bonePositions = new List<Vector3>();
        foreach (Transform bone in bones)
        {
            bonePositions.Add(bone.position);
        }
        defaultBonePositions = bonePositions.ToArray();
        SkinMesh();
    }

    void SkinMesh()
    {
        int boneI = 0;
        foreach (Transform bone in bones)
        {
            if (bone.gameObject.activeSelf)
            {
                bone.position = defaultBonePositions[boneI];
                boneI++;
            }
        }

        // Create bindPoses
        // This is an array of matrices, one per bone
        // Each is the inverse of the transformation matrix of the bone it corresponds to
        var bindPoses = new Matrix4x4[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            var bone = bones[i];
            bindPoses[i] = bone.worldToLocalMatrix * transform.localToWorldMatrix;
        }

        // Cache the base mesh vertices for faster lookup
        vertices = baseMesh.vertices;

        // Calculate bone weights
        // This is done per vertex based on proximity to each bone
        // As such it will get slow for very large meshes
        weights = new BoneWeight[vertices.Length];
        for (int i = 0; i < weights.Length; i++)
        {
            // We're working in world space for proximity checks so need to get each vertex in world space
            var vertex = transform.localToWorldMatrix.MultiplyPoint(vertices[i]);
            // Create a list of BoneVertexMatch objects
            // and sort them in order of proximity of bone to vertex, nearest first
            var matches = new List<BoneVertexMatch>();
            for (int j = 0; j < bones.Length; j++)
            {
                var bone = bones[j];
                var distance = Vector3.Distance(vertex, bone.position);
                BoneVertexMatch match = new BoneVertexMatch();
                match.distance = distance;
                // Calculate weight based on distance
                // This is done by normalising based on the maxDistance
                // and then appying an exponential falloff so that near bones have a greater effect on vertices
                var weight = 1f;
                if (fallOff > 0)
                {
                    weight = Mathf.Clamp(distance, 0, maxDistance) / maxDistance;
                    weight = MathUtils.exponentialEasing(fallOff, distance);
                }
                else
                {
                    weight = 1 - (Mathf.Clamp(distance, 0, maxDistance) / maxDistance);
                }
                match.weight = weight;
                match.boneIndex = j;
                matches.Add(match);
            }
            matches.Sort(delegate (BoneVertexMatch a, BoneVertexMatch b) {
                return a.distance.CompareTo(b.distance);
            });

            // Skin based on proximities
            float weightsTotal = 0;
            float weightsTotalInv = 0;

            for (int matchI = 0; matchI < boneCount; matchI++)
            {
                weightsTotal += matches[matchI].weight;
            }
            for (int matchI = 0; matchI < boneCount; matchI++)
            {
                weightsTotalInv += (weightsTotal - matches[matchI].weight);
            }
            
            weights[i].boneIndex0 = matches[0].boneIndex;
            weights[i].weight0 = matches[0].weight / weightsTotal;
            if (boneCount > 1)
            {
                weights[i].boneIndex1 = matches[1].boneIndex;
                weights[i].weight1 = matches[1].weight / weightsTotal;
            }
            if (boneCount > 2)
            {
                weights[i].boneIndex2 = matches[2].boneIndex;
                weights[i].weight2 = matches[2].weight / weightsTotal;
            }
            if (boneCount > 3)
            {
                weights[i].boneIndex3 = matches[3].boneIndex;
                weights[i].weight3 = matches[3].weight / weightsTotal;
            }

            if (i == 0)
            {
                Debug.Log(string.Format("TOTAL : {0}", weightsTotal));
                Debug.Log(string.Format("TOTAL INV : {0}", weightsTotalInv));
                Debug.Log(string.Format("distances : {0}, {1}, {2}, {3}", matches[0].weight, matches[1].weight, matches[2].weight, matches[3].weight));
                Debug.Log(string.Format("normalise : {0}, {1}, {2}, {3}", weights[i].weight0, weights[i].weight1, weights[i].weight2, weights[i].weight3));
            }
        }

        skinnedMeshBase = new Mesh();
        skinnedMeshBase.vertices = vertices;
        skinnedMeshBase.uv = baseMesh.uv;
        skinnedMeshBase.triangles = baseMesh.triangles;
        skinnedMeshBase.normals = baseMesh.normals;
        skinnedMeshBase.RecalculateBounds();
        // add bones to mesh
        skinnedMeshBase.boneWeights = weights;
        skinnedMeshBase.bindposes = bindPoses;
        skinnedMeshRenderer.sharedMesh = skinnedMeshBase;
        skinnedMeshRenderer.bones = bones;
    }
    
    void Update()
    {
        if (doInit)
        {
            doInit = false;
            Init();
        }
        if (doReset)
        {
            doReset = false;
            SkinMesh();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (weights != null && skinnedMeshRenderer != null)
        {
            if (skinnedMeshLive == null)
            {
                skinnedMeshLive = new Mesh();
            }
            skinnedMeshRenderer.BakeMesh(skinnedMeshLive);
            var liveVertices = skinnedMeshLive.vertices;
            for (int i = 0; i < weights.Length; i++)
            {
                var vertex = transform.localToWorldMatrix.MultiplyPoint(liveVertices[i]);
                Gizmos.color = Color.yellow;
                var bone = bones[weights[i].boneIndex0];
                Gizmos.DrawLine(vertex, bone.position);
                Gizmos.color = Color.blue;
                bone = bones[weights[i].boneIndex1];
                Gizmos.DrawLine(vertex, bone.position);
                Gizmos.color = Color.green;
                bone = bones[weights[i].boneIndex2];
                Gizmos.DrawLine(vertex, bone.position);
                Gizmos.color = Color.red;
                bone = bones[weights[i].boneIndex3];
                Gizmos.DrawLine(vertex, bone.position);
            }

        }
    }

}