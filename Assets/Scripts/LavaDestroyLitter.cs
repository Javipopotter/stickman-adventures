using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDestroyLitter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Rigidbody2D>() && !collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
