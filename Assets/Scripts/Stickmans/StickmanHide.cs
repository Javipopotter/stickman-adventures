using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanHide : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Chunk"))
        {
            transform.parent.transform.parent = collision.transform.parent;
        }
    }
}
