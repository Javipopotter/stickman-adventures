using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StickmanLifesManager))]
public class AI : HumanoidController
{
    public GameObject Player;
    public GameObject enemy;
    Vector2 FeetRay;
    public Vector2 EnemyDir;
    public AIGrab aIGrab;
    float AttackCoolDown;
    public float Range = 5;
    public float PlayerPersonalDistance = 5;
    public Balance[] Arms;
    float n1;
    float n2;
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

        if(aIGrab.grabbed == false && enemy != null)
        {
            if (Vector2.Distance(torso.transform.position, enemy.transform.position) <= 20)
            {
                MovementDir(enemy, -2);
                ArmsMove(90, 300);
            }
            else
            {
                ArmsMove(0, 0);
            } 
        }   
    }

    void ArmsMove(float targetRot, float force)
    {
        foreach (Balance bal in Arms)
        {
            bal.TargetRotation = targetRot;
            bal.force = force;
        }
    }

    void ArmsMove(Vector3 targetRot, float force)
    {
        float rotz;
        rotz = Mathf.Atan2(targetRot.y, targetRot.x) * Mathf.Rad2Deg;
        foreach (Balance bal in Arms)
        {
            bal.TargetRotation = rotz;
            bal.force = force;
        }
    }
    
    public void Attack(GameObject p, PickableObject f, GameObject a)
    {
        EnemyDir = a.transform.position - torso.transform.position;
        EnemyDir.Normalize();
        float rotz;
        rotz = Mathf.Atan2(EnemyDir.y, EnemyDir.x) * Mathf.Rad2Deg;
        ArmsMove(rotz, 300);
        AttackCoolDown -= Time.fixedDeltaTime;
        if (AttackCoolDown <= 0)
        {
            switch (f.ThisWeapon)
            {
                case PickableObject.Weapon.Sword:
                    p.GetComponent<Sword>().MoveArms.Punch(-EnemyDir.x, SoundManager.SoundMan.SwordSwings);
                    AttackCoolDown = 0.5f;
                    n1 = 0.1f;
                    n2 = 0.2f;
                    break;
                case PickableObject.Weapon.Spear:                   
                    p.GetComponent<Spear>().Attack(EnemyDir);
                    AttackCoolDown = 0.5f;
                    n1 = 0.1f;
                    n2 = 0.5f;
                    break;
                case PickableObject.Weapon.HookShot:
                    p.GetComponent<HookShot>().Shoot();
                    AttackCoolDown = 0.5f;
                    n1 = 0;
                    n2 = AttackCoolDown;
                    break;
                case PickableObject.Weapon.Gun:
                    p.GetComponent<Gun>().Shoot(enemy.transform.position);
                    AttackCoolDown = 0;
                    n1 = 0;
                    n2 = AttackCoolDown;
                    break;
                case PickableObject.Weapon.Boomerang:
                    p.GetComponent<Boomerang>().Throw(enemy.transform.position);
                    aIGrab.Drop();
                    break;
            } 
        }
        if (n1 < AttackCoolDown && n2 > AttackCoolDown)
        {
            p.GetComponent<Rigidbody2D>().MoveRotation(rotz);
        }
    }

    public void MovementDir(GameObject chase, int moveDirection)
    {
        if (torso.transform.position.x < chase.transform.position.x)
        {
            Move(moveDirection);
        }
        else
        {
            Move(-moveDirection);
        }
    }

    public void Teleport(Vector2 pointToTeleport)
    {
        transform.position = pointToTeleport;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.localPosition = Vector3.zero;
            transform.GetChild(i).GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if(aIGrab.grabbed)
        {
            aIGrab.grabbedObject.transform.position = pointToTeleport;
        }
    }
}
