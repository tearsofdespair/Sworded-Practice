using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform Player;
    private void Update()
    {
        transform.position = Player.position;
    }
}
