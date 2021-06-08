using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoCollisions : MonoBehaviour
{
    PlayerLifesManager respawn;
    private void Awake()
    {
        respawn = GetComponentInParent<PlayerLifesManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("DeathZone"))
        {
            respawn.respawn();
        }
    }
}
