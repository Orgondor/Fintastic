using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VR_Behaviour_Pose : SteamVR_Behaviour_Pose
{
    public float maxSpeed = 1.0f;
    public float maxRotation = 360.0f;
    public float fadeDistance = 0.2f;
    public float fadeRotation = 30.0f;
    public GameObject ghostModelPrefab;
    private GameObject ghostModelInstance = null;
    private Material handMaterial = null;
    private float maxEdgeFeather = 0.0f;

    protected override void Start()
    {
        if (poseAction == null)
        {
            Debug.LogError("<b>[SteamVR]</b> No pose action set for this component");
            return;
        }

        CheckDeviceIndex();

        if (origin == null)
            origin = this.transform.parent;

        ghostModelInstance = GameObject.Instantiate(ghostModelPrefab);
        ghostModelInstance.layer = gameObject.layer;
        ghostModelInstance.tag = gameObject.tag;
        ghostModelInstance.transform.parent = origin;
        ghostModelInstance.transform.localPosition = Vector3.zero;
        ghostModelInstance.transform.localRotation = Quaternion.identity;
        ghostModelInstance.transform.localScale = ghostModelPrefab.transform.localScale;

        handMaterial = ghostModelInstance.GetComponentInChildren<Renderer>().material;
        maxEdgeFeather = handMaterial.GetFloat("_EdgeFeather");
    }

    protected override void UpdateTransform()
    {
        CheckDeviceIndex();

        if (origin != null)
        {
            // Interpolate position of "real" hand
            Vector3 newPos = origin.transform.TransformPoint(poseAction[inputSource].localPosition);
            float maxDist = maxSpeed * Time.deltaTime;
            float curDist = 0.0f;
            if ((newPos - transform.position).sqrMagnitude > maxDist * maxDist)
            {
                transform.position += (newPos - transform.position).normalized * maxDist;
                curDist = (newPos - transform.position).magnitude;
            }
            else
            {
                transform.position = newPos;
            }

            // Interpolate rotation of "real" hand
            Quaternion newRot = origin.rotation * poseAction[inputSource].localRotation;
            float maxRot = maxRotation * Time.deltaTime;
            float angle = Quaternion.Angle(transform.rotation, newRot);
            float curRot = 0.0f;
            if (angle > maxRot)
            {
                float t = maxRot / angle;
                transform.rotation = Quaternion.Slerp(transform.rotation, newRot, t);
                curRot = angle - maxRot;
            }
            else
            {
                transform.rotation = newRot;
            }

            // Update the ghost hand
            ghostModelInstance.transform.position = newPos;
            ghostModelInstance.transform.rotation = newRot;

            if (curDist > 0.0f || curRot > 0.0f)
            {
                ghostModelInstance.SetActive(true);
                float fade = Mathf.Min(Mathf.Max(curDist / fadeDistance, curRot / fadeRotation), 1.0f);
                handMaterial.SetFloat("_EdgeFeather", maxEdgeFeather * fade);
            }
            else
            {
                ghostModelInstance.SetActive(false);
            }
        }
        else
        {
            transform.localPosition = poseAction[inputSource].localPosition;
            transform.localRotation = poseAction[inputSource].localRotation;
        }
    }
}
