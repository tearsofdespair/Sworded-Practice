using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShield : MonoBehaviour
{
    public Transform target;
    public Collider collider;
    public Collider[] other;
    private void Awake()
    {

        collider.enabled = true;
        /*for (int i = 0; i < other.Length; i++)
        {
            Physics.IgnoreCollision(collider, other[i]);
        }*/

    }
    void Update()
    {
        transform.Rotate(Vector3.up*100*Time.deltaTime);
        transform.position = target.position;
    }
}
