using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationLightInteraction : BasiliskInteraction
{
    public Transform headTransform;
    public SphereCollider lightCollider;
    [Range(0f, 10f)]
    public float activeThreshold = 5f;
    
    [Header("Sun")]
    public Light mainLight;
    public Vector3 lightRotMin = new Vector3(-6f, -212f, 90f);
    public Vector3 lightRotMax = new Vector3(159.42f, -212f, 90f);
    public MeshRenderer calibrationLightMesh;

    float timer = 0f;
    

    override protected void Update()
    {
        base.Update();
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

        var rotation = Vector3.Lerp(lightRotMin, lightRotMax, percent);
        mainLight.transform.rotation = Quaternion.Euler(rotation);
        calibrationLightMesh.gameObject.SetActive(percent < 1);
    }

    override public void Reset()
    {
        timer = percent = 0f;
    }
}
