using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : HumanoidController
{
    GameObject Player;
    Vector2 FeetRay;
    Vector2 PlayerDir;
    AIGrab aIGrab;
    float AttackCoolDown;
    float Range = 5;
    void Awake()
    {
        aIGrab = GetComponentInChildren<AIGrab>();
        Player = GameManager.Gm.PlayerTorso;
        rb = torso.GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        FeetRay = new Vector2(torso.transform.position.x - 2, torso.transform.position.y - 1);
        CheckCanJump();
        if(Vector2.Distance(torso.transform.position, Player.transform.position) < 120 && Vector2.Distance(torso.transform.position, Player.transform.position) > Range)
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
            StartCoroutine(Jump());
        }

        if(Vector2.Distance(torso.transform.position, Player.transform.position) < Range && aIGrab.grabbed)
        {
            Attack(aIGrab.grabbedObject, aIGrab.pickableObject);
        }
    }

    void Attack(GameObject p, PickableObject f)
    {
        PlayerDir = Player.transform.position - torso.transform.position;
        PlayerDir.Normalize();
        float rotz;
        rotz = Mathf.Atan2(PlayerDir.y, PlayerDir.x) * Mathf.Rad2Deg;
        AttackCoolDown -= Time.fixedDeltaTime;
        if (AttackCoolDown <= 0)
        {
            switch (f.ThisWeapon)
            {
                case PickableObject.Weapon.Sword:
                    p.GetComponent<Sword>().MoveArms.EnemyPunch(-PlayerDir.x);
                    AttackCoolDown = 0.5f;
                    break;
                case PickableObject.Weapon.Spear:                   
                    p.GetComponent<Spear>().Attack(PlayerDir);
                    AttackCoolDown = 0.5f;
                    break;
                case PickableObject.Weapon.HookShot:
                    p.GetComponent<HookShot>().Shot();                    
                    AttackCoolDown = 0.5f;
                    break;
            } 
        }
        if (f.ThisWeapon == PickableObject.Weapon.HookShot)
            p.GetComponent<Rigidbody2D>().MoveRotation(rotz);
    }

    public void CheckRange(float range)
    {
        Range = range;
    }
}
