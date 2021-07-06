using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : PickableObject
{
    public MoveArms MoveArms;
    public float HoldTimer;
    bool e = true;
    public override void Awake()
    {
        base.Awake();
        MoveArms = GetComponent<MoveArms>();
    }

    void Update()
    {
        if (e)
        {
            sr.color = Color.Lerp(Color.white, Color.yellow, HoldTimer / 1.5f);
            if(!Holded)
            {
                HoldTimer = 0;
            }
        }
    }

    private void OnDisable()
    {
        HoldTimer = 0;
    }

    public IEnumerator SpecialAttack()
    {
        e = false;
        MoveArms.punch = false;
        CanGetChanged = false;
        sr.color = Color.yellow;
        gameObject.layer = 7;
        MoveArms.force *= 4;
        transform.localScale = transform.localScale * 2;
        MoveArms.Punch(SoundManager.SoundMan.SwordSwings);
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
