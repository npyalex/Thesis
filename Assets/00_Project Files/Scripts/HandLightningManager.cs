using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLightningManager : MonoBehaviour
{
    private Transform point;
    public GameObject lightningEnd, lightningStart, fireEffect;
    public float multiplier;
    // Start is called before the first frame update
    void Start()
    {
        //fireEffect.SetActive(false);
        //only cast rays against colliders in layer 15 (hands)
        //int layerMask = 1 << 15;
        ////invert it so we collider with everything except layer 15
        //layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(lightningStart.transform.position, lightningStart.transform.TransformDirection(Vector3.right)*multiplier, out hit, Mathf.Infinity))
            if (hit.collider.gameObject.name == "Contact Fingerbone" || hit.collider.gameObject.name == "Contact Palm")
            {
                lightningEnd.transform.position = hit.point;
            }
            else
            {
                Instantiate(fireEffect);
                //fireEffect.SetActive(true);
                lightningEnd.transform.position = hit.point;
                fireEffect.transform.position = hit.point;
            }
        Debug.Log("Hit object is " + hit.collider.gameObject.name);
    }
}
