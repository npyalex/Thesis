using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{
    //Public variables
    [Tooltip("Where the ray originates from. Should be the palm or wrist.")]
    public GameObject rayOrigin;
    [Tooltip("Multiplier to adjust the ray's direction if it originates in the wrong place. 1 is forward, -1 is backward etc.")]
    public float multiplier, timerMax;
    [Tooltip("Object to appear when an interactable object is highlighted")]
    public GameObject toAppear;
    //private variables
    private GameObject hitObject;
    private Light lt;
    private Color startColor, endColor;
    private bool hovering;
    private float timer = 0.0f;
    private float lerpAmount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        lt = toAppear.GetComponent<Light>();
        startColor = lt.color;
        endColor = Color.green;
        int layerMask = 1 << 2;        //Create a layerMask to ignore all objects on Layer 2: IgnoreRaycasts
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 origin = rayOrigin.transform.position;

        //Debug.Log("Hovering is" + hovering + this);
        if (Physics.Raycast(origin, (rayOrigin.transform.forward * multiplier), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(origin, (rayOrigin.transform.forward * multiplier) * 1000, Color.green);
            if (hit.collider.gameObject.CompareTag("Interactable"))
            {
                hovering = true;
                //Debug.Log("Object Hit");
                hitObject = hit.collider.gameObject;    //hold the object that is being targeted
                Debug.Log("Hit object is" + hitObject.name + "with" + this);
            }
            else
            {
                hovering = false;
                //Debug.DrawRay(origin, (rayOrigin.transform.forward * multiplier) * 1000, Color.white);
            }
        }

        if (hovering == true)
        {
            //Countdown();
            //Get the starting colour of the targeting light
            //Change the colour to green over the course of the countdown timer.
            toAppear.SetActive(true);               //turn on the light to indicate what is being targeted
            Debug.Log("The hovering statement is running");
            lerpAmount += Time.deltaTime;
            if (lerpAmount > timerMax)
            {
                lerpAmount = timerMax;
            }
            //Debug.Log("Lerp Amount is" + lerpAmount);
            float percent = lerpAmount / timerMax;
            lt.color = Color.Lerp(startColor, endColor, percent);
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                Activate();
            }
        }
        //else if (hovering == false);
        //{
        //    //Debug.Log("Hovering is"+ hovering);
        //    timer = 0.0f;
        //    lt.color = startColor;
        //    toAppear.SetActive(false);
        //}
    }

    void Activate()
    {
        Debug.Log("Activated!");
    }
}


