using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRotate : MonoBehaviour
{

    public Vector3 angleDelta = Vector3.zero;


    void Start()
    {

    }
    
    void Update()
    {
        transform.Rotate(angleDelta);
    }
}