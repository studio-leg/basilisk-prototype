using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveMesh : MonoBehaviour
{
    public Collider boundsCollider;
    public bool debugDraw = false;
    public int debugTriangleDrawIndex = 0;

    protected Mesh mesh;
    protected Vector3[] worldVertices;
    protected Vector3[] worldNormals;
    protected Vector3[] worldVerticesInBounds;
    protected Vector3[] worldTrianglePointsInBounds;
    protected Vector3[] worldTriangleNormalsInBounds;
    protected int[] meshTriangles;
    protected int[] trianglesInBounds;
    protected bool isUpdateRequired = false;

    virtual protected void Start()
    {
        boundsCollider = GetComponent<Collider>();
    }

    virtual protected void Update()
    {
        if (isUpdateRequired && worldVertices != null)
        {
            int vertexIndex = 0;
            if (trianglesInBounds == null) PopulateTrianglesInBounds();
            for (int i = 0; i < trianglesInBounds.Length; i+=3)
            {
                int index = trianglesInBounds[i];
                worldVerticesInBounds[vertexIndex] = worldVertices[index];
                worldTrianglePointsInBounds[i] = worldVertices[index];
                worldTriangleNormalsInBounds[i] = worldNormals[index];

                index = trianglesInBounds[i + 1];
                worldTrianglePointsInBounds[i + 1] = worldVertices[index];
                worldTriangleNormalsInBounds[i + 1] = worldNormals[index];

                index = trianglesInBounds[i + 2];
                worldTrianglePointsInBounds[i + 2] = worldVertices[index];
                worldTriangleNormalsInBounds[i + 2] = worldNormals[index];

                vertexIndex++;
            }
            isUpdateRequired = false;
        }
    }

    public virtual Mesh GetMesh()
    {
        return mesh;
    }
    
    /// <summary>
    /// Returns mesh vertices in world space
    /// </summary>
    /// <returns></returns>
    public virtual Vector3[] GetWorldSpaceVertices()
    {
        if (worldVertices != null)
            return worldVertices;
        else
            return new Vector3[1];
    }

    /// <summary>
    /// Returns mesh vertices in world space that are within the bounds defined by the attached collider
    /// </summary>
    /// <returns></returns>
    virtual public Vector3[] GetWorldSpaceVerticesInBounds()
    {
        UpdateWorldSpaceVerticesInBounds();
        if (worldVerticesInBounds != null)
            return worldVerticesInBounds;
        else
            return new Vector3[1];
    }

    virtual public Vector3[] GetWorldSpaceTrianglePointsInBounds()
    {
        UpdateWorldSpaceVerticesInBounds();
        if (worldTrianglePointsInBounds != null)
            return worldTrianglePointsInBounds;
        else
            return new Vector3[1];
    }

    virtual public Vector3[] GetWorldSpaceTriangleNormalsInBounds()
    {
        UpdateWorldSpaceVerticesInBounds();
        if (worldTriangleNormalsInBounds != null)
            return worldTriangleNormalsInBounds;
        else
            return new Vector3[1];
    }

    /// <summary>
    /// TODO: populate indexes instead and resample live vertices each time? Or bounds check every frame? In compute shader?
    /// </summary>
    virtual public void UpdateWorldSpaceVerticesInBounds()
    {
        isUpdateRequired = true;
    }

    virtual public void PopulateTrianglesInBounds()
    {
        if (meshTriangles != null)
        {
            var triangles = new List<int>();
            for (int i = 0; i < meshTriangles.Length; i+=3)
            {
                int index = meshTriangles[i];
                var worldVertex = worldVertices[index];
                if (boundsCollider == null || boundsCollider.bounds.Contains(worldVertex))
                {
                    triangles.Add(meshTriangles[i]);
                    triangles.Add(meshTriangles[i+1]);
                    triangles.Add(meshTriangles[i+2]);
                }
            }
            trianglesInBounds = triangles.ToArray();
            worldVerticesInBounds = new Vector3[triangles.Count/3];
            worldTrianglePointsInBounds = new Vector3[triangles.Count];
            worldTriangleNormalsInBounds = new Vector3[triangles.Count];
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (debugDraw && worldVerticesInBounds != null)
        {
            for (int i = 0; i < worldVerticesInBounds.Length; i++)
            {
                //Gizmos.DrawCube(worldVerticesInBounds[i], Vector3.one);
            }
            for (int i = 0; i < worldTrianglePointsInBounds.Length-3; i+=3)
            {
                float shade = (float)i / (float)worldTrianglePointsInBounds.Length;
                Gizmos.color = new Color(0, 0, shade);

                if (i == debugTriangleDrawIndex)
                {
                    Gizmos.color = new Color(1, 0, 0);
                    Gizmos.DrawSphere(worldTrianglePointsInBounds[i + 0], 0.1f);
                }
                if (i + 1 == debugTriangleDrawIndex)
                {
                    Gizmos.color = new Color(1, 0, 0);
                    Gizmos.DrawSphere(worldTrianglePointsInBounds[i + 1], 0.1f);
                }
                if (i + 2 == debugTriangleDrawIndex)
                {
                    Gizmos.color = new Color(1, 0, 0);
                    Gizmos.DrawSphere(worldTrianglePointsInBounds[i + 2], 0.1f);
                }
                Gizmos.DrawLine(worldTrianglePointsInBounds[i + 0], worldTrianglePointsInBounds[i + 1]);
                Gizmos.DrawLine(worldTrianglePointsInBounds[i + 1], worldTrianglePointsInBounds[i + 2]);
                Gizmos.DrawLine(worldTrianglePointsInBounds[i + 2], worldTrianglePointsInBounds[i + 3]);


                Gizmos.color = new Color(1, 1, 0);
                Gizmos.DrawLine(worldTrianglePointsInBounds[i], worldTrianglePointsInBounds[i] + (worldTriangleNormalsInBounds[i] * 2));

            }
        }
    }

}