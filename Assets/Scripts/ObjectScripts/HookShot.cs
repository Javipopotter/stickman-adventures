using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShot : PickableObject
{
    public GameObject Hook;
    public Hook HookScript;
    MoveArms moveArms;
    [SerializeField] float HookForce;
    Rigidbody2D hookRb;
    public GameObject OriginalHookPos;
    LineRenderer lineRenderer;
    bool e = true;
    public Vector2 dir;
    bool HookGatherUp;
    void Awake()
    {
        HookScript = Hook.GetComponent<Hook>();
        moveArms = GetComponent<MoveArms>();
        lineRenderer = GetComponent<LineRenderer>();
        OriginalHookPos.transform.position = Hook.transform.position;
        hookRb = Hook.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        InitMaterial = sr.material;
        HookScript.DmgMultiplier = DmgMultiplier;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, OriginalHookPos.transform.position);
        lineRenderer.SetPosition(1, Hook.transform.position);
        if(Holded)
        {
            Hook.GetComponent<Hook>().PickedByEnemy = IsPickedByEnemy;
            if (!IsPickedByEnemy)
            {
                if (Input.GetMouseButton(0) && e)
                    Shot();
                else if (!Input.GetMouseButton(0) && e == false)
                    UnShot(); 
            }
            else
            {
                moveArms.enabled = false;
            }
        }

        if(e == false)
        {
            dir = Hook.transform.position - transform.position;
            dir.Normalize();
            float rotz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.MoveRotation(rotz);
            if (Vector2.Distance(transform.position, Hook.transform.position) > 5)
            {
                if (HookScript.isHooked && HookScript.col.TryGetComponent(out Rigidbody2D colRb))
                {
                    colRb.AddForce(-dir * 1500 * Time.deltaTime, ForceMode2D.Impulse);
                }
                else if (HookScript.isHooked)
                {
                    rb.AddForce(dir * 1500 * Time.deltaTime, ForceMode2D.Impulse);
                } 
            }
        }

        if(HookGatherUp)
        {
            hookRb.velocity = -dir * HookForce * 2;
            if(Vector2.Distance(Hook.transform.position, transform.position) < 3)
            {
                e = true;
                HookGatherUp = false;
                GetComponent<MoveArms>().enabled = true;
                Hook.transform.position = OriginalHookPos.transform.position;
                Hook.transform.rotation = OriginalHookPos.transform.rotation;
                Hook.transform.parent = gameObject.transform;
                hookRb.isKinematic = true;
            }
        }
    }

    public void Shot()
    {
        if (!HookGatherUp)
        {
            e = false;
            hookRb.isKinematic = false;
            Hook.GetComponent<BoxCollider2D>().enabled = true;
            hookRb.velocity = transform.right * HookForce;
            hookRb.freezeRotation = true;
            GetComponent<MoveArms>().enabled = false; 
        }
    }

    public void UnShot()
    {
        HookGatherUp = true;
        Hook.GetComponent<BoxCollider2D>().enabled = false;
        HookScript.isHooked = false;
        hookRb.freezeRotation = false;
        Destroy(Hook.GetComponent<DistanceJoint2D>());
        Destroy(Hook.GetComponent<FixedJoint2D>());
        Hook.transform.rotation = OriginalHookPos.transform.rotation;
    }
}
