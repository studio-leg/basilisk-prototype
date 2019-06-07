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

        GUILayout.Space(10f);
        GUILayout.Label("Cut to Scene");
        if (GUILayout.Button("Intro"))
        {
            thing.CutToScene(BasiliskSceneManager.Scene.Intro);
        }
        if (GUILayout.Button("Terrain"))
        {
            thing.CutToScene(BasiliskSceneManager.Scene.Terrain);
        }
        if (GUILayout.Button("Emancipation"))
        {
            thing.CutToScene(BasiliskSceneManager.Scene.Emancipation);
        }
        if (GUILayout.Button("Outro"))
        {
            thing.CutToScene(BasiliskSceneManager.Scene.Outro);
        }
        
        GUILayout.Space(10f);
        GUILayout.Label("Transition to Scene");
        if (GUILayout.Button("Intro"))
        {
            thing.PlayScene(BasiliskSceneManager.Scene.Intro);
        }
        if (GUILayout.Button("Terrain"))
        {
            thing.PlayScene(BasiliskSceneManager.Scene.Terrain);
        }
        if (GUILayout.Button("Emancipation"))
        {
            thing.PlayScene(BasiliskSceneManager.Scene.Emancipation);
        }
        if (GUILayout.Button("Outro"))
        {
            thing.PlayScene(BasiliskSceneManager.Scene.Outro);
        }
    }
}