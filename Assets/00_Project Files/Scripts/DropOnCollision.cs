using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnCollision : MonoBehaviour
{
    private Rigidbody body;
    private bool go;
    // Start is called before the first frame update
    void Start()
    {
        go = false;
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (go == true)
        {
            body.useGravity = true;
            body.isKinematic = false;
        }
    }

    void OnCollisionEnter()
    {
        go = true;
    }
}
