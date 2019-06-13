using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasiliskInteraction : MonoBehaviour
{
    [Range(0f, 1f)]
    public float percent = 0f;
    public bool doReset = false;
    
    public delegate void InteractionCompleteAction();
    public event InteractionCompleteAction OnInteractionComplete;


    protected virtual void Update()
    {
        if (doReset)
        {
            doReset = false;
            Reset();
        }
    }

    public virtual void BeginInteraction()
    {
    }

    public virtual void EndInteraction()
    {
        Debug.Log("End Interaction");
        OnInteractionComplete?.Invoke();
    }

    public virtual void Reset()
    {

    }
}
