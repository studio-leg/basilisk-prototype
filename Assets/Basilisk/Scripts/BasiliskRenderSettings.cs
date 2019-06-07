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

    ColorAdjustments colourAdjustments;

    void Start()
    {
    }
    
    void Update()
    {
        if (volume)
        {
            if (!colourAdjustments)
            {
                volume.TryGet(out colourAdjustments);
            }
            if (colourAdjustments)
            {
                var exposure = colourAdjustments.postExposure;
                exposure.value = Exposure;
                colourAdjustments.postExposure = exposure;
            }
        }
    }
    
}
