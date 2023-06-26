using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsMenu : MonoBehaviour
{
    public GameObject SkillMenu;
    public BalanceSheet balance;
    public Image Sprite;
    public SkillCard[] card;
    public Color orange;
    void Start()
    {
        for (int i = 0; i< card.Length; i++)
        {
            if (PlayerPrefs.GetInt("Level") >= i) 
            {
                card[i].button.interactable = true;
                card[i].text.color = Color.white;
                card[i].image.color = Color.grey;
                card[i].Lock.SetActive(false);
            }
        }
        card[PlayerPrefs.GetInt("ActiveSkill")].image.color = orange;
        Sprite.sprite = balance.level[PlayerPrefs.GetInt("ActiveSkill")].Skill;
        Sprite.SetNativeSize();
    }
    public void SelectSkill(int i)
    {
        card[PlayerPrefs.GetInt("ActiveSkill")].image.color = Color.grey;
        card[i].image.color = orange;
        PlayerPrefs.SetInt("ActiveSkill", i);
        Sprite.sprite = balance.level[i].Skill;
        Sprite.SetNativeSize();
        SkillMenu.SetActive(false);
    }

}
[System.Serializable]
public class SkillCard
{
    public Button button;
    public Text text;
    public Image image;
    public GameObject Lock;
}

