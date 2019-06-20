using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableRemote : MonoBehaviour
{

    private Material hingeMaterial;
    Color emission = Color.black;
    float emissionIntensity = 1f;
    public bool isInPlace = false;
    public bool ForceHome = false;
    private Vector3 homePosition;
    private Rigidbody rigidBody;
    public float force = 10f;
    public bool active = false;

    void Start()
    {
        homePosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
        var hingRenderer = GetComponent<MeshRenderer>();
        hingeMaterial = hingRenderer.material;
    }

    void Update()
    {
        if (ForceHome)
        {
            var direction = homePosition - rigidBody.position;
            rigidBody.velocity = direction * force;
            if (Vector3.Distance(rigidBody.position, homePosition) < 0.01f)
            {
                ForceHome = false;
            }
        }
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
    }

    public void Reset()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.position = homePosition;
    }

    public void MoveTowards(Vector3 target, float strength)
    {
        if (active)
        {
            var direction = target - rigidBody.position;
            rigidBody.velocity = direction * force;
        }
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
