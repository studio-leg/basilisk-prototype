using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtilsVisualiser : MonoBehaviour
{
    public enum Type
    {
        SmoothStep, SmoothStepInOut, ExponentialEasing
    }
    public Type type;
    public StepFunctionParams stepParams;
    [Range(0f, 1f)]
    public float exponentialEasingAmount = 0f;
    public Vector2 resolution = new Vector2(100, 100);
    public Vector2 size = new Vector2(10, 10);
    
    void Start()
    {

    }
    
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        var step = new Vector2(size.x / resolution.x, size.y / resolution.y);
        var x = 0f;
        
        Gizmos.color = Color.black;
        Gizmos.DrawLine(new Vector3(0,0), new Vector3(size.x, 0));
        Gizmos.DrawLine(new Vector3(size.x, 0), new Vector3(size.x, size.y));
        Gizmos.DrawLine(new Vector3(size.x, size.y), new Vector3(0, size.y));
        Gizmos.DrawLine(new Vector3(0, size.y), new Vector3(0, 0));

        Gizmos.color = Color.white;
        float x1 = 0f;
        float x2 = 0f;
        float y1 = 0f;
        float y2 = 0f;
        for (int i = 0; i < resolution.x - 1; i++)
        {
            x1 = (float)i / resolution.x;
            x2 = ((float)i + 1) / resolution.x;
            switch (type)
            {
                case Type.SmoothStep:
                    y1 = MathUtils.Smoothstep(stepParams.low0, stepParams.high1, x1) * size.y;
                    y2 = MathUtils.Smoothstep(stepParams.low0, stepParams.high1, x2) * size.y;
                    break;
                case Type.SmoothStepInOut:
                    y1 = MathUtils.SmoothStepInOut(stepParams, x1) * size.y;
                    y2 = MathUtils.SmoothStepInOut(stepParams, x2) * size.y;
                    break;
                case Type.ExponentialEasing:
                    y1 = MathUtils.exponentialEasing(x1, exponentialEasingAmount) * size.y;
                    y2 = MathUtils.exponentialEasing(x2, exponentialEasingAmount) * size.y;
                    break;
                default:
                    break;
            }
            var from = new Vector3(x, y1);
            var to = new Vector3(x + step.x, y2);
            Gizmos.DrawLine(from, to);
            x += step.x;
        }
    }
}