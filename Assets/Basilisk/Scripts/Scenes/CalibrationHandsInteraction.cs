using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationHandsInteraction : MonoBehaviour
{
    public Transform headTransform;
    public SphereCollider[] handColliders;
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
        if (headTransform && handColliders.Length == 2)
        {
            Ray ray = new Ray(headTransform.position, headTransform.forward);
            RaycastHit hitInfo;
            var hitLeft = handColliders[0].Raycast(ray, out hitInfo, 999f);
            var hitRight = handColliders[1].Raycast(ray, out hitInfo, 999f);
            if (hitLeft || hitRight)
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
