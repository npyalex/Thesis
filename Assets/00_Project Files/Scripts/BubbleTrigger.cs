using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrigger : MonoBehaviour
{
    public GameObject scene, light;
    public bool sceneActive, lightOn, destroyOnActivate;
    // Start is called before the first frame update
    void Start()
    {
        sceneActive = false;
        scene.SetActive(false);
        lightOn = false;
        light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleScene()
    {
        if (sceneActive == false)
        {
            scene.SetActive(true);
            sceneActive = true;
        }
        else if (sceneActive == true)
        {
            scene.SetActive(false);
            sceneActive = false;
        }

        if (destroyOnActivate == true)
        {
            Destroy(gameObject);
        }
    }

    public void LightSwitch()
    {
        if (lightOn == false)
        {
            light.SetActive(true);
            lightOn = true;
        }
        else if (lightOn == true)
        {
            light.SetActive(false);
            lightOn = false;
        }
    }
}
