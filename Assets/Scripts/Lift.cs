using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    Vector2 InitPos;
    Vector2 Dist;
    bool GoBack;
    Vector2 dir;
    [SerializeField] GameObject Ref;
    Rigidbody2D rb;
    enum States
    {
        Stop,
        Moving
    }
    States LiftState = States.Stop;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitPos = transform.position;
        Dist = Ref.transform.position;
        dir = transform.position - Ref.transform.position;
        dir.Normalize();
    }

    private void Update()
    {
        if(LiftState == States.Moving)
        {
            Vector2 ParadePos;

            if (GoBack)
                ParadePos = InitPos;
            else
                ParadePos = Dist;

            rb.MovePosition(rb.position - dir / 2);
            if(Vector2.Distance(transform.position, ParadePos) < 1)
            {
                transform.position = ParadePos;
                GoBack = !GoBack;
                dir = -dir;
                LiftState = States.Stop;
            }
        }
    }

    public void Move()
    {
        if(LiftState == States.Stop)
        {
            LiftState = States.Moving;
        }
    }
}
