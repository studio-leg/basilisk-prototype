using Basilisk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class TreeFoldInteraction : BasiliskInteraction
{
    public MeshFolder meshFolder;
    public SphereCollider[] handColliders;
    public int numInPlaceThreshold = 3;

    public BasiliskLeapHand leftHand;
    public BasiliskLeapHand rightHand;
    public VisualEffect leftHandVFX;
    public VisualEffect rightHandVFX;
    public string handVFXScalePropName = "SDFScaleMult";

    float timer = 0f;
    

    override protected void Update()
    {
        base.Update();
        if (meshFolder.numInPlace >= numInPlaceThreshold)
        {
            EndInteraction();
        }
        if (isActive)
        {
            leftHand.isActive = true;
            rightHand.isActive = true;
            if (leftHand.raycastHit)
            {
                leftHandVFX.SetFloat(handVFXScalePropName, 3f);
            }
            else
            {
                leftHandVFX.SetFloat(handVFXScalePropName, 1f);
            }
            if (rightHand.raycastHit)
            {
                rightHandVFX.SetFloat(handVFXScalePropName, 3f);
            }
            else
            {
                rightHandVFX.SetFloat(handVFXScalePropName, 1f);
            }
        }
        else
        {
            leftHand.isActive = false;
            rightHand.isActive = false;
            leftHandVFX.SetFloat(handVFXScalePropName, 3f);
            rightHandVFX.SetFloat(handVFXScalePropName, 1f);
        }
    }

    override public void Reset()
    {
        timer = percent = 0f;
        meshFolder.Reset();
    }

    override public void BeginInteraction()
    {
        base.BeginInteraction();
        meshFolder.Reset();
        meshFolder.doUpdateMesh = true;
    }

    override public void EndInteraction()
    {
        base.EndInteraction();
        meshFolder.doUpdateMesh = false;
    }
}
