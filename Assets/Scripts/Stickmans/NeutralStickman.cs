using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralStickman : AI
{
    public override void Awake()
    {
        base.Awake();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PickableObject picked))
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 20)
            {
                enemy = picked.Holder;
            } 
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (enemy != null)
        {
            if (Vector2.Distance(enemy.transform.position, torso.transform.position) > Range)
            {
                MovementDir(enemy);

            }

            if (Vector2.Distance(torso.transform.position, enemy.transform.position) <= Range && aIGrab.grabbed)
            {
                Attack(aIGrab.grabbedObject, aIGrab.pickableObject, enemy);
            } 
        }
    }
}
