using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    public GameObject HitFX;
    public BotMovement bm;
    public GameObject fx;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            if (!bm.bot) bm.Defense();

            //Invoke("SetTag", 1.5f);
            float magn = collision.relativeVelocity.magnitude;

            if (magn > 20) Instantiate(HitFX, collision.contacts[0].point + Vector3.up * 0.5f, Quaternion.identity);
        }
    }
}

