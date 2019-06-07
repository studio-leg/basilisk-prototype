using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BasiliskRenderController))]
public class BasiliskRenderControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BasiliskRenderController thing = (BasiliskRenderController)target;

        GUILayout.Space(10f);
        if (GUILayout.Button("Fade In"))
        {
            thing.FadeIn();
        }
        if (GUILayout.Button("Fade Out"))
        {
            thing.FadeOut();
        }
    }
}