using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendAI : AI
{
    public override void Awake()
    {
        base.Awake();
        aIGrab.IsFriend = true;
        GameManager.Gm.Friends.Add(this);
        GameManager.Gm.UpdateAllies();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameManager.Gm.PlayerEnemy != null)
        {
            enemy = GameManager.Gm.PlayerEnemy;
            if (Vector2.Distance(GameManager.Gm.PlayerEnemy.transform.position, transform.position) > Range)
            {
                MovementDir(enemy);

            }

            if (Vector2.Distance(torso.transform.position, enemy.transform.position) <= Range && aIGrab.grabbed)
            {
                Attack(aIGrab.grabbedObject, aIGrab.pickableObject, enemy);
            }
        }
        else if (Vector2.Distance(torso.transform.position, Player.transform.position) > PlayerPersonalDistance)
        {
            MovementDir(Player);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
