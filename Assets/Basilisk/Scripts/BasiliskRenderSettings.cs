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
    public float Exposure = 0f;
    public float ExposureDefault = -20f;

    [Header("Params")]
    public VolumeProfile volume;

    ColorAdjustments colourAdjustments;

    void Start()
    {
    }
    
    public void Reset()
    {
        Exposure = ExposureDefault;
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
