using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePaint : MonoBehaviour
{
    public GameObject paintParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ErasePaint()
    {
        foreach(Transform child in paintParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
