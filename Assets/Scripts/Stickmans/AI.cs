using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : HumanoidController
{
    GameObject Player;
    Vector2 FeetRay;
    void Start()
    {
        Player = GameManager.Gm.PlayerTorso;
        rb = torso.GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        FeetRay = new Vector2(torso.transform.position.x - 2, torso.transform.position.y - 1);
        CheckCanJump();
        if(Vector2.Distance(torso.transform.position, Player.transform.position) < 120 && Vector2.Distance(torso.transform.position, Player.transform.position) > 5)
        {
            if(torso.transform.position.x < Player.transform.position.x)
            {
                Move(1);
            }
            else
            {
                Move(-1);
            }
        }
        if(Physics2D.Raycast(FeetRay, Vector2.right, 4, floor))
        {
            Jump();
        }
    }
}
