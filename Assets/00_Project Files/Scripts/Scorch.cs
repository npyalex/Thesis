using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorch : MonoBehaviour
{
    public float lifetimeMultiplier;
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, 1 * lifetimeMultiplier);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
