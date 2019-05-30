using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GUIParams
{
    public bool enabled = false;
    public Vector2 position = new Vector2(20, 20);
    public Color colour = Color.white;
    public int width = 250;
    public int height = 30;
    public GUISkin skin;
}