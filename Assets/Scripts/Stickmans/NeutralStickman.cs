using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralStickman : AI
{
    public override void Awake()
    {
        base.Awake();
        aIGrab.LayerOfTheWeapon = 0;
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
