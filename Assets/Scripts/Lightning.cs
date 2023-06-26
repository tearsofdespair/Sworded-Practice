using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    float t;
    Collider collider;
    AudioSource audio;
    public GameObject Fx;
    private void Start()
    {
        collider = GetComponent<Collider>();
        audio = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Unit"&& other.transform!=transform.parent)
        {
            other.GetComponent<BotMovement>().playerStats.AddHP(-25);
            Instantiate(Fx, other.transform.position, Quaternion.identity);
            if (!audio.isPlaying) audio.Play();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (t<2)
        {
            if (t==0) collider.enabled = false;
            t += Time.fixedDeltaTime;
        }
        else
        {
            collider.enabled = true;
            t = 0;
        }
    }
}
