using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AI
{
    public override void Awake()
    {
        base.Awake();
        enemy = Player;
        aIGrab.LayerOfTheWeapon = 6;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (aIGrab.grabbed)
        {
            if (Vector2.Distance(torso.transform.position, Player.transform.position) < 120 && Vector2.Distance(torso.transform.position, Player.transform.position) > Range)
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
