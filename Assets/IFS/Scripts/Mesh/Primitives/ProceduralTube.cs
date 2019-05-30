using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTube
{

    public static Mesh Generate(float radius = 1f, float length = 1f, int radiusRes = 24, int lengthRes = 24, bool centre = true)
    {
        Mesh mesh = new Mesh();

        // Create array of length points
        var lengthPoints = new Vector3[lengthRes];
        for (int i = 0; i < lengthRes; i++)
        {
            float position = length * ((float)i / (float)lengthRes);
            lengthPoints[i] = new Vector3(0, 0, position);
        }

        // Create radius points
        var radiusPoints = CircumferenceShapeUtils.GetCircle(radiusRes, radius).ToArray();

        
        // init arays of vertices, UVs and normals
        Vector3[] vertices = new Vector3[(lengthPoints.Length * radiusPoints.Length) + 1];
        Vector2[] uvs = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Color[] colours = new Color[vertices.Length];

        // init triangles
        int nbFaces = vertices.Length;
        int nbTriangles = nbFaces * 2;
        int nbIndexes = nbTriangles * 3;
        int[] triangles = new int[nbIndexes];
        int triI = 0;

        int vertI = 0;
        for (int radiusI = 0; radiusI < radiusPoints.Length; radiusI++)
        {
            Vector3 radiusPoint = new Vector3(radiusPoints[radiusI].x, radiusPoints[radiusI].y);
            float radiusPercent = MathUtils.Map(radiusI, 0, radiusPoints.Length - 1, 0, 1);

            for (int lengthI = 0; lengthI < lengthPoints.Length; lengthI++)
            {
                float lengthPercent = MathUtils.Map(lengthI, 0, lengthPoints.Length - 1, 0, 1);
                var lengthPoint = lengthPoints[lengthI];
                
                Matrix4x4 tmpPosMat = Matrix4x4.TRS(new Vector3(0, 0, lengthPoint.z), Quaternion.Euler(0, 0, 0), new Vector3(1, 1, 1));
                Vector3 rotatedPoint = tmpPosMat.MultiplyPoint3x4(radiusPoint);

                vertices[vertI] = rotatedPoint;
                normals[vertI] = (rotatedPoint - lengthPoint).normalized;
                uvs[vertI] = new Vector2(lengthPercent, radiusPercent);
                colours[vertI] = new Color(normals[vertI].x, normals[vertI].y, normals[vertI].z, 1f);

                // triangles
                // next refers to the next horizontal segment
                int current = vertI;
                int next = current + lengthPoints.Length;
                // if this is the last segment, attach it to the first
                if (radiusI == radiusPoints.Length - 1)
                {
                    next = lengthI;
                }

                tangents[vertI] = Vector3.forward;
                tangents[vertI].z = 1f;

                // we're attaching triangles to the next row, so don't do this for the last point
                if (lengthI < lengthPoints.Length - 1)
                {
                    // Clockwise triangles for back to front mesh generation
                    // Triangle 1
                    triangles[triI++] = current;
                    triangles[triI++] = next + 1;
                    triangles[triI++] = current + 1;
                    // Triangle 2
                    triangles[triI++] = current;
                    triangles[triI++] = next;
                    triangles[triI++] = next + 1;
                }
                vertI++;
            }
        }

        // build the mesh
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.tangents = tangents;
        mesh.colors = colours;
        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();

        return mesh;
    }
}