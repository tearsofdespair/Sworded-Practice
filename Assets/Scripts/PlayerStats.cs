using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public bool prop;
    public bool player;
    public bool botface;
    public int HP;
    public int MaxHP;
    public float Damage=1;
    public int Level=1;
    public int Points;
    [Header("Tech")]
    public Animator animator;
    public Animation DamageAnim;
    public Animation CoinAnim;
    public Animation healtyLineAnim;
    public Text DamageText;
    public Text XPText;
    public GameUI gameUI;
    public BotMovement movement;
    public GameObject XPFX;
    public GameObject Crown;
    public GameObject NewLevel;
    public GameObject DamageVignette;
    public GameObject UIobj;
    public BalanceSheet balance;
    public Transform RespawnFX;
    public Transform SmokeFX;
    public Transform PointObj;
    public Image LevelLine;
    public Text LevelText;
    public Text nameText;
    public string Name;
    public Text healtyText;
    public Transform healtyLine;
    public Transform damageLine;
    public Transform Player;
    public RectTransform HolderStats;
    public RectTransform UI_Element;
    public Camera Cam;
    float startsize;
    public int lastHit;
    bool vibro;
    bool timeshift;
    public DamageSounder sounder;
    public float damagemod = 1;
    public bool vampire;
    private void Start()
    {
        if (!player)
        {
            if (PlayerPrefs.GetInt("Internet") == 1) botface = false;
            else botface = true;
        }
        
        if (!botface) nameText.text = "Player" + Random.Range(1000, 10000);
        if (player) nameText.text = PlayerPrefs.GetString("Name");
        if (gameUI==null) gameUI = GameUI.instance;
        Name = nameText.text;
        HolderStats = GameObject.FindGameObjectWithTag("HolderStats").GetComponent<RectTransform>();
        UI_Element = GetComponent<RectTransform>();
        Cam = Camera.main;
        transform.parent = HolderStats;
        startsize = Cam.fieldOfView;
        HP = MaxHP;
        AddHP(0);
        AddPoints(0);
        movement = Player.GetComponent<BotMovement>();
        if (player&&PlayerPrefs.GetInt("Vibro") == 0) vibro = true;
        //DamageVignette = gameUI.DamageVignette;
    }

    private void Update()
    {
        XPFX.transform.rotation = Quaternion.identity;
        Vector2 ViewportPosition = Cam.WorldToViewportPoint(Player.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * HolderStats.sizeDelta.x) - (HolderStats.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * HolderStats.sizeDelta.y) - (HolderStats.sizeDelta.y * 0.5f)));
        UI_Element.localScale = Vector2.one * startsize / Cam.fieldOfView;
        //now you can set the position of the ui element
        UI_Element.anchoredPosition =Vector2.Lerp(UI_Element.anchoredPosition, WorldObject_ScreenPosition, 8*Time.deltaTime);
        damageLine.localScale = Vector3.up + Vector3.right * Mathf.MoveTowards(damageLine.localScale.x, healtyLine.localScale.x, Time.deltaTime/3);
    }
    public void AddHP(int hp)
    {
        // if(hp<0) DamageVignette.SetActive(true);
        if (gameUI.Stats.Count > 1)
        {
            
            PlayerStats playerStats = gameUI.Stats[lastHit];
            hp = (int)(hp * playerStats.MaxHP* playerStats.Damage * playerStats.damagemod / 100);
            if (hp < 0 && playerStats.vampire)
            {

                gameUI.Stats[lastHit].AddHP(-hp/4);
                
            }
        }
        HP += hp;
        if (hp != 0)
        {
            healtyLineAnim.Play();
        }
        if (hp < 0) 
        {

            if (!botface)
            {
                animator.SetTrigger("Hit");
                sounder.PlayOuch();
            }
            DamageAnim.Play();
            DamageText.text=(-hp).ToString(); 
            DamageText.fontSize=Mathf.Clamp(-hp * 160 / MaxHP, 50,85);
            if (-hp > MaxHP / 4)
            {
                DamageText.color = Color.red;
                if (Time.timeScale == 1)
                {
                    if (player || lastHit == 0) StartCoroutine(TimeShift());
                }
            }
            else DamageText.color = Color.white;
            DamageText.transform.localPosition=Vector3.right * Random.Range(-50, 50) + Vector3.forward *Random.Range(-50, 50);
        }


        HP = Mathf.Clamp(HP, 0, MaxHP);
        healtyText.text = HP.ToString();
        healtyLine.localScale = Vector3.up + Vector3.right * ((float)HP / MaxHP);
        if (HP<=0)
        {
            Death();
        }
    }
    IEnumerator TimeShift()
    {
        //float elapsed=0;
        //float wait=1;

        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.01f;

        yield return new WaitForSecondsRealtime(0.5f);

        while (Time.timeScale<1)
        {
            Time.timeScale += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

    }
    public void GiveXP(int p)
    {
        int d = (int)(Points - 5 * balance.XPcoeff * (Level - 1) * (Level + balance.XPcoeff - 1));
        AddPoints(-d);
        if (p!=0)
        gameUI.Stats[p].AddPoints((int)(d*1.5f + 25));
        else
        gameUI.Stats[p].AddPoints(d + 25);
        /*if (d > 10)
        {
            Transform rb = Instantiate(PointObj, pos, Quaternion.identity);
            //rb.position = Vector3.right * (Player.position.x + Random.Range(-1f, 1)) + Vector3.forward * (Player.position.z + Random.Range(-1f, 1));
            rb.localScale = Vector3.one * (1+ d /100);
        }
        else
        {
            Transform rb = Instantiate(PointObj, pos, Quaternion.identity);
            //rb.position = Vector3.right * (Player.position.x + Random.Range(-1f, 1)) + Vector3.forward * (Player.position.z + Random.Range(-1f, 1));
            rb.localScale = Vector3.one ;
        }*/

    }
    public void Death()
    {
        if (!prop)
        {
            if (lastHit == 0)
            {
                gameUI.AddKill();
            }
            if (player)
            {
                gameUI.AddDeath();
            }
            GiveXP(lastHit);
            Player.gameObject.SetActive(false);
            UIobj.SetActive(false);
            HP = MaxHP;
            AddHP(0);
            Instantiate(SmokeFX, Player.position + Vector3.up * 0.5f, Quaternion.identity);
            if (gameUI.mode == 0)
                StartCoroutine(Respawn());
            else
            {
                if (player) gameUI.EndGame();
                else Player.position = Vector3.up * 3 + Vector3.right * 100 + Vector3.forward * 100;
                gameUI.MinusAlive();
            }
        }
    }
    public void AddPoints(int points)
    {
        Points += points;
        float pointsCL = 5 * balance.XPcoeff * (Level) * (Level + balance.XPcoeff);
        float pointsPL = 5 * balance.XPcoeff * (Level-1) * (Level-1 + balance.XPcoeff);
        if (points > 0)
        {
            if (player)
            {
                XPText.text = "+" + points + "xp";
                XPText.GetComponent<Animation>().Stop();
                XPText.GetComponent<Animation>().Play();
            }
            XPFX.SetActive(true);
            NewLevel.SetActive(true);
            NewLevel.transform.localScale = Vector3.one;
        }
        if (Points > (5 * balance.XPcoeff * (Level) * (Level + balance.XPcoeff)))
        {
            if (!botface) animator.SetTrigger("Level");
            AddLevel(); 
        }
        else if (!botface) animator.SetTrigger("Point");

        AddHP((int)(10 * Mathf.Pow((float)Level, balance.HPcoeff)));
        if (Level>1) gameUI.FindKing();
        if (Points > 1) gameUI.checkPlayers();
        LevelLine.fillAmount = 0.81f * ((Points- pointsPL) / ((5 * balance.XPcoeff * (Level) * (Level + balance.XPcoeff)) - pointsPL));
        LevelText.text = (Level).ToString();
    }
    void AddLevel()
    {
        Level++;
        NewLevel.transform.localScale = Vector3.one*1.5f;
        if (Points > (5 * balance.XPcoeff * (Level) * (Level + balance.XPcoeff))) AddLevel();
        else
        {
            MaxHP = (int)(100 * Mathf.Pow((float)Level, balance.HPcoeff));
            HP = MaxHP;
            AddHP(0);
        }
        LevelLine.fillAmount = 0;
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        Player.position = Vector3.up * 3 + Vector3.right * Random.Range(-balance.MapSize, balance.MapSize) + Vector3.forward * Random.Range(-balance.MapSize, balance.MapSize);
        if (player) gameUI.ShakeCam(0, 0);
        yield return new WaitForSeconds(1);
        Player.gameObject.SetActive(true);
        UIobj.SetActive(true);
        Instantiate(RespawnFX, Player.position, Quaternion.identity);
    }
}
