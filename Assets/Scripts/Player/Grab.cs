using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public bool holding;
    bool grabbed;
    bool objectGrab;
    bool f = true;
    public bool Grabs;
    public PickableObject pickable;
    bool CanPunch, BlockArm;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && grabbed)
        {
            Drop();
            f = false;
            Destroy(GetComponent<FixedJoint2D>());
        }

        if(Input.GetMouseButton(1) && f)
        {
            holding = true;
        }
        else
        {
            holding = false;
        }

        if(Input.GetMouseButtonUp(1))
        {
            f = true;
            if(objectGrab == false)
            {
                Destroy(GetComponent<FixedJoint2D>());
                grabbed = false;
            }
        }
    }
     private void OnCollisionEnter2D(Collision2D collision)
    {
        if(holding && !grabbed && collision.gameObject.GetComponent<PickableObject>() == null)
        {
            grabbed = true;
            objectGrab = false;
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb;
            }
            else
            {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.breakForce = 120000;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (holding && !grabbed && collision.transform.CompareTag("Pickable") && Grabs && !collision.GetComponent<PickableObject>().Holded)
        {
            grabbed = true;
            objectGrab = false;
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb;
            }
            else
            {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
            }

            if (collision.gameObject.TryGetComponent(out PickableObject Picked))
            {
                BlockArm = Picked.BlockArm;
                CanPunch = Picked.CanPunch;
                Picked.sr.material = Picked.InitMaterial;
                objectGrab = true;
                pickable = Picked;
                ActiveDeactivePunches(CanPunch, BlockArm);
                Picked.ChangeProperties(gameObject, true, false);
            }
        }
        else if(collision.transform.CompareTag("Pickable") && Grabs)
        {
            if (collision.gameObject.TryGetComponent(out PickableObject Picked) && !Picked.Holded)
            {
                GameManager.Gm.highLight.SetFloat("_OutlineThickness", Picked.OutLineThickness);
                Picked.sr.material = GameManager.Gm.highLight;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Pickable") && Grabs)
        {
            if (collision.gameObject.TryGetComponent(out PickableObject Picked))
            {
                Picked.sr.material = Picked.InitMaterial;
            }
        }
    }

    private void Drop()
    {
        if(Grabs)
        {
            grabbed = false;
            ActiveDeactivePunches(true, false);
            pickable.ChangeProperties(gameObject, false, false);
        }
    }

    public void ActiveDeactivePunches(bool p, bool a)
    {
        foreach (MoveArms arms in transform.parent.GetComponentsInChildren<MoveArms>())
        {
            arms.punch = p;
            arms.armLock = a;
        }
    }
}
