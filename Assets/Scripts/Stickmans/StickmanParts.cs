using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanParts : MonoBehaviour
{
    bool separated;

    void Update()
    {
        if(TryGetComponent(out PartsLifes PL) && PL.lifes <= 0)
        {
            separated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Chunk") && separated)
        {
            transform.parent = collision.transform.parent;
        }
    }
}
