using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenseOnActivate : MonoBehaviour
{
    public GameObject prefab;
    public float xThrust, yThrust, zThrust;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Dispense()
    {
        Instantiate(prefab);
        Rigidbody rb = prefab.GetComponent<Rigidbody>();
        prefab.transform.position = this.transform.position;
        rb.AddForce(xThrust, yThrust, zThrust, ForceMode.Impulse);
    }
}
