using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeLightInteraction : BasiliskInteraction
{
    public Transform headTransform;
    public SphereCollider lightCollider;
    [Range(0f, 10f)]
    public float activeThreshold = 1f;
    
    float timer = 0f;
    

    override protected void Update()
    {
        base.Update();
        if (isActive && headTransform && lightCollider)
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

    override public void Reset()
    {
        timer = percent = 0f;
    }
}
