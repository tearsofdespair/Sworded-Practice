using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoyaleWalls : MonoBehaviour
{
    public GameObject[] renderer;
    bool move;
    public float mod=0.1f;
    void Start()
    {
        if (PlayerPrefs.GetInt("Mode") == 1)
        {
            for (int a = 0; a < renderer.Length; a++)
            {
                renderer[a].SetActive(true);
            }
            move = true;
        }
        else Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (move&& transform.localScale.x>3.2f)
        {
            transform.localScale -= Vector3.right* mod * Time.deltaTime + Vector3.forward* mod * Time.deltaTime;
        }
    }
}
