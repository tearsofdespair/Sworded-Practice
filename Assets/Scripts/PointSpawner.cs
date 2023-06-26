using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public BalanceSheet balance;
    public SpawningPoint prefab;
    public List<SpawningPoint> points= new List<SpawningPoint>();
    void Start()
    {
        for (int i=0;i< balance.PointsToSpawn;i++)
        {
            SpawningPoint point= Instantiate(prefab,
                Vector3.right * Random.Range(-balance.MapSize, balance.MapSize) + Vector3.forward * Random.Range(-balance.MapSize, balance.MapSize),
                Quaternion.identity);
            //point.transform.localScale = Vector3.one* 2;
        }
    }
}
