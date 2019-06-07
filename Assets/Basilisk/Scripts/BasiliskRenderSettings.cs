using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Playables;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class BasiliskRenderSettings : MonoBehaviour
{
    [Header("Controls")]
    public float Exposure = 0;

    [Header("Params")]
    public VolumeProfile volume;

    Exposure exposure;

    void Start()
    {
    }
    
    void Update()
    {
        if (volume)
        {
            if (!exposure)
            {
                volume.TryGet(out exposure);
            }
            if (exposure)
            {
                var fixedExposure = exposure.fixedExposure;
                fixedExposure.value = Exposure;
                exposure.fixedExposure = fixedExposure;
            }
        }
    }
    
}
