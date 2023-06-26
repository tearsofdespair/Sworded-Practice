using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColor : MonoBehaviour
{
    public Color color;
    public ParticleSystem[] ps;
    private void Start()
    {
        for (int i=0;i<ps.Length;i++)
        {
            ps[i].startColor = color;
        }
    }
}
