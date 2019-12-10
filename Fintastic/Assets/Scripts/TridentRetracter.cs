using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TridentRetracter : MonoBehaviour
{

    public SteamVR_Action_Boolean grabPinch; //Grab Pinch is the trigger, select from inspecter
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;//which controller
                                                                         // Use this for initialization

    private Vector3 initPosition;
    private Rigidbody rigidbody;
    private void Awake()
    {
        initPosition = this.transform.position;
        rigidbody = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            this.transform.position = initPosition;
            this.rigidbody.velocity = Vector3.zero;
            this.rigidbody.angularVelocity = Vector3.zero;
        }
    }

    void OnEnable()
    {
        if (grabPinch != null)
        {
            Vector3 tridentPosition = GameObject.FindGameObjectWithTag("Trident").transform.position;
            grabPinch.AddOnChangeListener(OnTriggerPressedOrReleased, inputSource);
            tridentPosition = new Vector3(0, 0, 0);
        }
    }


    private void OnDisable()
    {
        if (grabPinch != null)
        {
            grabPinch.RemoveOnChangeListener(OnTriggerPressedOrReleased, inputSource);
        }
    }


    private void OnTriggerPressedOrReleased(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        Debug.Log("Trigger was pressed or released");
    }
}
