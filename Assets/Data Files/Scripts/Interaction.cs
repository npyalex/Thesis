using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject toAppear;
    public GameObject rayOrigin;
    private GameObject grabbedObject;
    private bool highlighted;
    // Start is called before the first frame update
    void Start()
    {
        toAppear.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 origin = rayOrigin.transform.position;
        //Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(origin, (rayOrigin.transform.up*-1), out hit) && hit.collider.gameObject.CompareTag("Interactable"))
        {
            //Vector3 appearHere = 
            toAppear.SetActive(true);
            toAppear.transform.position = hit.collider.gameObject.transform.position;
            highlighted = true;
            Debug.DrawRay(origin, (rayOrigin.transform.up * -1), Color.yellow);
        }
        else
        {
            toAppear.SetActive(false);
            highlighted = false;
            Debug.DrawRay(origin, (rayOrigin.transform.up * -1) * 1000, Color.white);
        }

    }

    public void Grab()
    {
        Debug.Log("Grasp Detected");
        if (highlighted == true)
        {
            RaycastHit hit;
            Vector3 origin = rayOrigin.transform.position;
            //Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(origin, (rayOrigin.transform.up * -1), out hit) && hit.collider.gameObject.CompareTag("Interactable"))
            {
                Vector3 newPosition = hit.point;
                grabbedObject = hit.collider.gameObject;
                //grabbedObject.rigidbody.freezeRotation = true;
                grabbedObject.transform.position = newPosition;
            }

        }
    }
}
