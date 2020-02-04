using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    private int advance;
    public GameObject paintTrigger, hideTrigger, castleTrigger, titleText, paintSceneObject, hideSceneObject, castleSceneObject;
    public FadeInTimer userBlindfold;
    public TextFade wordOne, wordTwo, wordThree;
    public TouchInterface hideScene, paintScene, castleScene;

    //timer variables
    private float timer;
    public float countdownTimerMax;
    private bool hasRun = false;

    // Start is called before the first frame update
    void Start()
    {
        advance = 1;
        paintScene = paintTrigger.GetComponent<TouchInterface>();
        hideScene = hideTrigger.GetComponent<TouchInterface>();
        castleScene = castleTrigger.GetComponent<TouchInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Advance = " + advance);
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
            paintTrigger.SetActive(false);
            hideTrigger.SetActive(false);
            castleTrigger.SetActive(false);
        }
        else if (advance == 2)
        {
            //Remove use blindfold
            userBlindfold.Fade();
        }
        else if (advance == 3)
        {
            //Fade out title text
            wordOne.Fade();
            wordTwo.Fade();
            wordThree.Fade();
        }
        else if (advance == 4)
        {
            //Hub State
            paintTrigger.SetActive(true);
            hideTrigger.SetActive(true);
            castleTrigger.SetActive(true);
            titleText.SetActive(false);

        }
        else if (advance == 5)
        {
            //turn all scenes off; lets the actor exit a scene and return to the hub.
            paintSceneObject.SetActive(false);
            hideSceneObject.SetActive(false);
            castleSceneObject.SetActive(false);
        }
        else if (advance == 6)
        {
            //turn all scenes off; lets the actor exit a scene and return to the hub.
            paintSceneObject.SetActive(false);
            hideSceneObject.SetActive(false);
            castleSceneObject.SetActive(false);
        }

        //if a scene has run, turn its trigger off.
        if (hideScene.hasRun == true)
        {
            hideTrigger.SetActive(false);
        }
        if (paintScene.hasRun == true)
        {
            paintTrigger.SetActive(false);
        }
        if (castleScene.hasRun == true)
        {
            castleTrigger.SetActive(false);
        }
    }

    //public void StartHub()
    //{
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