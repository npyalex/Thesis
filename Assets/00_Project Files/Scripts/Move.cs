using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float xSpeed, ySpeed, zSpeed, xAngle, yAngle, zAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime);
        transform.Rotate(new Vector3(xAngle, yAngle, zAngle) * Time.deltaTime);
    }
}
