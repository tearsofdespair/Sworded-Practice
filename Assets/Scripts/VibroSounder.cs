using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibroSounder : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interface(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }
    public void Soft(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }
}
