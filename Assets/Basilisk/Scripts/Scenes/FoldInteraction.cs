using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldInteraction : MonoBehaviour
{
    public MeshFolder meshFolder;
    public SphereCollider[] handColliders;

    [Range(0f, 1f)]
    public float percent = 0f;
    public bool doReset = false;
    float timer = 0f;

    void Start()
    {

    }

    void Update()
    {
        if (doReset)
        {
            doReset = false;
            Reset();
        }
    }

    public void Reset()
    {
        timer = percent = 0f;
    }

    public void BeginInteraction()
    {
        meshFolder.Reset();
        meshFolder.doUpdateMesh = true;
    }

    public void EndInteraction()
    {
        meshFolder.doUpdateMesh = false;
    }
}
