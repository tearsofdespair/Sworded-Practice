using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    //public VariableJoystick joystick;

    [Header ("Player")]
    public VariableJoystick joystick;

    [Header("Else")]
    public MeshRenderer meshRenderer;
    public bool canmove;
    //public bool moving;
    public bool bot=true;
    public bool damagable=true;
    public BoostManager Boost;
    public HitColor BigHitFX;
    public HitColor HitFX;
    public GameObject BotHitFX;
    public PlayerStats playerStats;
    public Rigidbody rb;
    public float speed = 1;
    public float speedmod = 1;
    public float rotspeed = 20;
    public float rotspeedmod = 1;
    public Transform Center;
    public Transform Sword;
    public Transform Target;
    bool right;
    public int Behavior;
    public float rotateTimer;
    float crotateTimer;
    public float behTimer;
    float cbehTimer;
    public PlayersList PlayersList;
    public float wall;
    GameUI gameUI;
    int AIDifficulty;
    float hittimer=0.5f;
    float hitt;

    void Start()
    {
        AIDifficulty = PlayerPrefs.GetInt("Wins");
        //PlayersList = PlayersList.instance;
        gameUI = GameUI.instance;
        GetComponent<PlayerIndex>().playerStats = playerStats;
        RandomBehavior();
        wall = playerStats.balance.MapSize-1;
        //Debug.Log(30 % 1000);
        if (bot)
        {
            speed *= Mathf.Clamp(0.8f + 0.05f * AIDifficulty, 0.8f, 1.2f);
            rotspeed *= Mathf.Clamp(0.5f+ 0.05f * AIDifficulty, 0.5f,1.1f);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Boost")
        {
            Boost.ActivateBoost(collision.gameObject.GetComponent<BoostIndex>().index);
            collision.gameObject.GetComponent<BoostIndex>().GetBoost();
        }
        if (collision.gameObject.tag == "Coin"&& !bot)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 5);
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Points"&& canmove)
        {
            if (bot) playerStats.AddPoints(50);
            else playerStats.AddPoints(25);
            collision.gameObject.SetActive(false);
            if (!bot)
            {
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 5);
                playerStats.CoinAnim.gameObject.SetActive(true);
                playerStats.CoinAnim.Play();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Unit")
        {
            rb.AddForce((transform.position- collision.transform.position).normalized * 20 * speed);
        }
        if (collision.gameObject.tag=="Sword"&& collision.transform.parent!= transform&& canmove)
        {
            float magn = collision.relativeVelocity.magnitude;
            if (magn > 10)
            {
                
                rb.AddForce((Vector3.up - collision.transform.parent.position + transform.position).normalized * magn *1.2f* speed);
                if (damagable)
                {
                    if (bot && Random.value > 0.5f) Behavior = 3;
                    if (!bot)
                    {
                        gameUI.ShakeCam(magn / 20, 1f);
                        gameUI.DamageVignette.SetActive(true);
                        magn /= Mathf.Clamp((2f-AIDifficulty*0.25f),1,2);
                    }
                    if (playerStats.botface) Instantiate(BotHitFX, collision.contacts[0].point + Vector3.up * 0.5f, Quaternion.identity);
                    else if (meshRenderer.isVisible)
                    {
                        
                        if (magn > 35)
                        {
                            HitColor hit=Instantiate(BigHitFX, collision.contacts[0].point + Vector3.up * 0.5f, Quaternion.identity);
                            Color color= meshRenderer.material.color;
                            Color.RGBToHSV(color,out float h, out float u, out float e);
                            hit.color = Color.HSVToRGB(h, u, 1);
                        }
                        else
                        {
                            HitColor hit = Instantiate(HitFX, collision.contacts[0].point + Vector3.up * 0.5f, Quaternion.identity);
                            Color color = meshRenderer.material.color;
                            Color.RGBToHSV(color, out float h, out float u, out float e);
                            hit.color = Color.HSVToRGB(h, u, 1);
                        }
                    }
                    playerStats.lastHit = collision.transform.parent.GetComponent<PlayerIndex>().index;
                    playerStats.AddHP(-(int)magn);
                }
                Defense();

            }
        }
    }
    void Wall()
    {
        if (transform.position.x > wall ||
            transform.position.x < -wall ||
            transform.position.z > wall ||
            transform.position.z < -wall)
        {
            Target = Center;
            Behavior = 1;
            behTimer = 3;
            cbehTimer = 0;
        }
    }
    public void EnemyBehavior()
    {
        switch (Behavior)
        {
            //Wait
            case 0:
                //rb.AddForce((-transform.position + Target.position).normalized * Time.deltaTime * 100 * speed);
                
                break;
            
            //Attack
            case 1:
                rb.AddForce((-transform.position + Target.position).normalized * Time.deltaTime * 100 * speed* speedmod);
                Rotate();
                break;
            
            //RunAway
            case 2:
                rb.AddForce((transform.position - Target.position).normalized * Time.deltaTime * 100 * speed* speedmod);
                Rotate();
                break;

            //StayOnePlace
            case 3:
                //rb.AddForce((transform.position - Target.position).normalized * Time.deltaTime * 100 * speed);
                Rotate();
                break;
        }
    }
    void SwitchRotation()
    {
        right = !right;
    }
    void Rotate()
    {
        if (right) rb.angularVelocity = Vector3.up * 100 * rotspeed * rotspeedmod * Time.deltaTime;
        else rb.angularVelocity = -Vector3.up * 100 * rotspeed* rotspeedmod * Time.deltaTime;
        //transform.eulerAngles = transform.rotation.y * Vector3.up;
    }
    void FindEnemy()
    {
        int i=0;
        float dist=0;
        float tdist=0;
        for (int a=0;a< PlayersList.players.Count;a++)
        {
            tdist = Vector3.Distance(transform.position, PlayersList.players[a].position);
            if (tdist < dist)
            { 
                dist = tdist;
                i = a;
            }
        }
        Target = PlayersList.players[i];
    }    
    void BehaviorTimer()
    {
        if (cbehTimer < behTimer) cbehTimer += Time.deltaTime;
        else RandomBehavior();
    }
    void RotationTimer()
    {
        if (crotateTimer < rotateTimer) crotateTimer += Time.deltaTime;
        else
        { 
            if (Random.value > 0.5f) SwitchRotation();
            rotateTimer = Random.Range(1f, 5f);
            crotateTimer = 0;
        }

    }
    void RandomBehavior()
    {
        cbehTimer = 0;
        if (Random.value < Mathf.Clamp(0.7f + 0.05f * AIDifficulty, 0.7f,0.9f))
        {
            Behavior = Random.Range(1, 4);
            behTimer= Random.Range(1f, 5f);
            FindEnemy();
        }
        else
        {
            Behavior = 0;
            behTimer = 1;
        }
    }
    public void Defense()
    {

        if (damagable)
        {
            hitt = 0;
            damagable = false;
        }
    }
    void Update()
    {
        Sword.localEulerAngles =
//Vector3.right * Sword.localEulerAngles.x +
Vector3.up * Sword.localEulerAngles.y;
        Sword.localPosition =
Vector3.right * Sword.localPosition.x +
Vector3.up * 0 +
Vector3.forward * Sword.localPosition.z;
        if (hitt<hittimer)
        {
            hitt += Time.deltaTime;
            if (hitt >= hittimer)
                damagable = true;
        }
        if (bot&& canmove)
        {
            RotationTimer();
            BehaviorTimer();
            Sword.localEulerAngles = Vector3.right * Sword.localEulerAngles.x + Vector3.up * Sword.localEulerAngles.y;
            EnemyBehavior();
            Wall();
        }
        else if (canmove)
        {

            if (Input.GetMouseButtonDown(0))
            {
                right = !right;
                rb.velocity = Vector3.zero;
            }

            if (Input.GetMouseButton(0))
            {
                //moving = true;
                rb.AddForce((Vector3.forward * joystick.Direction.y + Vector3.right * joystick.Direction.x) * Time.deltaTime * 100 * speed * speedmod);
                //rb.velocity=((Vector3.forward * joystick.Direction.y + Vector3.right * joystick.Direction.x) * Time.deltaTime * 100 * speed);
                if (right) rb.angularVelocity = Vector3.up * 100 * rotspeed * rotspeedmod * Time.deltaTime;
                else rb.angularVelocity = -Vector3.up * 100 * rotspeed * rotspeedmod * Time.deltaTime;
            }
            //else moving = false;

        }
    }
}
