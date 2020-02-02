using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerOnOff : MonoBehaviour
{
    public ViveControllerManager leftManager;
    public ViveControllerManager rightManager;
    public GameObject toggleObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(leftManager.GetGrabDown() || rightManager.GetGrabDown())
        {
            Debug.Log("Button Press Detected");
            if (toggleObject.activeSelf)
            {
                toggleObject.SetActive(false);
            } else 
            {
                toggleObject.SetActive(true);
            }
        }
    }
}
