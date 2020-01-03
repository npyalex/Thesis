using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInTimer : MonoBehaviour
{
    public GameObject fadeObject;
    public float fadeDuration = 60.0f;
    [Tooltip("1 = opaque, 0 = transparent")]
    public int alpha;
    private float lerpAmount = 0.0f;
    Color startColor;
    Color endColor;

    // Start is called before the first frame update
    void Start()
    {
        //   objectToAppear.SetActive(false);
        startColor = fadeObject.GetComponent<Renderer>().material.color; //hold the color of the target object
        endColor = new Color(startColor.r, startColor.g, startColor.b, alpha); //hold the target alpha: between 0 and 1
    }

    // Update is called once per frame
    void Update()
    {
        //manage the fade in
        lerpAmount += Time.deltaTime;
        if (lerpAmount > fadeDuration)
        {
            lerpAmount = fadeDuration;
        }
        float percent = lerpAmount / fadeDuration;
        fadeObject.GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, percent);
    }

}
