using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersList : MonoBehaviour
{
    public List<Transform> players = new List<Transform>();
    
    void Start()
    {
        for (int i = 0;i< players.Count;i++)
        {
            PlayerIndex index = players[i].GetComponent<PlayerIndex>();
            index.index = i;
            GameUI.instance.Stats.Add(index.playerStats);
        }
        GameUI.instance.PlayerStat = GameUI.instance.Stats[0];
    }


}
