using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject original;
    public KeyCode spawnKey = KeyCode.Space;
    public Bounds spawnBounds = new Bounds(Vector3.zero, Vector3.zero);
    public Vector3 spawnRotation;
    public int maxCount;

    void Start()
    {

    }
    
    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnOne();
        }
    }

    public GameObject SpawnOne()
    {
        if (maxCount > 0)
        {
            if (transform.childCount >= maxCount)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
        return Instantiate(original, GetPosition(), Quaternion.Euler(spawnRotation), transform);
    }

    private Vector3 GetPosition()
    {
        var extents = spawnBounds.extents;
        return spawnBounds.center + new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(spawnBounds.center, spawnBounds.size);
    }
}