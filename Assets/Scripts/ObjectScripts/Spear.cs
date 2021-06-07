using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Spear : PickableObject
{
    [SerializeField] float force = 150;
    [SerializeField] float coolDown = 3;
    [SerializeField] bool activeCoolDown;
    [SerializeField] int attackCount = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        InitMaterial = sr.material;
    }
    private void Update()
    {
        if (Holded)
        {
            if (!IsPickedByEnemy)
            {
                if (Input.GetMouseButtonDown(0) && activeCoolDown == false)
                {
                    Attack(GameManager.Gm.GetMouseVector(transform.position) * force);
                }

                coolDownTimer();  
            }
        }
    }

    public void Attack(Vector2 dir)
    {
        attackCount += 1;
        coolDown = 1;
        rb.velocity = dir;
    }

    void coolDownTimer()
    {
        if(attackCount >= 3)
        {
            if(activeCoolDown == false)
                coolDown = 3;

            activeCoolDown = true;
        }
        else
        {
            activeCoolDown = false;
        }

        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
        }
        else
        {
            attackCount = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.Gm.StartCoroutine(GameManager.Gm.DoDamage(collision, rb, DmgMultiplier, IsPickedByEnemy));
    }
}
