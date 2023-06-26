using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostIndex : MonoBehaviour
{
    public int index;
    public GameObject[] BoostObj;
    public float time = 5;
    public Collider collider;
    float t;
    bool active;
    public TextMesh text;
    public void GetBoost()
    {
        collider.enabled = false;
        active = false;
        BoostObj[index].SetActive(false);
        t = 0;
    }
    private void FixedUpdate()
    {
        if (!active)
        {
            if (t < time)
            {
                t += Time.fixedDeltaTime;
                text.text=((int)(time-t)).ToString();
            }
            else
            {
                active = true;
                text.text = null;
                index = Random.Range(0, BoostObj.Length);
                BoostObj[index].SetActive(true);
                collider.enabled = true;
            }
        }
    }
}
