using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    public GmgText tm;
    public float offset;
    public GameObject tobj;

    private void OnCollisionEnter(Collision collision)
    {
        if (tobj==null&&collision.transform.parent != transform)
        {
            int m = (int)collision.relativeVelocity.magnitude;
            if (collision.gameObject.tag == "Sword" && m > 10)
            {
                GmgText t = Instantiate(tm, transform.position +
                    Vector3.right * Random.Range(-offset, offset) +
                    Vector3.up * Random.Range(-offset, offset) +
                    Vector3.forward * Random.Range(-offset, offset),
                    Quaternion.identity
                    );
                t.text = m.ToString();
                tobj = t.gameObject;
            }
        }
    }
}
