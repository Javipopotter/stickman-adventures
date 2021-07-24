using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendAI : AI
{
    public override void Awake()
    {
        base.Awake();
        GameManager.Gm.Friends.Add(this);
        GameManager.Gm.UpdateAllies();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameManager.Gm.PlayerEnemy != null && aIGrab[0].grabbed)
        {
            enemy = GameManager.Gm.PlayerEnemy;
            foreach (AIGrab aIGrab in aIGrab)
            {
            if (Vector2.Distance(torso.transform.position, Player.transform.position) < 120 && Vector2.Distance(GameManager.Gm.PlayerEnemy.transform.position, torso.transform.position) > aIGrab.pickableObject.range)
            {
                MovementDir(enemy, 1);
            }

                if (Vector2.Distance(torso.transform.position, enemy.transform.position) <= aIGrab.pickableObject.range)
                {
                        Attack(aIGrab.grabbedObject, enemy, aIGrab); 
                }
            }
        }
        else if (Vector2.Distance(torso.transform.position, Player.transform.position) > PlayerPersonalDistance)
        {
            MovementDir(Player, 1);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Vector2.Distance(Player.transform.position, torso.transform.position) > 75)
        {
            Teleport(new Vector2(Player.transform.position.x, Player.transform.position.y + 10));
        }
    }
}
