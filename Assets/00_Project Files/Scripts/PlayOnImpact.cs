using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayOnImpact : MonoBehaviour
{
    public AudioClip impact;
    public float cooldown;
    private bool ready;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ready = true;
    }

    void Update()
    {
        if (ready == false)
        {
            float timer = 0.0f;
            timer += Time.deltaTime;
            if (timer >= cooldown)
            {
                ready = true;
            }
        }
    }

    void OnCollisionEnter()
    {
        audioSource.PlayOneShot(impact, 1.0F);
        ready = false;
    }
}
