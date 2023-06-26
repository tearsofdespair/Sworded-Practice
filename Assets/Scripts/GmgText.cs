using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GmgText : MonoBehaviour
{
    public TMPro.TextMeshPro tm;
    public string text;
    private void Start()
    {
        tm.text = text;
    }
    void Update()
    {
        Color color= tm.color;
        color.a = Mathf.MoveTowards(color.a, 0, Time.deltaTime);
        tm.color = color;
        if (color.a == 0) Destroy(gameObject);
    }
}
