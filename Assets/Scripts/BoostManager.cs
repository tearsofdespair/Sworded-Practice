using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    public GameObject[] BoostFX;
    public GameObject[] BoostUI;
    public BotMovement botMovement;
    public PlayerStats PlayerStats;
    public float Damage;
    public float Speed;
    public float time = 5;
    float t;
    bool active;
    public AudioSource audio;
    public AudioClip clip;
    private void OnEnable()
    {
        Damage = PlayerStats.Damage;
        Speed = botMovement.speed;
        botMovement.damagable = true;
        if (active)
        Deactivate();
    }
    public void ActivateBoost(int boost)
    {
        Deactivate();
        BoostFX[boost].SetActive(true);
        BoostUI[boost].SetActive(true);
        if (boost == 0) PlayerStats.Damage *= 2;
        if (boost == 1) botMovement.damagable = false;
        if (boost == 2) botMovement.speed *= 1.5f;
        active = true;
    }
    public void Deactivate()
    {
        for (int a = 0; a < BoostFX.Length; a++)
        {
            BoostFX[a].SetActive(false);
            BoostUI[a].SetActive(false);
        }
            t = 0;
        active = false;
        PlayerStats.Damage= Damage;
        botMovement.speed=Speed;
        botMovement.damagable = true;
        audio.PlayOneShot(clip);
    }
    private void FixedUpdate()
    {
        if (active)
        {
            if (t < time)
            {
                t += Time.fixedDeltaTime;

            }
            else
            {
                Deactivate();
            }
        }
    } 
}
