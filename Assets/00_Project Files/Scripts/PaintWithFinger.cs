using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintWithFinger : MonoBehaviour
{
    public GameObject paintSource, paintParent;
    private Material lMat;
    public Material blueMat, yellowMat, redMat;
    private LineRenderer currLine;
    private int numClicks = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartPaint()
    {
        if (lMat != null)
        {
            GameObject go = new GameObject();
            //All paint is held under the Paint Parent GameObject.
            go.transform.parent = paintParent.transform;
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            currLine = go.AddComponent<LineRenderer>();
            currLine.material = lMat;
            currLine.SetWidth(.01f, .01f);
            numClicks = 0;
        }
    }

    public void ContinuePaint()
    {
        if (lMat != null)
        {
            currLine.SetVertexCount(numClicks + 1);
            currLine.SetPosition(numClicks, paintSource.transform.position);
            numClicks++;
        }
    }

    //if this hand is left, change paint / if this hand is right, change paint. have two separate scripts, one for each hand
    public void ChangeToBluePaint()
    {
        {
            lMat = blueMat;
        }
    }

    public void ChangeToRedPaint()
    {
        {
            lMat = redMat;
        }
    }

    public void ChangeToYellowPaint()
    {
        {
            lMat = yellowMat;
        }
    }
}