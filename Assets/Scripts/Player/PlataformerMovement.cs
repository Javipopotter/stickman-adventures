using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
public class PlataformerMovement : HumanoidController
{
    Vector2 StartPos;
    // Start is called before the first frame update
    void Awake()
    {
        rb = torso.GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        StartPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float translation = Input.GetAxisRaw("Horizontal");

        if (translation != 0)
            Move(translation);
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            rb.velocity = new Vector2(0, rb.velocity.y);
        else if(Input.GetKey(KeyCode.S))
        {
            an.Play("Ragdoll");
        }

        CheckCanJump();
    }


    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jump()); ;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DeathZone"))
        {
            transform.position = StartPos;
        }
    }
}
