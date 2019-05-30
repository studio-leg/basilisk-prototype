using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;

namespace Basilisk
{
    public class Hand : MonoBehaviour
    {
        public LayerMask remoteGrabLayerMask;
        public XRNode node = XRNode.LeftHand;
        public float force = 1f;

        LineRenderer lineRenderer;
        TrackedPoseDriver trackedPose;
        InputDevice inputDevice;

        bool raycastHit = false;
        Transform remoteTransform;
        Vector3 remoteGripOffset;
        GameObject remoteTarget;
        bool isGrippingRemote = false;
        float distanceToTarget = 0;

        void Start()
        {
            trackedPose = GetComponent<TrackedPoseDriver>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;

            var devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(node, devices);
            if (devices.Count == 1)
            {
                inputDevice = devices[0];
                Debug.Log(string.Format("Init Hand with device name '{0}' and role '{1}'",
                                        inputDevice.name, inputDevice.role.ToString()));
            }
            else if (devices.Count > 1)
            {
                Debug.LogWarning("! Found more than one hand for " + node);
            }

            remoteTarget = new GameObject("Remote Grab Target");
            //remoteTarget.hideFlags = HideFlags.HideInHierarchy;
            remoteTarget.transform.parent = transform;
        }
        
        void Update()
        {
            if (!isGrippingRemote)
            {
                RaycastHit raycastTarget;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycastTarget, Mathf.Infinity, remoteGrabLayerMask))
                {
                    raycastHit = true;
                    remoteTransform = raycastTarget.transform;
                    remoteGripOffset = raycastTarget.transform.position - raycastTarget.point;
                    if (lineRenderer)
                    {
                        lineRenderer.SetPosition(0, transform.position);
                        lineRenderer.SetPosition(1, remoteTransform.position);
                        lineRenderer.material.color = Color.red;
                        lineRenderer.material.SetColor("_UnlitColor", Color.red);
                    }
                }
                else
                {
                    raycastHit = false;
                    if (lineRenderer)
                    {
                        lineRenderer.SetPosition(0, transform.position);
                        lineRenderer.SetPosition(1, transform.TransformDirection(Vector3.forward) * 200);
                        lineRenderer.material.color = Color.white;
                        lineRenderer.material.SetColor("_UnlitColor", Color.white);
                    }
                }
            }
            
            UpdateInputGrip();
            if (isGrippingRemote && remoteTransform != null)
            {
                remoteTarget.transform.position = transform.position + (transform.TransformDirection(Vector3.forward) * distanceToTarget) + remoteGripOffset;
                var targetRigidBody = remoteTransform.GetComponent<Rigidbody>();
                if (targetRigidBody)
                {
                    var direction = remoteTarget.transform.position - targetRigidBody.position;
                    targetRigidBody.velocity = direction * force;
                }

                if (lineRenderer)
                {
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, remoteTransform.position);
                    lineRenderer.material.color = Color.yellow;
                    lineRenderer.material.SetColor("_UnlitColor", Color.green);
                }
            }
        }

        void UpdateInputGrip()
        {
            bool isGripButtonActive;
            inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out isGripButtonActive);

            //Debug.Log(string.Format("isGripButtonActive : {0}, isGrippingRemote : {1}", isGripButtonActive, isGrippingRemote));

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
                distanceToTarget = Vector3.Distance(transform.position, remoteTransform.position);
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