using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseOnAwake : MonoBehaviour
{
    public float thrust = 1.0f;
    private Rigidbody rb;
    private bool hasRun = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasRun == false)
        { 
            rb.AddForce(transform.up * thrust, ForceMode.Impulse);
            hasRun = true;
        }
    }
}
