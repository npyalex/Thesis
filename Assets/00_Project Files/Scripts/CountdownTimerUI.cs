using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimerUI : MonoBehaviour
{
    public float timeLeft = 3.0f;
    public bool stop = true;

    private float seconds;

    private Text text;

    public void startTimer(float from)
    {
        stop = false;
        timeLeft = from;
        GameObject go;
        GameObject myText;
        go = new GameObject();
        go.name = "TempUI";
        go.AddComponent<Canvas>();
        Canvas myCanvas = go.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myText = new GameObject();
        myText.transform.parent = go.transform;
        text = myText.AddComponent<Text>();
        Update();
        StartCoroutine(updateCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (stop) return;
        timeLeft -= Time.deltaTime;
        seconds = timeLeft % 60;
    }

    private IEnumerator updateCoroutine()
    {
        while(!stop)
        {
            text.text = string.Format("{0:0}:{1:00}", seconds);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
