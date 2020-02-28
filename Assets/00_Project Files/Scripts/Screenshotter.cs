using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    private string dateTime;
    private string fileName;
    private Renderer rend;
    private Texture text;

    public GameObject polaroid;
    // Start is called before the first frame update
    void Start()
    {
        rend = polaroid.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
       dateTime = System.DateTime.Now.ToString();
       fileName = dateTime.Replace(" ","").Replace(":","");
       // take the screenshot when something happens
    }

    IEnumerator RecordFrame() //capture the frame as a texture
    {
        yield return new WaitForEndOfFrame();
        text = ScreenCapture.CaptureScreenshotAsTexture(ScreenCapture.StereoScreenCaptureMode.LeftEye);
        // do something with this texture
        rend.material.SetTexture("_MainTex", text);
    }

    public void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot("Screenshots/PunctumScreenshot_" + fileName +".png", ScreenCapture.StereoScreenCaptureMode.LeftEye);
        Debug.Log("Screenshot saved to Screenshots/PunctumScreenshot_" + fileName);
        StartCoroutine("RecordFrame");
    }
}
