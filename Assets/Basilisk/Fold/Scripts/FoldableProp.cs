using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldableProp : MonoBehaviour
{
    bool isInit = false;
    private Vector3 basePosition;
    private Quaternion baseRotation;

    public Vector3 BasePosition
    {
        get
        {
            if (!isInit)
            {
                Init();
            }
            return basePosition;
        }
        set => basePosition = value;
    }

    public Quaternion BaseRotation { get => baseRotation; set => baseRotation = value; }

    private void Awake()
    {
        if (!isInit)
        {
            Init();
        }
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    void Init()
    {
        isInit = true;
        basePosition = transform.position;
        baseRotation = transform.rotation;
    }

    public void Fold(Vector3 position)
    {
        transform.position = position;
    }
    
}
