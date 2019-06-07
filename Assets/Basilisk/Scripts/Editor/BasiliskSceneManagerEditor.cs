using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BasiliskSceneManager))]
public class BasiliskSceneManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BasiliskSceneManager thing = (BasiliskSceneManager)target;

        GUILayout.Label("Cut To:");
        if (GUILayout.Button("Intro"))
        {
            thing.CutToScene(BasiliskSceneManager.Scene.Intro);
        }
        if (GUILayout.Button("Calibration"))
        {
            thing.CutToScene(BasiliskSceneManager.Scene.Calibration);
        }
        if (GUILayout.Button("Manipulation"))
        {
            thing.CutToScene(BasiliskSceneManager.Scene.Manipulation);
        }
        if (GUILayout.Button("Emancipation"))
        {
            thing.CutToScene(BasiliskSceneManager.Scene.Emancipation);
        }
        if (GUILayout.Button("Outro"))
        {
            thing.CutToScene(BasiliskSceneManager.Scene.Outro);
        }
    }
}