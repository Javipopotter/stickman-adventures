using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoCollisions : MonoBehaviour
{
    StickmanLifesManager respawn;
    private void Start()
    {
        respawn = GetComponentInParent<StickmanLifesManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("DeathZone"))
        {
            respawn.respawn();
        }
    }
}
