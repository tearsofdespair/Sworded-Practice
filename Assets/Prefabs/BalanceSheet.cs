using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BalanceSheet", order = 1)]
public class BalanceSheet : ScriptableObject
{
    public float XPcoeff = 4;
    public float HPcoeff = 0.5f;
    public float MapSize = 40;
    public float PointsToSpawn = 20;
    public Skin[] skin;
    public Weapon[] weapon;
    public Level[] level;
}
[System.Serializable]
public class Skin
{
    public Color HandsColor;
    public int cost;
}
[System.Serializable]
public class Weapon
{
    public int cost;
    public float damage;
    public float speed;
}
[System.Serializable]
public class Level
{
    public int Points;
    public Sprite Skill;
}
