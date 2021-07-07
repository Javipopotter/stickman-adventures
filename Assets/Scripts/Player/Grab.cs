using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public bool holding;
    public bool grabbed;
    public bool objectGrab;
    bool f = true;
    public bool Grabs;
    public PickableObject pickable;
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && grabbed)
        {
            Drop();
            f = false;
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
                Drop();
            }
        }

        //if(holding || grabbed)
        //{
        //    col.enabled = true;
        //}
        //else
        //{
        //    col.enabled = false;
        //}
    }
     private void OnCollisionEnter2D(Collision2D collision)
    {
        if(holding && !grabbed && !collision.gameObject.GetComponent<PickableObject>())
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
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GrabWeapon(collision, false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PickableObject Picked) && Grabs)
        {
            ObjectSelect(Picked, Picked.InitMaterial);
        }
    }

    public void GrabWeapon(Collider2D collision, bool Invoke)
    {
        if (collision.TryGetComponent(out PickableObject Picked))
        {
            if ((holding || Invoke) && !grabbed && Grabs && !collision.GetComponent<PickableObject>().Holded)
            {
                Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
                PickUp(Picked, rb);
            }
            else if (Grabs)
            {
                ObjectSelect(Picked, GameManager.Gm.highLight);
            }
        }
    }

    private void ObjectSelect(PickableObject Picked, Material mat)
    {
        if (!Picked.Holded)
        {
            GameManager.Gm.highLight.SetFloat("_OutlineThickness", Picked.OutLineThickness);
            Picked.sr.material = mat;
        }
    }

    private void PickUp(PickableObject Picked, Rigidbody2D rb)
    {
        pickable = Picked;
        grabbed = true;
        objectGrab = true;
        FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
        fj.autoConfigureConnectedAnchor = false;
        fj.connectedBody = rb;
        Picked.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        fj.anchor = fj.connectedAnchor = Vector2.zero;
        Picked.sr.material = Picked.InitMaterial;
        ActiveDeactivePunches(Picked.CanPunch, Picked.BlockArm);
        Picked.ChangeProperties(true, false, 10, GameManager.Gm.PlayerTorso);
    }

    public void Drop()
    {
        Destroy(GetComponent<FixedJoint2D>());
        grabbed = false;
        if (Grabs && pickable != null)
        {
            ActiveDeactivePunches(true, false);
            pickable.ChangeProperties(false, false, 10, null);
            pickable = null;
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
