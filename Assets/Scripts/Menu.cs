using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public Material MainMaterial;
    public Material HandsMaterial;
    public Material WeaponMat;
    public Material LockedWeaponMat;
    public Color LockedColor;
    public MenuWeapon[] Weapon;
    public Transform WeaponTransform;
    public Button[] Arrow1;
    public Button[] Arrow2;
    public int cSkin;
    public int cWeapon;
    public GameObject WLockScreen;
    public GameObject SLockScreen;
    public BalanceSheet balance;
    public Text WCost;
    public Text SCost;
    public InputField PlayerName;
    public Text Coins;
    public Text Wins;
    public LoadingMenu Loading;
    public GameObject NewSkill;
    public GameObject SkillMenu;
    [Header("Settings")]
    public GameObject[] SettingSprite;
    public MusicOff Music;
    public UnityEvent BuyEvent;

    void Start()
    {
        if (PlayerPrefs.GetInt("LoadSkill") == 1)
        {
            SkillMenu.SetActive(true);
            PlayerPrefs.SetInt("LoadSkill", 0);
            PlayerPrefs.SetInt("NewSkill", 0);
        }
        if (PlayerPrefs.GetInt("NewSkill") == 1)
        {
            NewSkill.SetActive(true);
            PlayerPrefs.SetInt("NewSkill", 0);
        }
            Time.timeScale = 1;
        for (int i = 0; i < Weapon.Length; i++)
        {
            if (PlayerPrefs.GetInt("UWeapon" + i) == 0)
                for (int a = 0; a < Weapon[i].mesh.Length; a++)
                {
                    Weapon[i].mesh[a].material = LockedWeaponMat;
                }
        }
        Wins.text = (PlayerPrefs.GetInt("Level")+1).ToString();
        PlayerPrefs.SetInt("UWeapon0", 1);
        PlayerPrefs.SetInt("USkin0", 1);
        ChangeSkin(PlayerPrefs.GetInt("CSkin"));
        ChangeWeapon(PlayerPrefs.GetInt("CWeapon"));
        //LevelLine.fillAmount = 0.81f * PlayerPrefs.GetInt("Points");
        //Level.text = ((int)(Mathf.Pow(PlayerPrefs.GetInt("Points"), 2 / 3) / 20)).ToString();
        if (!PlayerPrefs.HasKey("Name"))
        PlayerPrefs.SetString("Name", "Player"+Random.Range(1000,10000));
        PlayerName.text = PlayerPrefs.GetString("Name");
        ChangeMoney();
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            SettingSprite[0].SetActive(false);
            Music.Off();
        }
        else
        {
            SettingSprite[0].SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vibro") == 1)
        {
            SettingSprite[1].SetActive(false);
        }
        else
        {
            SettingSprite[1].SetActive(true); 
        }
        if (PlayerPrefs.GetInt("Qual") == 1)
        {
            SettingSprite[2].SetActive(false);
        }
        else
        {
            SettingSprite[2].SetActive(true);
        }
    }

    public void UnlockAll()
    {
        PlayerPrefs.SetInt("Money", 219387);
        PlayerPrefs.SetInt("Level", 5);
        PlayerPrefs.SetInt("XP", 15000);
        SceneManager.LoadScene("Menu");
    }
    public void LoadRoyale()
    {
        MainMaterial.color = Color.white;
        HandsMaterial.color = balance.skin[PlayerPrefs.GetInt("CSkin")].HandsColor;
        Loading.gameObject.SetActive(true);
        Loading.Load(1);
    }
    public void LoadSupriority()
    {
        MainMaterial.color = Color.white;
        MainMaterial.mainTextureOffset = new Vector2(cSkin * 1f / 6, 1);
        HandsMaterial.color = balance.skin[PlayerPrefs.GetInt("CSkin")].HandsColor;
        Loading.gameObject.SetActive(true);
        Loading.Load(0);
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
    public void ChangeMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            PlayerPrefs.SetInt("Music", 1);
            SettingSprite[0].SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 0);
            SettingSprite[0].SetActive(true);
        }
        Music.Off();
    }
    public void ChangeVibro()
    {
        if (PlayerPrefs.GetInt("Vibro") == 0)
        {
            PlayerPrefs.SetInt("Vibro", 1);
            SettingSprite[1].SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Vibro", 0);
            SettingSprite[1].SetActive(true);
        }
    }
    public void ChangeQual()
    {
        if (PlayerPrefs.GetInt("Qual") == 0)
        {
            PlayerPrefs.SetInt("Qual", 1);
            SettingSprite[2].SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Qual", 0);
            SettingSprite[2].SetActive(true);
        }
    }
    public void ChangeName()
    {
        PlayerPrefs.SetString("Name", PlayerName.text);
    }
    public void ChangeMoney()
    {
        Coins.text = PlayerPrefs.GetInt("Money").ToString();
    }
    public void ChangeSkin(int s)
    {
        cSkin += s;
        cSkin = Mathf.Clamp(cSkin, 0, balance.skin.Length - 1);
        if (cSkin == 0) Arrow1[0].interactable = false;
        else Arrow1[0].interactable = true;
        if (cSkin == balance.skin.Length - 1) Arrow1[1].interactable = false;
        else Arrow1[1].interactable = true;


        if (PlayerPrefs.GetInt("USkin" + cSkin) == 0)
        {
            SLockScreen.SetActive(true);
            SCost.text = balance.skin[cSkin].cost.ToString();
            MainMaterial.color = LockedColor;
            //HandsMaterial.color = LockedColor;
           // MainMaterial.color = Color.white;
            MainMaterial.mainTextureOffset = new Vector2(cSkin * 1f / 6, 1);
            HandsMaterial.color = balance.skin[cSkin].HandsColor;

        }
        else
        {
            SLockScreen.SetActive(false);
            PlayerPrefs.SetInt("CSkin", cSkin);
            MainMaterial.color = Color.white;
            MainMaterial.mainTextureOffset = new Vector2 (cSkin*1f / 6,1);
            HandsMaterial.color = balance.skin[cSkin].HandsColor;
        }
    }
    public void BuySkin()
    {
        if (PlayerPrefs.GetInt("Money")>=balance.skin[cSkin].cost)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - balance.skin[cSkin].cost);
            PlayerPrefs.SetInt("USkin"+ cSkin, 1);
            ChangeSkin(0);
            ChangeMoney();
            BuyEvent.Invoke();
        }
    }
    public void ResetSkin()
    {
        MainMaterial.color = Color.white;
        MainMaterial.mainTextureOffset = new Vector2(cSkin * 1f / 6, 1);
        HandsMaterial.color = balance.skin[cSkin].HandsColor;
    }
    public void BuyWeapon()
    {
        if (PlayerPrefs.GetInt("Money")>=balance.weapon[cWeapon].cost)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - balance.weapon[cWeapon].cost);
            PlayerPrefs.SetInt("UWeapon" + cWeapon, 1);
            ChangeWeapon(0);
            ChangeMoney();
            BuyEvent.Invoke();

        }
    }
    public void ChangeWeapon(int w)
    {
        cWeapon += w;
        cWeapon = Mathf.Clamp(cWeapon,0, Weapon.Length-1);
        if (cWeapon == 0) Arrow2[0].interactable = false;
        else Arrow2[0].interactable = true;
        if (cWeapon == Weapon.Length-1) Arrow2[1].interactable = false;
        else Arrow2[1].interactable = true;
        if (PlayerPrefs.GetInt("UWeapon" + cWeapon) == 0)
        {
            for (int a = 0; a < Weapon[cWeapon].mesh.Length; a++)
            {
                Weapon[cWeapon].mesh[a].material = LockedWeaponMat;
            }
            WLockScreen.SetActive(true);
            WCost.text = balance.weapon[cWeapon].cost.ToString();
        }
        else
        {
            for (int a = 0; a < Weapon[cWeapon].mesh.Length; a++)
            {
                Weapon[cWeapon].mesh[a].material = WeaponMat;
            }
            WLockScreen.SetActive(false);
            PlayerPrefs.SetInt("CWeapon", cWeapon);
        }
    }
    void Update()
    {
        WeaponTransform.localPosition = Vector3.Lerp(WeaponTransform.localPosition, 5*Vector3.right * cWeapon, 5*Time.deltaTime);
    }
}
[System.Serializable]
public class MenuWeapon
{
    public Transform[] handpos;
    public MeshRenderer[] mesh;
}
