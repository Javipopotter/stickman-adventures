using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : PickableObject
{
    Animator an;
    MoveArms MoveArms;
    [SerializeField] float HoldTimer;
    bool e = true;
    private void Start()
    {
        MoveArms = GetComponent<MoveArms>();
        an = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        InitMaterial = sr.material;
    }

    private void Update()
    {
        if (e)
        {
            sr.color = Color.Lerp(Color.white, Color.yellow, HoldTimer / 1.5f); 
        }
        if (Holded)
        {

            if (Input.GetMouseButton(0))
            {
                ChargeAttack();
            }

            if (HoldTimer >= 1.5f && Input.GetMouseButtonUp(0))
            {
                Atack();
            }

            if (!Input.GetMouseButton(0))
            {
                HoldTimer = 0;
            } 
        }
        else
        {
            HoldTimer = 0;
            if(HoldTimer >= 1.5f)
            {
                MoveArms.Punch();
            }
        }
    }

    public void Atack()
    {
        e = false;
        sr.color = Color.yellow;
        gameObject.layer = 7;
        MoveArms.force = MoveArms.force * 4;
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
        gameObject.layer = 9;
        MoveArms.force = MoveArms.force / 4;
        transform.localScale = transform.localScale / 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.Gm.StartCoroutine(GameManager.Gm.DoDamage(collision, rb, DmgMultiplier, IsPickedByEnemy));
    }
}
