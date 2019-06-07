using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationLightInteraction : MonoBehaviour
{
    public Transform headTransform;
    public SphereCollider lightCollider;
    [Range(0f, 1f)]
    public float percent = 0f;
    [Range(0f, 10f)]
    public float activeThreshold = 5f;
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
        if (headTransform && lightCollider)
        {
            Ray ray = new Ray(headTransform.position, headTransform.forward);
            RaycastHit hitInfo;
            var hit = lightCollider.Raycast(ray, out hitInfo, 999f);
            if (hit)
            {
                timer += Time.deltaTime;
                percent = Mathf.Clamp01(timer / activeThreshold);
            }
        }
    }

    public void Reset()
    {
        timer = percent = 0f;
    }
}
