using Basilisk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class FoldInteraction : BasiliskInteraction
{
    public MeshFolder meshFolder;
    public GrabbableRemote[] handles;
    public int numInPlaceThreshold = 3;

    [Header("Audio")]
    public AudioSource audioAffirm;
    public AudioSource audioPrompt;
    public AudioClip audioTakeHold;
    public AudioClip audioThereEasy;
    public AudioClip audioExtendArms;
    public AudioClip audioGrabLift;
    bool hasPlayedAudioTakeHold = false;
    bool hasPlayedAudioThereEasy = false;

    [Header("Hands")]
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
            bool eitherHit = false;
            if (leftHand.raycastHit)
            {
                eitherHit = true;
                leftHandVFX.SetFloat(handVFXScalePropName, 3f);
            }
            else
            {
                leftHandVFX.SetFloat(handVFXScalePropName, 1f);
            }
            if (rightHand.raycastHit)
            {
                eitherHit = true;
                rightHandVFX.SetFloat(handVFXScalePropName, 3f);
            }
            else
            {
                rightHandVFX.SetFloat(handVFXScalePropName, 1f);
            }

            if (audioAffirm)
            {

                if (eitherHit && !hasPlayedAudioTakeHold)
                {
                    hasPlayedAudioTakeHold = true;
                    audioAffirm.clip = audioTakeHold;
                    audioAffirm.Play();
                }

                bool eitherGrabbing = (leftHand.isGrippingRemote || rightHand.isGrippingRemote);
                if (eitherGrabbing && !hasPlayedAudioThereEasy && !audioAffirm.isPlaying)
                {
                    hasPlayedAudioThereEasy = true;
                    audioAffirm.clip = audioThereEasy;
                    audioAffirm.Play();
                }
            }

        }
        else
        {
            //leftHand.isActive = false;
            //rightHand.isActive = false;
            //leftHandVFX.SetFloat(handVFXScalePropName, 3f);
            //rightHandVFX.SetFloat(handVFXScalePropName, 1f);
        }
    }

    override public void Reset()
    {
        hasPlayedAudioThereEasy = false;
        hasPlayedAudioTakeHold = false;
        timer = percent = 0f;
        meshFolder.Reset();
    }

    override public void BeginInteraction()
    {
        base.BeginInteraction();
        //meshFolder.Reset();
        meshFolder.doUpdateMesh = true;
        EnableHandles();
    }

    override public void EndInteraction()
    {
        base.EndInteraction();
        //meshFolder.doUpdateMesh = false;
        DisableHandles();
    }

    void EnableHandles()
    {
        var all = meshFolder.GetComponentsInChildren<GrabbableRemote>();
        for (int i = 0; i < all.Length; i++)
        {
            all[i].active = false;
        }
        for (int i = 0; i < handles.Length; i++)
        {
            handles[i].active = true;
        }
    }

    void DisableHandles()
    {
        for (int i = 0; i < handles.Length; i++)
        {
            handles[i].active = false;
        }
    }

    public void HandlesToHome()
    {
        for (int i = 0; i < handles.Length; i++)
        {
            handles[i].ForceHome = true;
        }
    }
}
