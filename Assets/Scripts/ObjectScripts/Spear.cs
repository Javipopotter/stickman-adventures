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
            if (Input.GetMouseButtonDown(0) && activeCoolDown == false)
            {
                attackCount += 1;
                coolDown = 1;
                Vector2 dir = GameManager.Gm.GetMouseVector(transform.position) * force;
                rb.AddForce(dir, ForceMode2D.Impulse);
            }

            coolDownTimer(); 
        }
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
        if(collision.gameObject.TryGetComponent(out HingeJoint2D hinge) && collision.gameObject.CompareTag("enemy") && rb.velocity.magnitude > 20)
        {
            Destroy(hinge);
        }
    }
}
