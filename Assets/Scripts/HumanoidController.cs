using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidController : MonoBehaviour
{
    public LayerMask floor;
    public GameObject torso;
    public Rigidbody2D rb;
    public Animator an;
    public float speed = 10;
    public float jumpForce;
    public bool canJump;

    public void CheckCanJump()
    {
        canJump = Physics2D.Raycast(torso.transform.position, Vector2.down, 5, floor);
    }
    public void Move(float s)
    {
        rb.velocity = new Vector2(s * speed, rb.velocity.y);
        an.Play("Move");
    }

    public void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
