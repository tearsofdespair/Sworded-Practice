using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 1) Off();
    }

    public void Off()
    {
        AudioSource audio = GetComponent<AudioSource>(); ;
        audio.enabled = !audio.enabled;
    }

}
