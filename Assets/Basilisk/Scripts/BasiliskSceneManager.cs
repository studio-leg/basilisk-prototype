using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasiliskSceneManager : MonoBehaviour
{
    public enum Scene { Intro, Terrain, Emancipation, Outro};
    public Scene activeScene;

    // 0 : Space Intro
    // 1 : Terrain Calibration & Manipulation
    // 2 : Space Emancipation
    // 3 : Black Outro
    public BasiliskScene[] sceneEnvironments;
    public KeyCode nextSceneKey = KeyCode.N;

    int sceneEnvIndex = 0;
    int nextSceneEnvIndex = -1;

    private void Awake()
    {
        for (int i = 0; i < sceneEnvironments.Length; i++)
        {
            sceneEnvironments[i].Activate(false);
            sceneEnvironments[i].OnNextScene += BasiliskSceneManager_OnNextScene;
        }
    }

    void Start()
    {
        Scene scene = (Scene)((int)activeScene);
        sceneEnvIndex = GetEnvironmentIndex(scene);
        sceneEnvironments[sceneEnvIndex].Activate(true);
        sceneEnvironments[sceneEnvIndex].PlayIntro();
    }

    private void BasiliskSceneManager_OnNextScene()
    {
        NextScene();
    }

    void Update()
    {
        if (Input.GetKeyDown(nextSceneKey))
        {
            NextScene();
        }
    }

    public void NextScene()
    {
        Scene scene = (Scene)((int)activeScene + 1);
        CutToScene(scene);
    }

    /// <summary>
    /// Plays a scene without jump cutting e.g. outro current scene then intro specified scene
    /// </summary>
    /// <param name="scene"></param>
    public void PlayScene(Scene scene)
    {
        // Cache the next scene index
        nextSceneEnvIndex = GetEnvironmentIndex(scene);
        activeScene = scene;
        if (nextSceneEnvIndex != sceneEnvIndex)
        {
            // Outro the current scene
            // Next scene will transition in afterwards
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
            case Scene.Terrain:
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