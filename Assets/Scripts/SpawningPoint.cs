using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoint : MonoBehaviour
{
    public BalanceSheet balance;
    public GameObject point;
    public float time=5;
    float t;
    private void FixedUpdate()
    {
        if (!point.activeSelf)
        {
            t += Time.fixedDeltaTime;
            if (t> time)
            {
                t = 0;
                point.transform.position = Vector3.right * Random.Range(-balance.MapSize, balance.MapSize) + Vector3.forward * Random.Range(-balance.MapSize, balance.MapSize);
                point.SetActive(true);
            }
        }
    }
}
