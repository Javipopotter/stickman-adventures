using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    Vector2 InitPos;
    Vector2 Dist;
    bool TakeBack;
    Vector2 dir;
    [SerializeField] GameObject Ref;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitPos = transform.position;
        Dist = Ref.transform.position;
        dir = transform.position - Ref.transform.position;
        dir.Normalize();
    }

    private void Update()
    {
        if (TakeBack && Vector2.Distance(transform.position, InitPos) > 1)
            rb.MovePosition(rb.position + dir * 0.5f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            TakeBack = false;
            if (Vector2.Distance(transform.position, Dist) > 1)
                rb.MovePosition(rb.position + -dir * 0.3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
            TakeBack = true;
    }
}
