using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRandom : MonoBehaviour
{
    public GameObject[] World;    
    void Awake()
    {
        World[Random.Range(0, World.Length)].SetActive(true);
    }

}
