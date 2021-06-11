using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : PickableObject
{
    Animator an;
    public MoveArms MoveArms;
    [SerializeField] float HoldTimer;
    bool e = true;
    public override void Awake()
    {
        base.Awake();
        MoveArms = GetComponent<MoveArms>();
        an = GetComponent<Animator>();
    }

    void Update()
    {
        if (e)
        {
            sr.color = Color.Lerp(Color.white, Color.yellow, HoldTimer / 1.5f); 
        }
        if (Holded)
        {

            if (Input.GetMouseButton(0) && !IsPickedByEnemy)
            {
                ChargeAttack();
            }

            if (HoldTimer >= 1.5f && Input.GetMouseButtonUp(0) && !IsPickedByEnemy)
            {
                HoldTimer = 0;
                Attack();
            }

            if (!Input.GetMouseButton(0) && !IsPickedByEnemy)
            {
                HoldTimer = 0;
            } 
        }
        else
        {
            if(HoldTimer >= 1.5f)
            {
                MoveArms.Punch();
            }
            HoldTimer = 0;
        }
    }

    private void OnDisable()
    {
        HoldTimer = 0;
    }

    public void Attack()
    {
        e = false;
        CanGetChanged = false;
        sr.color = Color.yellow;
        gameObject.layer = 7;
        MoveArms.force *= 4;
        transform.localScale = transform.localScale * 2;
        MoveArms.Punch();
        an.Play("SwordAtk");
    }
    public void ChargeAttack()
    {
        if (HoldTimer >= 0.1f)
        {
            Vector2 dir;
            float rotz;
            dir = GameManager.Gm.GetMouseVector(transform.position);
            rotz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.MoveRotation(rotz);
        }
        HoldTimer += Time.deltaTime;
    }

    public void ResetForce()
    {
        e = true;
        CanGetChanged = true;
        gameObject.layer = 9;
        MoveArms.force /= 4;
        transform.localScale = transform.localScale / 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.Gm.StartCoroutine(GameManager.Gm.DoDamage(collision, rb, DmgMultiplier, IsPickedByEnemy));
    }
}
