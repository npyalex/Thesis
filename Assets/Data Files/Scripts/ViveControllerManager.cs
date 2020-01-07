using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveControllerManager : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean clickAction;
    public SteamVR_Action_Boolean clickHeld;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean grabHeld;

    public GameObject toggleObject;

    public bool GetClickDown()
    {
        return clickAction.GetStateDown(handType);
    }

    public bool GetClick()
    {
        return clickHeld.GetState(handType);
    }

    public bool GetGrabDown()
    {
        return grabAction.GetStateDown(handType);
    }

    public bool GetGrab()
    {
        return grabHeld.GetState(handType);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
