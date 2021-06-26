using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : PickableObject
{
    public MoveArms MoveArms;
    [SerializeField] float HoldTimer;
    bool e = true;
    public override void Awake()
    {
        base.Awake();
        MoveArms = GetComponent<MoveArms>();
    }

    public override void Update()
    {
        base.Update();
        if (e)
        {
            sr.color = Color.Lerp(Color.white, Color.yellow, HoldTimer / 1.5f);
            if (Holded && PickedByAI == false)
            {

                if (Input.GetMouseButton(0))
                {
                    ChargeAttack();
                }

                if (HoldTimer >= 1.5f && Input.GetMouseButtonUp(0))
                {
                    HoldTimer = 0;
                    StartCoroutine(Attack());
                }

                if (!Input.GetMouseButton(0))
                {
                    HoldTimer = 0;
                }
            }
            else
            {
                HoldTimer = 0;
            }
        }
    }

    private void OnDisable()
    {
        HoldTimer = 0;
    }

    public IEnumerator Attack()
    {
        e = false;
        MoveArms.punch = false;
        CanGetChanged = false;
        sr.color = Color.yellow;
        gameObject.layer = 7;
        MoveArms.force *= 4;
        transform.localScale = transform.localScale * 2;
        MoveArms.Punch();
        yield return new WaitForSeconds(0.3f);
        e = true;
        MoveArms.punch = true;
        CanGetChanged = true;
        if (Holded)
        {
            gameObject.layer = 9; 
        }
        MoveArms.force /= 4;
        transform.localScale = transform.localScale / 2;
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
}
