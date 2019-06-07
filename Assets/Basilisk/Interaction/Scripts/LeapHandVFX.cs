using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.VFX.Utils;

public class LeapHandVFX : MonoBehaviour
{

    public Transform hand;
    public VisualEffect visualEffect;
    public string positionPropertyName = "Pos";
    
    void Start()
    {
    }
    
    void Update()
    {
        if (visualEffect && hand)
        {
            visualEffect.SetVector3(positionPropertyName, hand.position);
        }
    }
}