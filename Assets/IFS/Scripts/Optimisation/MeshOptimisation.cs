using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshOptimisation : MonoBehaviour {

    public bool castShadows = true;
    public bool receiveShadows = true;

    MeshRenderer[] meshRenderers;
    
    void OnEnable() {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    void Update() {

        foreach (var meshRenderer in meshRenderers)
        {
            if (castShadows)
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            else
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            meshRenderer.receiveShadows = receiveShadows;
        }
    }
}