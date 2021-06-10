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
    [SerializeField]bool e = true;
    bool GatheredUp;
    public Vector2 dir;
    bool HookGatherUp;
    public override void Awake()
    {
        base.Awake();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Hook.GetComponent<Collider2D>());
        HookScript = Hook.GetComponent<Hook>();
        moveArms = GetComponent<MoveArms>();
        lineRenderer = GetComponent<LineRenderer>();
        OriginalHookPos.transform.position = Hook.transform.position;
        hookRb = Hook.GetComponent<Rigidbody2D>();      
        HookScript.DmgMultiplier = DmgMultiplier;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, OriginalHookPos.transform.position);
        lineRenderer.SetPosition(1, Hook.transform.position);
        if(Holded)
        {
            HookScript.PickedByEnemy = IsPickedByEnemy;
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
        else if(!GatheredUp)
        {
            UnShot();
        }
        else
        {
            hookRb.velocity = Vector2.zero;
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
                    colRb.AddForce(1500 * Time.deltaTime * -dir, ForceMode2D.Impulse);
                }
                else if (HookScript.isHooked)
                {
                    rb.AddForce(1500 * Time.deltaTime * dir, ForceMode2D.Impulse);
                } 
            }
        }

        if(HookGatherUp)
        {
            hookRb.velocity = 2 * HookForce * -dir;
            if(Vector2.Distance(Hook.transform.position, transform.position) < 3)
            {
                GatheredUp = true;
                e = true;
                HookGatherUp = false;
                Hook.transform.SetPositionAndRotation(OriginalHookPos.transform.position, OriginalHookPos.transform.rotation);
                Hook.transform.parent = gameObject.transform;
                hookRb.isKinematic = true;
                if (Holded)
                    GetComponent<MoveArms>().enabled = true;
            }
        }

        if(Vector2.Distance(transform.position, Hook.transform.position) > 80)
        {
            UnShot();
        }
    }

    public void Shot()
    {
        if (!HookGatherUp)
        {
            GatheredUp = false;
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
