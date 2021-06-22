using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    public float TargetRotation;
    [HideInInspector]public Rigidbody2D rb;
    public float force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, TargetRotation, force));
    }
}
