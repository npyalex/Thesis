using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ActorManager : MonoBehaviour
{
    //private int advance;
    //public GameObject PaintTrigger, HideTrigger, CastleTrigger;
    //public FadeInTimer userBlindfold, titleText;
    public ViveControllerManager controller;
    //timer variables
    //private float timer;
    //public float timerMax;
    //private bool hasRun = false;
    // Start is called before the first frame update
    void Start()
    {
        //advance = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (timer >= timerMax && hasRun == false)
        //{
        //    advance++;
        //    hasRun = true;
        //} else {
        //    hasRun = false;
        //}

        if (controller.GetGrab())
        {
            Debug.Log("Grab Held");
            //    timer += Time.deltaTime;
            //}
            //else
            //{
            //    timer = 0.0f;
            //}
        }
    }

    //    if (advance == 1)
    //    {
    //        //Game Start State
    //        PaintTrigger.SetActive(false);
    //        HideTrigger.SetActive(false);
    //        CastleTrigger.SetActive(false);
    //    }
    //    else if (advance == 2)
    //    {
    //        //Fade out blindfold
    //        userBlindfold.Fade();
    //    } else if (advance == 3)
    //    {
    //        //Hub State
    //        StartHub();
    //    } else if (advance == 4)
    //    {
    //        //Game state
    //    } else if (advance == 5)
    //    {
    //        //outro state
    //    } else if (advance == 6)
    //    {
    //        //end game state
    //    }
    //}

    //public void StartHub()
    //{
    //    PaintTrigger.SetActive(true);
    //    HideTrigger.SetActive(true);
    //    CastleTrigger.SetActive(true);
    //}

    //public void StartCastle()
    //{

    //}

    //public void StartHideNSeek()
    //{

    //}

    //public void StartPainting()
    //{

    //}
}
