using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldActivation : MonoBehaviour
{
    
    public bool isInPlace = false;
    
    public void Reset()
    {
        isInPlace = false;
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
