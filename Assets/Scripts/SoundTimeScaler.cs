using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTimeScaler : MonoBehaviour
{
    AudioSource audio;
    float scale;
        void Start()
    {
        audio = GetComponent<AudioSource>();
        scale = audio.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        audio.pitch = scale * Time.timeScale;
    }
}
