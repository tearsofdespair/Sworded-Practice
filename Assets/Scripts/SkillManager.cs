using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public int skill;
    public BotMovement movement;
    public PlayerStats PlayerStats;
    public MeleeWeaponTrail[] Trail;
    public GameObject[] SkillFire;
    public GameObject Vampire;
    public GameObject Storm;
    public RotatingShield Shield;
    public DamageSounder whosh;
    public Transform[] Weapon;
    public bool bot;
    float t;
    void Start()
    {
        if (bot)
        {
            if (Random.value < (0.5 + 0.05 * PlayerPrefs.GetInt("Wins"))) skill = Random.Range(0, 3);
            else Destroy(this);
        }
        else skill = PlayerPrefs.GetInt("ActiveSkill");
        switch (skill)
        {
            case 3:
                for (int i = 0; i < Weapon.Length; i++)
                {
                    Weapon[i].localScale=1.5f*Vector3.one;
                }
                break;
            case 2:
                {
                    RotatingShield shield= Instantiate(Shield, transform.position, Quaternion.identity);
                    shield.target = transform;
                    for (int i = 0; i < Weapon.Length; i++)
                    {
                        Physics.IgnoreCollision(shield.collider, Weapon[i].GetComponent<Collider>());
                        //shield.other[i]= Weapon[i].GetComponent<Collider>();
                    }

                }
                break;
            case 4:
                {
                    PlayerStats.vampire = true;
                    Vampire.SetActive(true);
                }
                break;
            case 5:
                {
                    Storm.SetActive(true);
                }
                break;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(movement.rb.angularVelocity.y);
        switch (skill)
        {
            case 0:
                {
                    if (movement.damagable&& movement.rb.velocity!=Vector3.zero)
                    {
                        if (movement.rotspeedmod < 3f)
                        {
                            movement.rotspeedmod += Time.fixedDeltaTime * 1.2f;
                            movement.speedmod += Time.fixedDeltaTime * 0.1f;
                            if (movement.rotspeedmod > 3f)
                            {
                                movement.speedmod = 1.5f;
                                for (int i = 0; i < Trail.Length; i++)
                                {
                                    Trail[i].Emit = true;
                                }
                            }
                        }
                        else if (whosh.gameObject.activeSelf) whosh.PlayOuch();
                    }
                    else if (movement.rotspeedmod != 1f)
                    {
                        movement.rotspeedmod = 1f;
                        movement.speedmod = 1f;
                        for (int i = 0; i < Trail.Length; i++)
                        {
                            Trail[i].Emit = false;
                        }
                    }
                    break;
                }
            case 1:
                {
                    if (movement.damagable) 
                    {
                        if (t < 3)
                        {
                            t += Time.fixedDeltaTime;
                            if (t >= 3)
                            {
                                for (int i = 0; i < SkillFire.Length; i++)
                                {
                                    SkillFire[i].SetActive(true);
                                    
                                }
                                PlayerStats.damagemod = 2;
                            }
                        } 
                    }
                    else if (t != 0)
                    {
                        t = 0;
                        for (int i = 0; i < SkillFire.Length; i++)
                        {
                            SkillFire[i].SetActive(false);
                            
                        }
                        PlayerStats.damagemod = 1;  
                    }
                    break;
                }
        }
        //if (movement.rb.angularVelocity.y>50)

    }
}
