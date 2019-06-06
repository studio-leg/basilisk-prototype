using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX.Utils;

public class LeapHandVFX : MonoBehaviour
{

    public Transform hand;
    public VFXParameterBinder paramBinder;

    //VisualEffec

    // Start is called before the first frame update
    void Start()
    {
        paramBinder.ClearParameterBinders();
        var param = paramBinder.AddParameterBinder<VFXPositionBinder>();
        param.Parameter = "Pos";
        param.Target = hand;
    }

    // Update is called once per frame
    void Update()
    {
    }
}