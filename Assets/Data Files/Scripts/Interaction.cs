using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject toAppear;
    private GameObject grabbedObject;
    private bool highlighted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Point()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        if (Physics.Raycast(transform.position, fwd, out hit)&&hit.collider.gameObject.CompareTag("Interactable"))
        {
            Vector3 appearHere = hit.collider.gameObject.transform.position;
            toAppear.SetActive(true);
            appearHere = toAppear.transform.position;
            highlighted = true;
            Debug.DrawRay(transform.position, fwd * hit.distance, Color.yellow);
        } else
        {
            toAppear.SetActive(false);
            highlighted = false;
            Debug.DrawRay(transform.position, fwd * 1000, Color.white);
        }
    }

    public void Grab()
    {
        if (highlighted == true)
        {
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, fwd, out hit) && hit.collider.gameObject.CompareTag("Interactable"))
            {
                Vector3 newPosition = hit.point;
                grabbedObject = hit.collider.gameObject;
                //grabbedObject.rigidbody.freezeRotation = true;
                grabbedObject.transform.position = newPosition;
            }

        }
    }
}
