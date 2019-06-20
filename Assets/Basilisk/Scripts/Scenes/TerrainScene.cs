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

    [Header("Interactions")]
    NoticeLightInteraction noticeLight;
    CalibrationLightInteraction gazeAtLight;
    public CalibrationHandsInteraction gazeAtHands;
    public FoldInteraction treeFolding;
    public FoldInteraction folding;

    void Start()
    {
        if (noticeLight) noticeLight.OnInteractionComplete += NoticeLight_OnInteractionComplete;
        if (gazeAtLight) gazeAtLight.OnInteractionComplete += GazeAtLight_OnInteractionComplete;
        if (gazeAtHands) gazeAtHands.OnInteractionComplete += GazeAtHands_OnInteractionComplete;
        if (folding) folding.OnInteractionComplete += Folding_OnInteractionComplete;
        if (treeFolding) treeFolding.OnInteractionComplete += TreeFolding_OnInteractionComplete;
    }
    
    void Update()
    {
        if (sunriseProgress != sunriseProgress_)
        {
            sunriseProgress_ = sunriseProgress;
        }
        if (handsProgress != handsProgress_)
        {
            handsProgress_ = handsProgress;
        }
    }

    #region Interaction Complete Listeners
    private void TreeFolding_OnInteractionComplete()
    {
        EndFoldTreeInteraction();
    }

    private void Folding_OnInteractionComplete()
    {
        EndFoldInteraction();
    }

    private void GazeAtHands_OnInteractionComplete()
    {
        EndHandInteraction();
    }

    private void GazeAtLight_OnInteractionComplete()
    {
        EndGazeLightInteraction();
    }

    private void NoticeLight_OnInteractionComplete()
    {
        EndNoticeLightInteraction();
    }
    #endregion

    #region Interaction Controls
    public void StartNoticeLightInteraction()
    {
        director.Pause();
        noticeLight.BeginInteraction();
    }
    public void EndNoticeLightInteraction()
    {
        director.Play();
    }

    public void StartGazeLightInteraction()
    {
        director.Pause();
        gazeAtLight.BeginInteraction();
    }
    public void EndGazeLightInteraction()
    {
        director.Play();
    }

    public void StartHandInteraction()
    {
        //director.Pause();
        gazeAtHands.BeginInteraction();
    }
    public void EndHandInteraction()
    {
        director.Play();
    }

    public void StartFoldTreeInteraction()
    {
        director.Pause();
        treeFolding.BeginInteraction();
    }
    public void EndFoldTreeInteraction()
    {
        director.Play();
    }
    public void TreeToHome()
    {
        treeFolding.HandlesToHome();
    }

    public void StartFoldInteraction()
    {
        director.Pause();
        folding.BeginInteraction();
    }
    public void EndFoldInteraction()
    {
        director.Play();
    }
    #endregion

}
