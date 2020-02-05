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

    public bool GetClickDown()
    {
        return clickAction.GetStateDown(handType);
        Debug.Log("Click Down");
    }

    public bool GetClick()
    {
        return clickHeld.GetState(handType);
        Debug.Log("Click");
    }

    public bool GetGrabDown()
    {
        return grabAction.GetStateDown(handType);
        Debug.Log("Grab Down");
    }

    public bool GetGrab()
    {
        return grabHeld.GetState(handType);
        //Debug.Log("Grab Held");
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
