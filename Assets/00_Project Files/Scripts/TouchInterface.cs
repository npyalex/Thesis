using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInterface : MonoBehaviour
{
    private float timer;
    public float timerMax;
    public bool hasRun;
    public GameObject toTrigger, switchOffOne, switchOffTwo;

    //Audio variables
    public AudioClip feedbackNoise;
    public GameObject cameraEyes;
    AudioSource audioSource;
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
        if(timer >= timerMax){
            toTrigger.SetActive(true);
            switchOffOne.SetActive(false);
            switchOffTwo.SetActive(false);
            hasRun = true;
            audioSource.PlayOneShot(feedbackNoise, 1.0f);
        }
    }

    //this function is called when the player touches the trigger object
   public void CountUp(){
        timer += Time.deltaTime;
    }
    //this function is called when the player removes their hand from the trigger object
   public void EndCount(){
        timer = 0.0f;
    }
}
