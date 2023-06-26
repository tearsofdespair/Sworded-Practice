using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class sorttest : MonoBehaviour
{
    public List<Stats> stats = new List<Stats>();
    private void Awake()
    {
    stats.Sort((stat1, stats2) => stat1.Point > stats2.Point ? 1 : -1);
    }
}

[System.Serializable]
public class Stats
{
    public int Point;
}
