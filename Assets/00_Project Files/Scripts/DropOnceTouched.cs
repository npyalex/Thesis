using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnceTouched : MonoBehaviour
{
    private Vector3 startLocation;
    private Vector3 currentLocation;
    private Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.position;
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentLocation = transform.position;
        if (currentLocation != startLocation)
        {
            body.useGravity = true;
            body.isKinematic = false;
        }
    }
}