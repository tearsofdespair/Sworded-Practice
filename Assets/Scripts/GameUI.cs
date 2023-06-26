using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public GameObject DamageVignette;
    public CinemachineVirtualCamera cinemachine;
    CinemachineBasicMultiChannelPerlin perlin;
    float shakeTimer;
    public int kills;
    public int deads;
    public Text killsText;
    public Text deadsText;
    public List<PlayerStats> Stats = new List<PlayerStats>();
    public PlayerStats PlayerStat;
    public BalanceSheet balance;

    [Header("EndGame")]
    public GameObject EndMenu;
    public GameObject Victory;
    public GameObject Defeat;
    public GameObject Whistle;
    public GameObject HolderStats;
    bool finish;
    [Header("Modes")]
    public GameObject[] Mode;
    public int mode;
    public int alive = 6;
    public Text Alive;
    [Header("PlayersCheck")]
    public Text[] PlayerCheck;
    public Text[] PlayerScore;
    public GameObject LastString;
    public GameObject Rating;
    public GameObject FinishRating;
    bool win;
    public Text[] PlayerCheckEnd;
    public Text[] PlayerScoreEnd;
    [Header("Progress")]
    public Image ProgressLine;
    public GameObject NewLevel;
    public Button NextButton;
    public Text PlusText;
    public Text LineText;
    public Text LevelText;
    public Text PlayerPosText;

    public void checkPlayersEnd()
    {
        Application.targetFrameRate = 60;
        List<PlayerStats> CheckStats = Stats;
        CheckStats.Sort((stat1, stats2) => stat1.Points < stats2.Points ? 1 : -1);
        int player = 0;
        for (int i = 0; i < CheckStats.Count; i++)
        {
            if (Stats[i].player == true)
            {
                player = i;
            }
        }
        for (int i = 0; i < PlayerCheckEnd.Length; i++)
        {
            PlayerCheckEnd[i].text = "#" + (i + 1).ToString() + " " + CheckStats[i].Name;
            PlayerScoreEnd[i].text = CheckStats[i].Points.ToString();
        }
        for (int i = 0; i < PlayerCheckEnd.Length; i++)
        {
            if (player == i)
            {
                PlayerCheckEnd[i].fontStyle = FontStyle.BoldAndItalic;
                PlayerPosText.text = "Place: " + (i + 1);
            }
            else PlayerCheckEnd[i].fontStyle = FontStyle.Normal;
            
        }

    }
    public void Start()
    {
        //Time.timeScale = 0.5f;
        Application.targetFrameRate = 60;
    }
    public void MenuSkill()
    {
        PlayerPrefs.SetInt("LoadSkill", 1);
        SceneManager.LoadScene("Menu");
    }
    void SkillProgresss(int xp)
    {
        LevelText.text= PlayerPrefs.GetInt("Level").ToString();
        PlusText.text = "+" + xp;

        NextButton.interactable = false;
        float clevel=0;
        float nlevel =0;

        if (PlayerPrefs.GetInt("Level") < balance.level.Length-1)
        {
            clevel = balance.level[PlayerPrefs.GetInt("Level")].Points;
            nlevel = balance.level[PlayerPrefs.GetInt("Level") + 1].Points;
        }
        else
        {
            clevel = PlayerPrefs.GetInt("Level") * 3000;
            nlevel = (PlayerPrefs.GetInt("Level")+1) * 3000;
        }

        float cpoints = PlayerPrefs.GetInt("XP");
        //ProgressLine.fillAmount = 
        float start = (cpoints - clevel)/ (nlevel- clevel);
        float finish = (cpoints+xp - clevel)/ (nlevel - clevel);
        StartCoroutine(ProgressLineMove(start, finish, xp, clevel, nlevel));

    }
    IEnumerator ProgressLineMove(float start, float finish, int xp, float clevel, float nlevel)
    {

        int cpoints = PlayerPrefs.GetInt("XP");
        
        float scale = (finish-start)/100;
        //Debug.Log(start + "/" + finish);
        LineText.text = (cpoints) + "/" + nlevel;
        ProgressLine.fillAmount = start;
        float add = 0;
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().Play();
        while (start<finish)
        {
            start += scale;
            add += 0.01f;
            LineText.text = (int)(cpoints + xp * add) + "/"+nlevel;
            ProgressLine.fillAmount = start;
            yield return new WaitForSeconds(0.01f);
        }
        LineText.text = (int)(cpoints + xp) + "/" + nlevel;
        NextButton.interactable = true;
        PlayerPrefs.SetInt("XP", cpoints+ xp);
        for (int i = PlayerPrefs.GetInt("Level")+1; i < balance.level.Length; i++)
        {
            if (cpoints + xp >= balance.level[i].Points)
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                LevelText.text = PlayerPrefs.GetInt("Level").ToString();
                NewLevel.SetActive(true);
                PlayerPrefs.SetInt("NewSkill",1);
            }
        }
    }
    public void checkPlayers()
    {
        if (mode == 0)
        {
            int player = 0;
            int playerscore = Stats[0].Points;
            List<PlayerStats> CheckStats = Stats;
            CheckStats.Sort((stat1, stats2) => stat1.Points < stats2.Points ? 1 : -1);

            for (int i = 0; i < CheckStats.Count; i++)
            {
                if (Stats[i].player == true)
                {
                    player = i;
                }
            }
            PlayerCheck[0].text = "#1 " + CheckStats[0].Name;
            PlayerCheck[1].text = "#2 " + CheckStats[1].Name;
            PlayerCheck[2].text = "#3 " + CheckStats[2].Name;
            PlayerScore[0].text = CheckStats[0].Points.ToString();
            PlayerScore[1].text = CheckStats[1].Points.ToString();
            PlayerScore[2].text = CheckStats[2].Points.ToString();
            if (player > 2)
            {
                LastString.SetActive(true);
                PlayerCheck[3].text = "#" + (player + 1).ToString() + " " + Stats[player].Name;
                PlayerScore[3].text = Stats[player].Points.ToString();
            }
            else LastString.SetActive(false);
            for (int i = 0; i < PlayerCheck.Length; i++)
            {
                if (player == i)
                {
                    PlayerCheck[i].fontStyle = FontStyle.BoldAndItalic;
                }
                else PlayerCheck[i].fontStyle = FontStyle.Normal;
            }
        }
    }
    public void MinusAlive()
    {
        alive--;
        Alive.text = alive.ToString();
        if (alive == 1) EndGame();
    }
    public void StartGame()
    {
        Whistle.SetActive(true);
        for (int i = 0; i < Stats.Count; i++)
        {
            Stats[i].movement.canmove = true;
        }
    }

    public void Finish()
    {

        EndMenu.SetActive(true);
        if (mode == 0)
        {
            PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins") + 1);
            int XP = PlayerStat.Points+ kills * 50;
            

            checkPlayersEnd();
            List<PlayerStats> CheckStats = Stats;
            CheckStats.Sort((stat1, stats2) => stat1.Points < stats2.Points ? 1 : -1);
            if (PlayerStat.Points >= CheckStats[0].Points)
            {
                Victory.SetActive(true);
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 100);
                PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins") + 1);
                //EventSenderManager.SendLevelFinish(0, 100, mode == 0 ? "supriority" : "royale");
            }
            else
            {
                Defeat.SetActive(true);
                if (PlayerPrefs.GetInt("Wins") > 0)
                    PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins") - 1);
                //EventSenderManager.SendLevelFinish(0, 0, mode == 0 ? "supriority" : "royale");
            }
            SkillProgresss(XP);
            
        }
        else
        {
            int XP = kills*200;
            

            FinishRating.SetActive(false);
            if (win)
            {
                XP += 500;
                Victory.SetActive(true);
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 100);
                PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins") + 1);
               // EventSenderManager.SendLevelFinish(0, 100, mode == 0 ? "supriority" : "royale");
            }
            else
            {
                Defeat.SetActive(true);
                if (PlayerPrefs.GetInt("Wins") > 0)
                    PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins") - 1);
                // EventSenderManager.SendLevelFinish(0, 0, mode == 0 ? "supriority" : "royale");
                PlayerPosText.text = "Place: " + (alive+1);
            }
            SkillProgresss(XP);

            
        }
        HolderStats.SetActive(false);
        finish = true;
    }
    public void EndGame()
    {
        Time.timeScale = 1;
        Whistle.SetActive(true);
        for (int i = 0; i < Stats.Count; i++)
        {
            Stats[i].movement.canmove = false;
        }

        if (PlayerStat.movement.gameObject.activeSelf) win = true;
        Invoke("Finish", 2);
    }
    void Awake()
    {

        //System.Array.Sort(Rating);
        mode = PlayerPrefs.GetInt("Mode");
        Mode[mode].SetActive(true);
        if (mode != 0) Rating.SetActive(false);
        Invoke("StartGame", 1);
        instance = this;

        perlin = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void Restart()
    {
        SceneManager.LoadScene("Menu");
    }
    public void AddKill()
    {
        kills++;
        killsText.text = kills.ToString();
    }
    public void AddDeath()
    {
        deads++;
        deadsText.text = deads.ToString();
    }
    public void FindKing()
    {
        int king = 0;
        int kingpoints = 0;
        for (int i = 0; i < Stats.Count; i++)
        {
            Stats[i].Crown.SetActive(false);
            if (Stats[i].Points > kingpoints)
            { king = i; kingpoints = Stats[i].Points; }

        }
        Stats[king].Crown.SetActive(true);
    }
    public void ShakeCam(float intensity, float time)
    {
        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
    void ShakeTime()
    {
        shakeTimer -= Time.deltaTime;
        perlin.m_AmplitudeGain = Mathf.MoveTowards(perlin.m_AmplitudeGain, 0, Time.deltaTime);
        if (shakeTimer <= 0)
        {
            perlin = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0;
        }
    }
    private void Update()
    {
       // Debug.Log(Application.targetFrameRate);
        if (shakeTimer > 0) ShakeTime();

        //if (Input.GetMouseButton(0) && !finish)
        if (!finish)
        {
            cinemachine.m_Lens.FieldOfView = Mathf.Lerp(cinemachine.m_Lens.FieldOfView, 28
                //* Mathf.Clamp(Stats[0].movement.rb.velocity.magnitude/5,1,1.4f)
                //* Time.timeScale
                , 2 * Time.deltaTime);
          //  Debug.Log(Stats[0].movement.rb.velocity.magnitude);
        }
        else
        {
            cinemachine.m_Lens.FieldOfView = Mathf.Lerp(cinemachine.m_Lens.FieldOfView, 15, 2 * Time.deltaTime);
        }
    }


}
