using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterThrow : MonoBehaviour
{
    private Vector3 startPosition, currPosition;
    [Tooltip("Destroy this object if it is moved this far from its starting location.")]
    public float destroyDistance;
    [Tooltip("Wait this long in seconds before destroying")]
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currPosition = this.transform.position;
        float dist = Vector3.Distance(currPosition, startPosition);
        if (dist >= destroyDistance){
            Destroy(gameObject, time);
        }
    }
}
