using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : HumanoidController
{
    public GameObject Player;
    GameObject enemy;
    Vector2 FeetRay;
    Vector2 PlayerDir;
    AIGrab aIGrab;
    float AttackCoolDown;
    public float Range = 5;
    public bool Friend;
    public float Distance;
    void Awake()
    {
        aIGrab = GetComponentInChildren<AIGrab>();
        aIGrab.IsFriend = Friend;
        rb = torso.GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        Player = GameManager.Gm.PlayerTorso;
        if(!Friend)
        {
            enemy = Player;
        }
    }
    void FixedUpdate()
    {
        FeetRay = new Vector2(torso.transform.position.x - 2, torso.transform.position.y - 1);
        CheckCanJump();

        if (GameManager.Gm.PlayerEnemy != null && Friend)
        {
            if (Vector2.Distance(GameManager.Gm.PlayerEnemy.transform.position, transform.position) > Range)
            {
                enemy = GameManager.Gm.PlayerEnemy;
                MovementDir(enemy);

            }

            if (Vector2.Distance(torso.transform.position, enemy.transform.position) < 10 && aIGrab.grabbed)
            {
                Attack(aIGrab.grabbedObject, aIGrab.pickableObject, enemy);
            }
        }
        else if(Vector2.Distance(torso.transform.position, Player.transform.position) < 120 && Vector2.Distance(torso.transform.position, Player.transform.position) > Range)
        {
            MovementDir(Player);
        }

        if(Physics2D.Raycast(FeetRay, Vector2.right, 4, floor))
        {
            StartCoroutine(Jump());
        }

        if (enemy != null)
        {
            if (Vector2.Distance(torso.transform.position, enemy.transform.position) <= Range && aIGrab.grabbed && !Friend)
            {
                Attack(aIGrab.grabbedObject, aIGrab.pickableObject, enemy);
            }  
        }

    }
    
    void Attack(GameObject p, PickableObject f, GameObject a)
    {
        PlayerDir = a.transform.position - torso.transform.position;
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
        if (AttackCoolDown < 0.1f || f.ThisWeapon == PickableObject.Weapon.HookShot)
            p.GetComponent<Rigidbody2D>().MoveRotation(rotz);
    }

    public void CheckRange(float range)
    {
        Range = range;
    }

    void MovementDir(GameObject chase)
    {
        if (torso.transform.position.x < chase.transform.position.x)
        {
            Move(1);
        }
        else
        {
            Move(-1);
        }
    }
}
