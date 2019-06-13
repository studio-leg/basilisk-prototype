using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableRemote : MonoBehaviour
{

    private Material hingeMaterial;
    Color emission = Color.black;
    float emissionIntensity = 1f;
    public bool isInPlace = false;

    void Start()
    {
        var hingRenderer = GetComponent<MeshRenderer>();
        hingeMaterial = hingRenderer.material;
    }

    void Update()
    {

        if (isInPlace)
        {
            emission = Color.Lerp(emission, Color.cyan, Time.deltaTime * 10);
            emissionIntensity = Mathf.Lerp(emissionIntensity, 50f, Time.deltaTime * 5);
        }
        else
        {
            emission = Color.Lerp(emission, Color.black, Time.deltaTime * 5);
            emissionIntensity = Mathf.Lerp(emissionIntensity, 1f, Time.deltaTime);
        }
        hingeMaterial.SetColor("_EmissiveColor", emission * emissionIntensity);
        //hingeMaterial.SetFloat("_EmissiveIntensity", emissionIntensity);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Monolith collision");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FoldTarget>())
        {
            Debug.Log("Monolith trigger");
            isInPlace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FoldTarget>())
        {
            Debug.Log("Monolith trigger Exit");
            isInPlace = false;
        }
    }

}
