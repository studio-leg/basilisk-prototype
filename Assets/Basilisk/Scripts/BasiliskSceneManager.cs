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
    int sceneEnvIndex = 0;
    int nextSceneEnvIndex = -1;


    void Start()
    {

    }
    
    void Update()
    {

    }

    /// <summary>
    /// Plays a scene without jump cutting e.g. outro current scene then intro specified scene
    /// </summary>
    /// <param name="scene"></param>
    public void PlayScene(Scene scene)
    {
        // outro current scene
        // intro target scene
        nextSceneEnvIndex = GetEnvironmentIndex(scene);
        if (nextSceneEnvIndex != sceneEnvIndex)
        {
            sceneEnvironments[sceneEnvIndex].OnOutroComplete += BasiliskSceneManager_OnOutroComplete;
            sceneEnvironments[sceneEnvIndex].PlayOutro();
        }
    }

    /// <summary>
    /// Called when a scene has finished playing its outro
    /// </summary>
    private void BasiliskSceneManager_OnOutroComplete()
    {
        // Disable outroed scene
        sceneEnvironments[sceneEnvIndex].OnOutroComplete -= BasiliskSceneManager_OnOutroComplete;
        sceneEnvironments[sceneEnvIndex].Activate(false);
        // If we have a scene queued, activate and play it
        if (nextSceneEnvIndex > -1)
        {
            sceneEnvIndex = nextSceneEnvIndex;
            sceneEnvironments[sceneEnvIndex].Activate(true);
            sceneEnvironments[sceneEnvIndex].PlayIntro();
            nextSceneEnvIndex = -1;
        }
    }

    /// <summary>
    /// Jump cuts to the specified scene
    /// </summary>
    /// <param name="scene"></param>
    public void CutToScene(Scene scene)
    {
        // disable current scene
        // enable target scene
        sceneEnvironments[sceneEnvIndex].Activate(false);
        sceneEnvIndex = GetEnvironmentIndex(scene);
        sceneEnvironments[sceneEnvIndex].Activate(true);
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