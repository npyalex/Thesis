using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtRandomPoint : MonoBehaviour
{
    public GameObject prefab;
    public float minX, maxX, minZ, maxZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Spawn()
    {
        Vector3 position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        Instantiate(prefab, position, Quaternion.identity);
    }
}
