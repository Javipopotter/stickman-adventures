using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralStickman : AI
{
    public override void Awake()
    {
        base.Awake();
        if (transform.gameObject.layer == 11)
        {
            enemy = GameManager.Gm.PlayerTorso;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (enemy != null && aIGrab.grabbed)
        {
            if(Vector2.Distance(torso.transform.position, enemy.transform.position) < 120 && Vector2.Distance(enemy.transform.position, torso.transform.position) > Range)
            {
                MovementDir(enemy, 1);
            }

            if (Vector2.Distance(torso.transform.position, enemy.transform.position) <= Range)
            {
                Attack(aIGrab.grabbedObject, aIGrab.pickableObject, enemy);
            } 
        }
    }
}
