using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.Playables;

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

    [Header("Calibration Light")]
    CalibrationLightInteraction calibrationLightInteraction;

    [Header("Hands")]
    CalibrationHandsInteraction calibrationHandsInteraction;
    
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (!calibrationLightInteraction)
        {
            calibrationLightInteraction = GetComponent<CalibrationLightInteraction>();
        }

        if (!calibrationHandsInteraction)
        {
            calibrationHandsInteraction = GetComponent<CalibrationHandsInteraction>();
        }

        if (sunriseProgress != sunriseProgress_)
        {
            sunriseProgress_ = sunriseProgress;
        }
        if (handsProgress != handsProgress_)
        {
            handsProgress_ = handsProgress;
        }
    }

    public void StartNoticeLightInteraction()
    {
        director.Pause();
    }
    public void EndNoticeLightInteraction()
    {
        director.Play();
    }

    public void StartGazeLightInteraction()
    {
        director.Pause();
    }
    public void EndGazeLightInteraction()
    {
        director.Play();
    }

    public void StartHandInteraction()
    {
        director.Pause();
    }
    public void EndHandInteraction()
    {
        director.Play();
    }

    public void StartFoldInteraction()
    {
        director.Pause();
    }
    public void EndFoldInteraction()
    {
        director.Play();
    }
}
