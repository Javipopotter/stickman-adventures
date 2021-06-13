using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : HumanoidController
{
    public GameObject Player;
    public GameObject enemy;
    Vector2 FeetRay;
    Vector2 PlayerDir;
    public AIGrab aIGrab;
    float AttackCoolDown;
    public float Range = 5;
    public float PlayerPersonalDistance = 5;
    public virtual void Awake()
    {
        aIGrab = GetComponentInChildren<AIGrab>();
        rb = torso.GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        Player = GameManager.Gm.PlayerTorso;
    }
    public virtual void FixedUpdate()
    {
        FeetRay = new Vector2(torso.transform.position.x - 2, torso.transform.position.y - 1);
        CheckCanJump();

        if(Physics2D.Raycast(FeetRay, Vector2.right, 4, floor))
        {
            StartCoroutine(Jump());
        }
    }
    
    public void Attack(GameObject p, PickableObject f, GameObject a)
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

    public void MovementDir(GameObject chase)
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
