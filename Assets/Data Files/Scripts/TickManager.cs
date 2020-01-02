using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public GameObject tickObject1, tickObject2, tickObject3;
    public float tickLength = 10f;
    public int tick = 0;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        tickObject1.SetActive(false);
        tickObject2.SetActive(false);
        tickObject3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // manage the length of each tick
        timer += Time.deltaTime;
        if (timer > tickLength)
        {
            timer = 0;
            tick++;
        }
        // change available objects based on ticks
        if (tick == 0)
        {
            tickObject1.SetActive(false);
            tickObject2.SetActive(false);
            tickObject3.SetActive(false);
        }
        else if (tick == 1)
        {
            tickObject1.SetActive(true);
            tickObject2.SetActive(false);
            tickObject3.SetActive(false);
        }
        else if (tick == 2)
        {
            tickObject1.SetActive(false);
            tickObject2.SetActive(true);
            tickObject3.SetActive(false);
        }
        else if (tick >= 3)
        {
            tickObject1.SetActive(false);
            tickObject2.SetActive(false);
            tickObject3.SetActive(true);
        }

    }
}
