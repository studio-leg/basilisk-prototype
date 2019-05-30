using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    string text = "";
    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }
    
    public void AddText(string text)
    {
        this.text = text;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(20, 20, w, h);
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        //style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

        string text = "SHELTER TABLE";

        System.TimeSpan time = System.TimeSpan.FromSeconds(Time.time);
        text += string.Format("\nUptime:\n{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", time.Hours,
                time.Minutes,
                time.Seconds,
                time.Milliseconds);
        text += string.Format("\n{0:0.0} ms / {1:0.} fps", msec, fps);

        text += string.Format("\n\nSCREEN\nResolution: {0}", Screen.currentResolution);
        text += string.Format("\nDPI: {0})", Screen.dpi);

        text += this.text;

        GUI.Label(rect, text, style);
    }
}