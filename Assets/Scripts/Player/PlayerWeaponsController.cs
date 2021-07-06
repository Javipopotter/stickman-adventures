using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsController : MonoBehaviour
{
    public Grab grab;
    PickableObject weapon;

    void FixedUpdate()
    {
        if(grab.objectGrab && grab.grabbed)
        {
            weapon = grab.pickable;
            switch (weapon.ThisWeapon)
            {
                case PickableObject.Weapon.Sword:
                    Sword sword = weapon.GetComponent<Sword>();

                    if (Input.GetMouseButtonDown(0))
                    {
                        sword.MoveArms.Punch(SoundManager.SoundMan.SwordSwings);
                    }

                    if (Input.GetMouseButton(0))
                    {
                        sword.ChargeAttack();
                    }

                    if (sword.HoldTimer >= 1.5f && Input.GetMouseButtonUp(0))
                    {
                        sword.HoldTimer = 0;
                        StartCoroutine(sword.SpecialAttack());
                    }

                    if (!Input.GetMouseButton(0))
                    {
                        sword.HoldTimer = 0;
                    }
                    break;

                case PickableObject.Weapon.Spear:
                    if (Input.GetMouseButtonDown(0))
                        weapon.GetComponent<Spear>().Attack(GameManager.Gm.GetMouseVector(weapon.transform.position));
                    break;

                case PickableObject.Weapon.HookShot:
                    HookShot hookShot;
                    hookShot = weapon.GetComponent<HookShot>();
                    if (Input.GetMouseButton(0) && hookShot.e)
                        hookShot.Shoot();
                    else if (!Input.GetMouseButton(0) && !hookShot.e)
                        hookShot.UnShot();
                    break;

                case PickableObject.Weapon.Gun:
                    if (Input.GetMouseButton(0))
                        weapon.GetComponent<Gun>().Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    break;

                case PickableObject.Weapon.Boomerang:
                    if (Input.GetMouseButtonDown(0))
                    {
                        weapon.GetComponent<Boomerang>().Throw(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        grab.Drop();
                    }
                    break;
            } 
        }
    }
}
