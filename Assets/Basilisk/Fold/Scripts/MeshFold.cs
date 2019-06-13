using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFold : MonoBehaviour
{
    
    [Header("Params")]
    public Vector3 offset;
    public float angle;
    public Color colour;
    public bool useHinge = true;
    
    private Vector3 foldLinePointA = new Vector3(-999f, 0, 0);
    private Vector3 foldLinePointB = new Vector3(999f, 0, 0);


    [Header("Animate")]
    public bool animationEnabled = false;
    public float animationSpeed = 1f;
    public float animationMaxAngle = -40f;
    private float animationOffset;

    [HideInInspector]
    public FoldableProp[] props;
    private BoxCollider boxCollider;
    private Vector3[] meshVertices;
    private bool[] meshMaskVertices;
    private HingeJoint hinge;
    private Rigidbody hingeRigidBody;
    private Vector3 hingeHomePosition;
    private GrabbableRemote grabbable;

    void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateFoldLine();
        if (animationEnabled)
        {
            angle = Mathf.PerlinNoise(animationOffset, Time.time * animationSpeed) * animationMaxAngle;
        }
    }

    public void Reset()
    {
        angle = 0;
        hingeRigidBody.velocity = Vector3.zero;
        hinge.transform.position = hingeHomePosition;
    }

    void UpdateFoldLine()
    {
        foldLinePointA = transform.TransformPoint(new Vector3(-99f, 0, 0));
        foldLinePointB = transform.TransformPoint(new Vector3(99f, 0, 0));
        if (hinge && useHinge)
        {
            angle = hinge.angle;
            foldLinePointA = hinge.transform.TransformPoint(hinge.anchor + new Vector3(-99f, 0, 0));
            foldLinePointB = hinge.transform.TransformPoint(hinge.anchor + new Vector3(99f, 0, 0));
        }

        
    }

    public void InitMesh(ref Vector3[] meshVertices)
    {
        meshMaskVertices = new bool[meshVertices.Length];
        for (int vertI = 0; vertI < meshVertices.Length; vertI++)
        {
            var vertex = meshVertices[vertI];
            meshMaskVertices[vertI] = ContainsPoint(vertex);
        }
    }

    public void FoldVertices(ref Vector3[] liveVertices)
    {
        for (int vertI = 0; vertI < liveVertices.Length; vertI++)
        {
            if (meshMaskVertices[vertI])
            {
                var liveVertex = liveVertices[vertI] + offset;
                liveVertex = MathUtils.RotatePointAroundLine(foldLinePointA, foldLinePointB, liveVertex, angle);
                liveVertices[vertI] = liveVertex;
            }
        }
    }

    public void FoldProps()
    {
        for (int propI = 0; propI < props.Length; propI++)
        {
            var prop = props[propI];
            var transform = prop.transform;
            MathUtils.RotateTransformAroundLine(foldLinePointA, foldLinePointB, ref transform, angle);
        }
    }

    public void Init()
    {
        hinge = GetComponentInChildren<HingeJoint>();
        hingeRigidBody = hinge.GetComponent<Rigidbody>();
        grabbable = hinge.GetComponent<GrabbableRemote>();
        
        hingeHomePosition = hinge.transform.position;
        boxCollider = GetComponent<BoxCollider>();
        animationOffset = Random.value;
        UpdateFoldLine();
        colour = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
    }

    public bool ContainsPoint(Vector3 Point)
    {
        if (!boxCollider) Init();
        Vector3 localPos = transform.InverseTransformPoint(Point);
        var bounds = (boxCollider.size / 2);
        if (Mathf.Abs(localPos.x - boxCollider.center.x) < bounds.x &&
            Mathf.Abs(localPos.y - boxCollider.center.y) < bounds.y &&
            Mathf.Abs(localPos.z - boxCollider.center.z) < bounds.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(foldLinePointA, foldLinePointB);
    }

}