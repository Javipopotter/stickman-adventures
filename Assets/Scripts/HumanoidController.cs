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
    bool t;
    [HideInInspector] public float OriginalSpeed, OriginalJumpForce;
    public int LegsCount;
    private void Start()
    {
        foreach(PartsLifes parts in GetComponentsInChildren<PartsLifes>())
        {
            if (parts.IsLeg)
                LegsCount++;
        }
        OriginalSpeed = speed;
        OriginalJumpForce = jumpForce;
    }

    public void CheckCanJump()
    {
        canJump = Physics2D.Raycast(torso.transform.position, Vector2.down, 5, floor);
    }
    public void Move(float s)
    {
        rb.velocity = new Vector2(s * speed, rb.velocity.y);
        an.Play("Move");
    }

    public IEnumerator Jump()
    {
        if (canJump && t)
        {
            t = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        yield return new WaitForSeconds(0.3f);
        t = true;
    }
}
