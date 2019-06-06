﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

[ExecuteInEditMode]
public class TerrainScene : BasiliskScene
{
    [Header("Controls")]
    [Range(0f, 1f)]
    public float sunriseProgress = 0f;
    float sunriseProgress_ = 0f;
    [Range(0f, 1f)]
    public float handsProgress = 0f;
    float handsProgress_ = 0f;

    [Header("Sun")]
    public Light mainLight;
    public Vector3 lightRotMin = new Vector3(-6f, -212f, 90f);
    public Vector3 lightRotMax = new Vector3(159.42f, -212f, 90f);
    
    [Header("Calibration Light")]
    public MeshRenderer calibrationLightMesh;

    [Header("Hands")]
    [Tooltip("Visual effect whose property to set with the output SDF texture")]
    public VisualEffect handVFX;
    public string handVFXSpawnPropName = "SpawnRate";
    public SkinnedMeshRenderer leftHandMesh;
    public SkinnedMeshRenderer rightHandMesh;


    void Start()
    {
        
    }
    
    void Update()
    {
        if (sunriseProgress != sunriseProgress_)
        {
            sunriseProgress_ = sunriseProgress;
            var rotation = Vector3.Lerp(lightRotMin, lightRotMax, sunriseProgress);
            mainLight.transform.rotation = Quaternion.Euler(rotation);
            calibrationLightMesh.gameObject.SetActive(sunriseProgress < 1);
        }
        if (handsProgress != handsProgress_)
        {
            handsProgress_ = handsProgress;
            handVFX.SetFloat(handVFXSpawnPropName, handsProgress_ * 400);
        }
    }
}
