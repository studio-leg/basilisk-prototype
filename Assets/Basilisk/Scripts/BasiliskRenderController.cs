using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Playables;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class BasiliskRenderController : MonoBehaviour
{
    [Header("Params")]
    public PlayableAsset fadeInAsset;
    public PlayableAsset fadeOutAsset;

    public delegate void FadeOutCompleteAction();
    public event FadeOutCompleteAction OnFadeOutComplete;
    public delegate void FadeInCompleteAction();
    public event FadeInCompleteAction OnFadeInComplete;

    PlayableDirector director;

    void Start()
    {
    }
    
    void Update()
    {
        if (!director)
        {
            director = GetComponent<PlayableDirector>();
        }
    }

    public void FadeIn()
    {
        director.Play(fadeInAsset);
    }


    public void FadeOut()
    {
        director.Play(fadeOutAsset);
    }

    public void FadeInComplete()
    {
        Debug.Log("BasiliskRenderController FadeInComplete");
        OnFadeInComplete?.Invoke();
    }

    public void FadeOutComplete()
    {
        Debug.Log("BasiliskRenderController FadeOutComplete");
        OnFadeOutComplete?.Invoke();
    }
}
