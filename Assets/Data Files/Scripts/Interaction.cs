using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject toAppear;
    public GameObject rayOrigin;
    [Tooltip("Multiplier for the ray. 1 is forward, -1 is backward etc.")]
    public int multiplier;
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
        if (Physics.Raycast(origin, (rayOrigin.transform.up * multiplier), out hit) && hit.collider.gameObject.CompareTag("Interactable"))
        {
            //Vector3 appearHere = 
            toAppear.SetActive(true);
            toAppear.transform.position = hit.collider.gameObject.transform.position;
            Debug.Log("Object Highlighted");
            Debug.DrawRay(origin, (rayOrigin.transform.up * multiplier) * 1000, Color.green);
        }
        else
        {
            toAppear.SetActive(false);
            //Debug.Log("Highlighting Down");
            Debug.DrawRay(origin, (rayOrigin.transform.up * multiplier) * 1000, Color.white);
        }

    }
}
