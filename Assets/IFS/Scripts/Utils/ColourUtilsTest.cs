using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColourUtilsTest : MonoBehaviour
{

    public Color output = Color.white;
    public bool updateSeed = true;
    
    [Header("Saturation")]
    [Range(0f, 1f)]
    public float saturationMin = 0f;
    [Range(0f, 1f)]
    public float saturationMax = 1f;

    [Header("Value")]
    [Range(0f, 1f)]
    public float valueMin = 0f;
    [Range(0f, 1f)]
    public float valueMax = 1f;


    [Range(0f, 1f)]
    public float exponentialEasingAmount = 0f;

    [Range(0f, 1f)]
    public float saturationNorm = 0f;
    float valueNorm = 0f;


    void Start()
    {

    }
    
    void Update()
    {
        if (updateSeed)
        {
            saturationNorm = Random.value;
            updateSeed = false;
        }
        UpdateColour();
    }

    void UpdateColour()
    {
        valueNorm = MathUtils.exponentialEasing(saturationNorm, exponentialEasingAmount);
        var saturation = MathUtils.Map(saturationNorm, 0f, 1f, saturationMin, saturationMax);
        var value = MathUtils.Map(valueNorm, 0f, 1f, valueMin, valueMax);
        output = ColourUtils.GetRandomHue(saturation, value);
    }
}