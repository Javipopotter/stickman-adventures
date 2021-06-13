using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AI
{
    public override void Awake()
    {
        base.Awake();
        enemy = Player;
        aIGrab.IsFriend = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Vector2.Distance(torso.transform.position, Player.transform.position) < 120 && Vector2.Distance(torso.transform.position, Player.transform.position) > Range)
        {
            MovementDir(Player);
        }
        if (Vector2.Distance(torso.transform.position, enemy.transform.position) <= Range && aIGrab.grabbed)
        {
            Attack(aIGrab.grabbedObject, aIGrab.pickableObject, enemy);
        }
    }
}
