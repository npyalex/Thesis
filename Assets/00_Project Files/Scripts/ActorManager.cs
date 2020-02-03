using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    private int advance;
    public GameObject PaintTrigger, HideTrigger, CastleTrigger;
    public FadeInTimer userBlindfold, titleText;
    //timer variables
    private float timer;
    public float countdownTimerMax;
    private bool hasRun = false;

    // Start is called before the first frame update
    void Start()
    {
        advance = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Advance = " + advance);
        //Debug.Log("HasRun = " + hasRun);

        if ((Input.GetButton("Fire1") || Input.GetButton("Fire2")) && hasRun == false)
        {
            //Debug.Log("Grab Held");
            timer += Time.deltaTime;
            if (timer >= countdownTimerMax )
            {
                //Debug.Log("Trigger Activated");
                advance++;
                hasRun = true;
            }
        }
        if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
        {
            timer = 0.0f;
            hasRun = false;
        }

        if (advance == 1)
        {
            //Game Start State
            PaintTrigger.SetActive(false);
            HideTrigger.SetActive(false);
            CastleTrigger.SetActive(false);
        }
        else if (advance == 2)
        {
            //Debug.Log("Fade Stage");
            //Fade out blindfold
            userBlindfold.Fade();
        }
        //else if (advance == 3)
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
}
