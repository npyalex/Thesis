using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorch : MonoBehaviour
{
    public GameObject scorch;
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, 1*0.5f);
        Instantiate(scorch, transform.position, transform.rotation);
        FadeOutTimer fade = scorch.GetComponent<FadeOutTimer>();
        fade.Fade();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
