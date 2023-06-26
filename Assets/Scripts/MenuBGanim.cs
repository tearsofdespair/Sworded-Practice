using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBGanim : MonoBehaviour
{
    public Material material;
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset -= (Vector2.right+ Vector2.up)*0.5f*Time.deltaTime;
    }
}
