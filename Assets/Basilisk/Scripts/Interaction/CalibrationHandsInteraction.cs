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
    public VisualEffect leftHandVFX;
    public VisualEffect rightHandVFX;
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

            var handSmoothness = MathUtils.Map(percent, 0f, 1f, 1f, 0.6f);
            var handRefaction = MathUtils.Map(percent, 0f, 1f, 1f, 1.02f);
            var handThickness = MathUtils.Map(percent, 0f, 1f, 0f, 0.4f);
            leftHandMesh.material.SetFloat("_Smoothness", handSmoothness);
            leftHandMesh.material.SetFloat("_Ior", handRefaction);
            leftHandMesh.material.SetFloat("_Thickness", handThickness);
            rightHandMesh.material.SetFloat("_Smoothness", handSmoothness);
            rightHandMesh.material.SetFloat("_Ior", handRefaction);
            rightHandMesh.material.SetFloat("_Thickness", handThickness);

        }

        if (leftHandMesh.gameObject.activeInHierarchy)
        {
            leftHandVFX.SetFloat(handVFXSpawnPropName, percent * 400);
        }
        else
        {
            leftHandVFX.SetFloat(handVFXSpawnPropName, 0);
        }

        if (rightHandMesh.gameObject.activeInHierarchy)
        {
            rightHandVFX.SetFloat(handVFXSpawnPropName, percent * 400);
        }
        else
        {
            rightHandVFX.SetFloat(handVFXSpawnPropName, 0);
        }

    }

    override public void Reset()
    {
        timer = percent = 0f;
    }
}
