using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;

namespace Basilisk
{
    public class BasiliskLeapHand : MonoBehaviour
    {
        RiggedHand riggedHand;
        public LayerMask remoteGrabLayerMask;
        public float force = 1f;
        public Transform rayCastTransform;
        public Vector3 rayCastDirection = Vector3.forward;
        public bool isActive = false;

        LineRenderer lineRenderer;

        public bool raycastHit = false;
        Transform remoteTransform;
        Vector3 remoteGripOffset;
        GameObject remoteTarget;
        public bool isGrippingRemote = false;
        float distanceToTarget = 0;

        void Start()
        {
            riggedHand = GetComponent<RiggedHand>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            
            remoteTarget = new GameObject("Remote Grab Target");
            //remoteTarget.hideFlags = HideFlags.HideInHierarchy;
            remoteTarget.transform.parent = rayCastTransform;
        }
        
        void Update()
        {
            if (!isActive)
            {
                return;
            }
            if (!isGrippingRemote)
            {
                RaycastHit raycastTarget;
                if (Physics.Raycast(rayCastTransform.position, rayCastTransform.TransformDirection(rayCastDirection), out raycastTarget, Mathf.Infinity, remoteGrabLayerMask))
                {
                    raycastHit = true;
                    remoteTransform = raycastTarget.transform;
                    remoteGripOffset = raycastTarget.transform.position - raycastTarget.point;
                    if (lineRenderer)
                    {
                        lineRenderer.SetPosition(0, rayCastTransform.position);
                        lineRenderer.SetPosition(1, raycastTarget.point);
                        lineRenderer.material.color = Color.red;
                        lineRenderer.material.SetColor("_UnlitColor", Color.red);
                    }
                }
                else
                {
                    raycastHit = false;
                    if (lineRenderer)
                    {
                        lineRenderer.SetPosition(0, rayCastTransform.position);
                        lineRenderer.SetPosition(1, rayCastTransform.TransformDirection(rayCastDirection) * 200);
                        lineRenderer.material.color = Color.white;
                        lineRenderer.material.SetColor("_UnlitColor", Color.white);
                    }
                }
            }
            
            UpdateInputGrip();
            if (isGrippingRemote && remoteTransform != null)
            {
                remoteTarget.transform.position = rayCastTransform.position + (rayCastTransform.TransformDirection(rayCastDirection) * distanceToTarget) + remoteGripOffset;
                var targetRigidBody = remoteTransform.GetComponent<Rigidbody>();
                if (targetRigidBody)
                {
                    var direction = remoteTarget.transform.position - targetRigidBody.position;
                    targetRigidBody.velocity = direction * force;
                }

                if (lineRenderer)
                {
                    lineRenderer.SetPosition(0, rayCastTransform.position);
                    lineRenderer.SetPosition(1, remoteTransform.position - remoteGripOffset);
                    lineRenderer.material.color = Color.yellow;
                    lineRenderer.material.SetColor("_UnlitColor", Color.green);
                }
            }
        }

        void UpdateInputGrip()
        {
            bool isGripButtonActive = (riggedHand.GetLeapHand().GrabStrength > 0.5f);
            
            if (isGripButtonActive && !isGrippingRemote)
            {
                StartGrip();
            }
            else if (!isGripButtonActive && isGrippingRemote)
            {
                StopGrip();
            }
        }

        void StartGrip()
        {
            if (raycastHit)
            {
                Debug.Log("Start Grip");
                isGrippingRemote = true;
                distanceToTarget = Vector3.Distance(rayCastTransform.position, remoteTransform.position);
                remoteTarget.transform.position = remoteTransform.position;
            }
        }
        void StopGrip()
        {
            isGrippingRemote = false;
            Debug.Log("Stop Grip");
            var targetRigidBody = remoteTransform.GetComponent<Rigidbody>();
            if (targetRigidBody)
            {
                targetRigidBody.velocity = Vector3.zero;
            }
        }

    }

}