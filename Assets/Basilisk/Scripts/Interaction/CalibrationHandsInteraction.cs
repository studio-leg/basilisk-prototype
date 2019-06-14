using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class CalibrationHandsInteraction : BasiliskInteraction
{
    public Transform headTransform;
    public SphereCollider[] handColliders;
    [Range(0f, 10f)]
    public float activeThreshold = 5f;
    
    [Header("Hands")]
    [Tooltip("Visual effect whose property to set with the output SDF texture")]
    public VisualEffect[] handVFX;
    public string handVFXSpawnPropName = "SpawnRate";
    public SkinnedMeshRenderer leftHandMesh;
    public SkinnedMeshRenderer rightHandMesh;

    float timer = 0f;


    override protected void Update()
    {
        base.Update();
        if (isActive && headTransform && handColliders.Length == 2)
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

        for (int i = 0; i < handVFX.Length; i++)
        {
            handVFX[i].SetFloat(handVFXSpawnPropName, percent * 400);
        }
    }

    override public void Reset()
    {
        timer = percent = 0f;
    }
}
