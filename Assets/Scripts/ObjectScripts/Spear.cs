using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Spear : PickableObject
{
    [SerializeField] float force = 150;
    int attackCount = 0;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Update()
    {
        base.Update();
        sr.color = Color.Lerp(Color.white,Color.red,WeaponCoolDown / 3);
        CoolDownTimer();  
    }

    private void OnDisable()
    {
        WeaponCoolDown = 1;
        attackCount = 0;
    }

    public void Attack(Vector2 dir)
    {
        if (activateCoolDown == false)
        {
            attackCount += 1;
            WeaponCoolDown = 1;
            rb.velocity = dir * force; 
        }
    }

    void CoolDownTimer()
    {
        if(attackCount >= 3)
        {
            if(activateCoolDown == false)
                WeaponCoolDown = 3;

            activateCoolDown = true;
        }
        else
        {
            activateCoolDown = false;
        }

        if (WeaponCoolDown > 0)
        {
            WeaponCoolDown -= Time.deltaTime;
        }
        else
        {
            attackCount = 0;
        }
    }
}
