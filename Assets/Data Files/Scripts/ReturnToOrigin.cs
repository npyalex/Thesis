using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToOrigin : MonoBehaviour
{
    private Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Returner"))
        {
            this.transform.position = origin;
        }
    }
}
