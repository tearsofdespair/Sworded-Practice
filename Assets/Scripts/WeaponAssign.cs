using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssign : MonoBehaviour
{
    public int test=100;
    public bool bot;
    float r=0.6f;
    public GameObject[] Weapon;
    public Transform[] Hand;
    public Transform[] Dagger;
    public BotMovement movement;
    public PlayerStats playerStats;
    
    public BalanceSheet balance;
    void Start()
    {
        if (bot&&PlayerPrefs.GetInt("Internet") == 0)
        {
            playerStats.Damage *= 1.5f;
            movement.speed *= 1.2f;
        }
        int weapon=0;
        if (bot)
        {
            if (Random.value > r) weapon = 0;
            else
            {
                if (Random.value > r) weapon = 1;
                else
                {
                    if (Random.value > r) weapon = 2;
                    else
                    {
                        if (Random.value > r) weapon = 3;
                        else
                        {
                            if (Random.value > r) weapon = 4;
                            else
                            {
                                if (Random.value > r) weapon = 5;
                            }
                        }
                    }
                }
            }

        }
        else weapon = PlayerPrefs.GetInt("CWeapon");
        if (test < 100) weapon = test;
        movement.rotspeed *= balance.weapon[weapon].speed;
        playerStats.Damage *= balance.weapon[weapon].damage;
        movement.Sword = Weapon[weapon].transform;
        if (weapon!=2)
        {
            Weapon[weapon].SetActive(true);
            Hand[0].parent = Weapon[weapon].transform;
            Hand[1].parent = Weapon[weapon].transform;
        }
        else
        {
            Dagger[0].gameObject.SetActive(true);
            Dagger[1].gameObject.SetActive(true);
            Hand[0].parent = Dagger[0];
            Hand[1].parent = Dagger[1];
        }
        
    }
}
