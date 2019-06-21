using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

[ExecuteInEditMode]
public class IntroScene : BasiliskScene
{

    public bool isStepActive = false;
    Doorway doorway;

    void Start()
    {
        doorway = GetComponentInChildren<Doorway>(true);
        doorway.OnEnter += Doorway_OnEnter;
    }

    public override void Activate(bool active)
    {
        isStepActive = false;
        base.Activate(active);
    }

    private void Doorway_OnEnter()
    {
        if (isStepActive)
        {
            isStepActive = false;
            director.Play();
        }
    }

    void Update()
    {
        if (isStepActive && Input.GetMouseButtonDown(0))
        {
            isStepActive = false;
            director.Play();
        }
    }

    public void StartStepInteraction()
    {
        Pause();
        isStepActive = true;
    }
}
