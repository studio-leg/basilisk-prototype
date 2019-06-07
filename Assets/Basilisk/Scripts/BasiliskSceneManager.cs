using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasiliskSceneManager : MonoBehaviour
{
    public enum Scene { Intro, Calibration, Manipulation, Emancipation, Outro};
    public Scene activeScene;

    // 0 : Space Intro
    // 1 : Terrain Calibration & Manipulation
    // 2 : Space Emancipation
    // 3 : Black Outro
    public BasiliskScene[] sceneEnvironments;
    int sceneEnvironmentIndex = 0;


    void Start()
    {

    }
    
    void Update()
    {

    }

    public void PlayScene(Scene scene)
    {
        // outro current scene
        // intro target scene
    }

    public void CutToScene(Scene scene)
    {
        // disable current scene
        // enable target scene
        sceneEnvironments[sceneEnvironmentIndex].Activate(false);
        sceneEnvironmentIndex = GetEnvironmentIndex(scene);
        sceneEnvironments[sceneEnvironmentIndex].Activate(true);
        activeScene = scene;
    }
    
    int GetEnvironmentIndex(Scene scene)
    {
        switch (scene)
        {
            case Scene.Intro:
                return 0;
            case Scene.Calibration:
                return 1;
            case Scene.Manipulation:
                return 1;
            case Scene.Emancipation:
                return 2;
            case Scene.Outro:
                return 3;
            default:
                return -1;
        }
    }
}