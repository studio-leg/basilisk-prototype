using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class PlyUtils
{
    /// <summary>
    /// ply
    /// format ascii 1.0           { ascii/binary, format version number
    /// }
    /// comment made by anonymous { comments are keyword specified }
    /// comment this file is a cube
    /// 
    /// element vertex 8           { define "vertex" element, 8 in file }
    /// property float32 x         { vertex contains float "x" coordinate }
    ///         property float32 y         { y coordinate is also a vertex property }
    ///         property float32 z         { z coordinate, too }
    ///         element face 6             { there are 6 "face" elements in the file }
    ///         property list uint8 int32 vertex_index
    ///                                    { "vertex_indices" is a list of ints }
    ///         end_header                 { delimits the end of the header }
    ///         0 0 0                      { start of vertex list }
    ///         0 0 1
    ///         0 1 1
    ///         0 1 0
    ///         1 0 0
    ///         1 0 1
    ///         1 1 1
    ///         1 1 0
    ///         4 0 1 2 3                  { start of face list }
    ///         4 7 6 5 4
    ///         4 0 4 5 1
    ///         4 1 5 6 2
    ///         4 2 6 7 3
    ///         4 3 7 4 0
    /// </summary>
    /// <param name="mesh"></param>
    /// <returns></returns>
    public static string MeshToPly(Mesh mesh)
    {
        var vertices = mesh.vertices;
        var triangles = mesh.triangles;
        return MeshToPly(vertices, triangles);
    }
    public static string MeshToPly(Vector3[] vertices, int[] triangles)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("ply");
        builder.AppendLine("format ascii 1.0");
        builder.AppendLine("comment Generated in Unity");
        builder.AppendLine(string.Format("element vertex {0}", vertices.Length));
        builder.AppendLine("property float32 x");
        builder.AppendLine("property float32 y");
        builder.AppendLine("property float32 z");
        builder.AppendLine(string.Format("element face {0}", triangles.Length/3));
        builder.AppendLine("property list uint8 int32 vertex_index");
        builder.AppendLine("end_header");

        for (int i = 0; i < vertices.Length; i++)
        {
            builder.AppendLine(string.Format("{0} {1} {2}", vertices[i].x, vertices[i].y, vertices[i].z));
        }
        for (int i = 0; i < triangles.Length; i+=3)
        {
            builder.AppendLine(string.Format("3 {0} {1} {2}", triangles[i + 0], triangles[i + 1], triangles[i + 2]));
        }

        return builder.ToString();
    }

    public static string PointsToPly(Vector3[] vertices)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("ply");
        builder.AppendLine("format ascii 1.0");
        builder.AppendLine("comment Generated in Unity");
        builder.AppendLine(string.Format("element vertex {0}", vertices.Length));
        builder.AppendLine("property float32 x");
        builder.AppendLine("property float32 y");
        builder.AppendLine("property float32 z");
        builder.AppendLine("end_header");
        for (int i = 0; i < vertices.Length; i++)
        {
            builder.Append(vertices[i].x.ToString());
            builder.Append(" ");
            builder.Append(vertices[i].y.ToString());
            builder.Append(" ");
            builder.Append(vertices[i].z.ToString());
            builder.Append("\n");
            //builder.AppendLine(string.Format("{0} {1} {2}", vertices[i].x, vertices[i].y, vertices[i].z));
        }
        return builder.ToString();
    }

    /// <summary>
    /// Saves a point cloud to a binary ply file
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="path"></param>
    public static void PointsToPlyBinary(Vector3[] vertices, string path)
    {
        BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Create), Encoding.ASCII);
        writer.Write(Str2byteArray("ply\n"));
        writer.Write(Str2byteArray("format binary_little_endian 1.0\n"));
        writer.Write(Str2byteArray("comment Generated in Unity\n"));
        writer.Write(Str2byteArray(string.Format("element vertex {0}\n", vertices.Length)));
        writer.Write(Str2byteArray("property float x\n"));
        writer.Write(Str2byteArray("property float y\n"));
        writer.Write(Str2byteArray("property float z\n"));
        writer.Write(Str2byteArray("end_header\n"));
        for (int i = 0; i < vertices.Length; i++)
        {
            writer.Write(Float2byteArray(vertices[i].x));
            writer.Write(Float2byteArray(vertices[i].y));
            writer.Write(Float2byteArray(vertices[i].z));
        }
        writer.Close();
    }

    static byte[] Float2byteArray(float value)
    {
        return System.BitConverter.GetBytes(value);
    }

    static byte[] Str2byteArray(string theString)
    {
        return System.Text.Encoding.ASCII.GetBytes(theString);
    }

}