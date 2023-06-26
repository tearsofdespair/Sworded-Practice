using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Qual") == 1)
        {
            GetComponent <Light>().shadows= LightShadows.None;
        }
    }

}
