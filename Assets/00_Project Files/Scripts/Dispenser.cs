using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [Tooltip("Dispense the prefab if toDispense is not present in the scene")]
    public GameObject prefab, toDispense;
    private Vector3 startLocation;
    // Start is called before the first frame update
    void Start()
    {
        startLocation = new Vector3(toDispense.transform.position.x, toDispense.transform.position.y, toDispense.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (toDispense == null){
        // create a new copy of the prefab and assign it to toDispense
            toDispense = Instantiate(prefab);
            toDispense.transform.position = startLocation;
            toDispense.transform.parent = GameObject.Find("CastleScene").transform;
        }
    }
}
