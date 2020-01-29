using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInterface : MonoBehaviour
{
    private float timer;
    public float timerMax;
    public GameObject toTrigger, switchOffOne, switchOffTwo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timer);
        if(timer >= timerMax){
            toTrigger.SetActive(true);
            switchOffOne.SetActive(false);
            switchOffTwo.SetActive(false);
        }
    }

   public void CountUp(){
        timer += Time.deltaTime;
    }

   public void EndCount(){
        timer = 0.0f;
    }
}
