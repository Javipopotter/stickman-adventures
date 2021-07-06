using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : PickableObject
{
    GameObject LastHolder;
    public float force = 10;
    bool OnAir;
    int bounces;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (LastHolder != null)
        {
            if (OnAir == false && Vector2.Distance(transform.position, LastHolder.transform.position) < 2.5f)
            {
                OnAir = true;
                LastHolder = null;
                rb.velocity = Vector2.zero;
            }
        }
    }

    public void Throw(Vector2 dir)
    {
        if (Holded)
        {
            bounces = 0;
            OnAir = true;
            LastHolder = Holder;
            dir = new Vector2(transform.position.x, transform.position.y) - dir;
            dir.Normalize();
            rb.velocity = -dir * force;
        }
    }

    public void Return()
    {
        OnAir = false;
        Vector2 dir;
        dir = transform.position - LastHolder.transform.position;
        dir.Normalize();
        rb.velocity = -dir * force;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (!Holded && LastHolder != null)
        {
            if (Vector2.Distance(transform.position, LastHolder.transform.position) > 5 && bounces <= 4)
            {
                bounces++;
                Return(); 
            }
        }
    }
}
