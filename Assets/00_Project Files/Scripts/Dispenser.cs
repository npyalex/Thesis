using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [Tooltip("Dispense the prefab if toDispense is not present in the scene")]
    public GameObject prefab, toDispense;
    private Transform startLocation;
    // Start is called before the first frame update
    void Start()
    {
        startLocation = toDispense.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (toDispense == null){
        // create a new copy of the prefab and assign it to toDispense
            toDispense = Instantiate(prefab, startLocation);
        }
    }
}
