using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class NetRetracter : MonoBehaviour
{

    public SteamVR_Action_Boolean grabPinch; //Grab Pinch is the trigger, select from inspecter
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;//which controller
                                                                         // Use this for initialization

    void OnEnable()
    {
        if (grabPinch != null)
        {
            Vector3 netPosition = GameObject.FindGameObjectWithTag("Net").transform.position;
            grabPinch.AddOnChangeListener(OnTriggerPressedOrReleased, inputSource);
            netPosition = new Vector3(0, 0, 0);
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
