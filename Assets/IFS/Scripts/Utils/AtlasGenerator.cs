using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[ExecuteInEditMode]
public class AtlasGenerator : MonoBehaviour
{
    public bool DoGenerate = false;
    public bool DoSave = false;
    public bool DoLoad = false;
    public int width = 8192;
    public int height = 8192;
    public string directory;
    public string filename;
    public Texture2D[] atlasTextures;
    public Rect[] rects;
    Texture2D atlas;
    
    void Update()
    {
        if (DoGenerate)
        {
            DoGenerate = false;
            Generate();
        }
        if (DoSave)
        {
            DoSave = false;
            Save();
        }
        if (DoLoad)
        {
            DoLoad = false;
            Load();
        }
    }

    void Generate()
    {
        atlas = new Texture2D(width, height, TextureFormat.RGB24, false, true);
        rects = atlas.PackTextures(atlasTextures, 0, width);
    }

    void Save()
    {
        byte[] bytes = atlas.EncodeToPNG();
        File.WriteAllBytes(string.Format("{0}/{1}.png", directory, filename), bytes);
        /*
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Format("{0}/{1}.dat", directory, filename));
        bf.Serialize(file, rects);
        file.Close();
        */
    }

    void Load()
    {
        /*
        var filePath = string.Format("{0}/{1}.dat", directory, filename);
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            rects = (Rect[])bf.Deserialize(file);
            file.Close();
        }
        */
    }
}