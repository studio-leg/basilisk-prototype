using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralPlane {

	static public Mesh Generate(float length = 1f, float width = 1f, int resX = 2, int resZ = 2)
    {

        Mesh mesh = new Mesh();
        mesh.Clear();
        
        #region Vertices		
        Vector3[] vertices = new Vector3[resX * resZ];
        for (int z = 0; z < resZ; z++)
        {
            // [ -length / 2, length / 2 ]
            float zPos = ((float)z / (resZ - 1) - 0.5f) * length;
            for (int x = 0; x < resX; x++)
            {
                // [ -width / 2, width / 2 ]
                float xPos = ((float)x / (resX - 1) - .5f) * width;
                vertices[x + z * resX] = new Vector3(xPos, 0f, zPos);
            }
        }
        #endregion

        #region Normals
        Vector3[] normals = new Vector3[vertices.Length];
        for (int n = 0; n < normals.Length; n++)
            normals[n] = Vector3.up;
        #endregion

        #region UVs		
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int v = 0; v < resZ; v++)
        {
            for (int u = 0; u < resX; u++)
            {
                uvs[u + v * resX] = new Vector2((float)u / (resX - 1), (float)v / (resZ - 1));
            }
        }
        #endregion

        #region Triangles
        int nbFaces = (resX) * (resZ);
        int[] triangles = new int[nbFaces * 6];
        for (int ti = 0, vi = 0, y = 0; y < resZ-1; y++, vi++)
        {
            for (int x = 0; x < resX-1; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + resX;
                triangles[ti + 5] = vi + resX + 1;
            }
        }
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();

        return mesh;
    }
}
