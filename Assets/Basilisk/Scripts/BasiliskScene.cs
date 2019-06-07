using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BasiliskScene : MonoBehaviour
{
    public bool isActive = true;

    bool isActive_ = true;
    GameObject sceneAssets;

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
            sceneAssets = GetComponentInChildren<BasiliskSceneAssets>().gameObject;
        }
        sceneAssets.SetActive(active);
    }

    public virtual void PlayIntro()
    {

    }

    public virtual void PlayOutro()
    {

    }
}
