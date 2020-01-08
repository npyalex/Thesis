using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject key1, key2, key3, solved1, solved2, solved3, phase1Object;
    public GameObject rayOrigin;
    public FadeInTimer fadeInTimer;
    public LoadScene loadScene;
    [Tooltip("Multiplier for the ray. 1 is forward, -1 is backward etc.")]
    public int multiplier;
    //private Vector3 key1Location, key2Location, key3Location;
    private bool key1Solved, key2Solved, key3Solved;
    private int ticks = 0;
    private float timer = 0.0f;
    private float timerMax = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        key1Solved = false;
        key2Solved = false;
        key3Solved = false;
        //key1Location = key1.transform;
        //key2Location = key2.transform;
        //key3Location = key3.transform;
        key1.name = "Key1";
        key2.name = "Key2";
        key3.name = "Key3";
        //solved1.SetActive(false);
        //solved2.SetActive(false);
        //solved3.SetActive(false);
        phase1Object.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ticks == 1)
        {

        } else if (ticks == 2)
        {
            phase1Object.SetActive(true);
        } else if (ticks == 3)
        {
            //move on
            fadeInTimer.Fade();
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                loadScene.LoadSceneTwo();
            }
        }
    }

    public void Solve()
    {
        Debug.Log("Solve Initiated");
        RaycastHit hit;
        Vector3 origin = rayOrigin.transform.position;
        //Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(origin, (rayOrigin.transform.up * multiplier), out hit) && hit.collider.gameObject.CompareTag("Interactable"))
        {
            if(hit.collider.gameObject.name == "Key1")
            {
                //key1Solved = true;
                ticks++;
                key1.SetActive(false);
                solved1.SetActive(true);
            }
            else if (hit.collider.gameObject.name == "Key2")
            {
                //key2Solved = true;
                ticks++;
                key2.SetActive(false);
                solved2.SetActive(true);
            }
            else if (hit.collider.gameObject.name == "Key3")
            {
                //key3Solved = true;
                ticks++;
                key3.SetActive(false);
                solved3.SetActive(true);
            }

        }

    }

}
