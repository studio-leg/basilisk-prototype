using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldInteraction : BasiliskInteraction
{
    public MeshFolder meshFolder;
    public SphereCollider[] handColliders;
    public int numInPlaceThreshold = 3;

    float timer = 0f;
    

    override protected void Update()
    {
        base.Update();
        if (meshFolder.numInPlace >= numInPlaceThreshold)
        {
            EndInteraction();
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
