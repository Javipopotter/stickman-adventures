using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public virtual void Awake()
    {
        aIGrab = GetComponentInChildren<AIGrab>();
        rb = torso.GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        Player = GameManager.Gm.PlayerTorso;
    }

    public void GetEnemy(GameObject en)
    {
        if(enemy == null)
        {
            enemy = en;
        }
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
        float n = 0;
        EnemyDir = a.transform.position - torso.transform.position;
        EnemyDir.Normalize();
        float rotz;
        rotz = Mathf.Atan2(EnemyDir.y, EnemyDir.x) * Mathf.Rad2Deg;
        AttackCoolDown -= Time.fixedDeltaTime;
        if (AttackCoolDown <= 0)
        {
            switch (f.ThisWeapon)
            {
                case PickableObject.Weapon.Sword:
                    p.GetComponent<Sword>().MoveArms.EnemyPunch(-EnemyDir.x);
                    AttackCoolDown = 0.5f;
                    n = 0.1f;
                    break;
                case PickableObject.Weapon.Spear:                   
                    p.GetComponent<Spear>().Attack(EnemyDir);
                    AttackCoolDown = 0.5f;
                    n = 0.1f;
                    break;
                case PickableObject.Weapon.HookShot:
                    p.GetComponent<HookShot>().Shot();
                    AttackCoolDown = 0.5f;
                    n = 0;
                    break;
                case PickableObject.Weapon.Gun:
                    p.GetComponent<Gun>().Shoot();
                    AttackCoolDown = 0.5f;
                    n = 0;
                    break;
            } 
        }
        if (n < AttackCoolDown)
        {
            p.GetComponent<Rigidbody2D>().MoveRotation(rotz); 
        }
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
