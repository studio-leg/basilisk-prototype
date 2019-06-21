using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{

    public delegate void EnterAction();
    public event EnterAction OnEnter;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnEnter?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEnter?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        OnEnter?.Invoke();
    }
}
