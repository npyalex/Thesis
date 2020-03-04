using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartTouchInterface : MonoBehaviour
{
    private float timer;
    public float timerMax;
    public bool hasRun;
    public GameObject switchOffOne, switchOffTwo, switchOffThree, switchOffFour;

    //Audio variables
    public AudioClip feedbackNoise;
    public GameObject cameraEyes;
    AudioSource audioSource;

    public ActorManager actorManager;
    // Start is called before the first frame update
    void Start()
    {
        hasRun = false;
        audioSource = cameraEyes.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timer);
        if (timer >= timerMax)
        {
            switchOffOne.SetActive(false);
            switchOffTwo.SetActive(false);
            switchOffThree.SetActive(false);
            switchOffFour.SetActive(false);
            actorManager.Advance();
            hasRun = true;
            audioSource.PlayOneShot(feedbackNoise, 1.0f);
        }
    }

    //this function is called when the player touches the trigger object
    public void CountUp()
    {
        timer += Time.deltaTime;
    }
    //this function is called when the player removes their hand from the trigger object
    public void EndCount()
    {
        timer = 0.0f;
    }
}
