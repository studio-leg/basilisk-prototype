using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasiliskInteraction : MonoBehaviour
{
    [Range(0f, 1f)]
    public float percent = 0f;
    public bool doReset = false;

    public bool isActive = false;

    public delegate void InteractionCompleteAction();
    public event InteractionCompleteAction OnInteractionComplete;


    protected virtual void Update()
    {
        if (doReset)
        {
            doReset = false;
            Reset();
        }

        if (isActive && percent >= 1f)
        {
            EndInteraction();
        }
    }

    public virtual void BeginInteraction()
    {
        isActive = true;
    }

    public virtual void EndInteraction()
    {
        isActive = false;
        //Debug.Log("End Interaction");
        OnInteractionComplete?.Invoke();
    }

    public virtual void Reset()
    {
    }
}
