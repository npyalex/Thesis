using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownAndScreenshot : MonoBehaviour
{
    public Screenshotter screenshotter;
    public AudioSource audioSource;
    public int countdownTime;
    public AudioClip tick, camClick;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeScreenshot()
    {
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown()
    {
        audioSource.PlayOneShot(tick, 1.0f);
        yield return new WaitForSeconds(countdownTime);
        screenshotter.TakeScreenshot();
        audioSource.PlayOneShot(camClick, 1.0f);
    }
}
