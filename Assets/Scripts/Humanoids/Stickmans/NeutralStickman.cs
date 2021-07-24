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
        Movement();
    }

    private void Movement()
    {
        if (enemy != null && aIGrab[0].grabbed)
        {
            foreach (AIGrab aIGrab in aIGrab)
            {
                if (Vector2.Distance(torso.transform.position, enemy.transform.position) < 120 && Vector2.Distance(enemy.transform.position, torso.transform.position) > aIGrab.pickableObject.range)
                {
                    MovementDir(enemy, 1);
                }

                if (Vector2.Distance(torso.transform.position, enemy.transform.position) <= aIGrab.pickableObject.range)
                {
                    Attack(aIGrab.grabbedObject, enemy, aIGrab);
                } 
            }
        }
    }
}
