using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BasiliskScene : MonoBehaviour
{
    public bool isActive = true;

    bool isActive_ = true;
    public GameObject sceneAssets;
    BasiliskRenderController renderController;


    public delegate void OutroCompleteAction();
    public event OutroCompleteAction OnOutroComplete;
    public delegate void IntroCompleteAction();
    public event IntroCompleteAction OnIntroComplete;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (isActive != isActive_)
        {
            isActive_ = isActive;
            Activate(isActive_);
        }
    }

    public virtual void Activate(bool active)
    {
        if (!sceneAssets)
        {
            sceneAssets = GetComponentInChildren<BasiliskSceneAssets>(true).gameObject;
        }
        sceneAssets.SetActive(active);
    }

    public virtual void PlayIntro()
    {
        if (!renderController)
        {
            renderController = FindObjectOfType<BasiliskRenderController>();
        }
        renderController.OnFadeInComplete += RenderController_OnFadeInComplete;
        renderController.FadeIn();
    }
    
    public virtual void PlayOutro()
    {
        if (!renderController)
        {
            renderController = FindObjectOfType<BasiliskRenderController>();
        }
        renderController.OnFadeOutComplete += RenderController_OnFadeOutComplete;
        renderController.FadeOut();
    }
    
    private void RenderController_OnFadeInComplete()
    {
        Debug.Log("BasiliskScene FadeInComplete");
        renderController.OnFadeInComplete -= RenderController_OnFadeInComplete;
        OnIntroComplete?.Invoke();
    }

    private void RenderController_OnFadeOutComplete()
    {
        Debug.Log("BasiliskScene FadeOutComplete");
        renderController.OnFadeOutComplete -= RenderController_OnFadeOutComplete;
        OnOutroComplete?.Invoke();
    }
}
