using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeChildren : MonoBehaviour
{
    private Color startColor;
    private Color currColor;
    private Color endColor;
    private bool shouldFade = false;
    private float startTime;
    private float endTime;
    public float seconds = 5.0f;
    [Tooltip("1 = opaque, 0 = transparent")]
    public int alpha;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        startColor = gameObject.GetComponent<Renderer>().material.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
        for (int childIndex = 0; childIndex < gameObject.transform.childCount; childIndex++)
        {
            Transform child = gameObject.transform.GetChild(childIndex);

            child.gameObject.AddComponent<FadeChildren>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        startTime = Time.time;
        endTime = startTime + seconds;
        Fade();
    }

    void OnEnable()
    {
        shouldFade = true;
    }

    void Fade()
    {
        if (shouldFade == true)
        {
            t = Time.time / endTime;
            currColor = Color.Lerp(startColor, endColor, t);
            gameObject.GetComponent<Renderer>().material.color = currColor;
            if (currColor == endColor)
            {
                shouldFade = false;
                startTime = 0.0f;
                endTime = 0.0f;
                t = 0.0f;
            }
        }
    }
}
