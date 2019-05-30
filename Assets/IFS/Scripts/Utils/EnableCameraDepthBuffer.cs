using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnableCameraDepthBuffer : MonoBehaviour
{

    [SerializeField]
    private DepthTextureMode _depthTextureMode = DepthTextureMode.Depth;

    void Awake()
    {
        GetComponent<Camera>().depthTextureMode = _depthTextureMode;
    }
}