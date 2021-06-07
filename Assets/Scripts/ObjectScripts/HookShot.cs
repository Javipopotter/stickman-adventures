using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShot : PickableObject
{
    public GameObject Hook;
    [SerializeField] float HookForce;
    Rigidbody2D hookRb;
    public GameObject OriginalHookPos;
    LineRenderer lineRenderer;
    bool e = true;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        OriginalHookPos.transform.position = Hook.transform.position;
        hookRb = Hook.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        InitMaterial = sr.material;
        Hook.GetComponent<Hook>().DmgMultiplier = DmgMultiplier;
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
        }

        if(e == false)
        {
            Vector2 dir = Hook.transform.position - transform.position;
            dir.Normalize();
            float rotz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.MoveRotation(rotz);
        }
    }

    void Shot()
    {
        e = false;
        hookRb.isKinematic = false;
        Hook.GetComponent<BoxCollider2D>().enabled = true;
        hookRb.velocity = transform.right * HookForce;
        hookRb.freezeRotation = true;
        GetComponent<MoveArms>().enabled = false;
    }

    void UnShot()
    {
        e = true;
        Hook.GetComponent<Hook>().isHooked = false;
        GetComponent<MoveArms>().enabled = true;
        hookRb.freezeRotation = false;
        Hook.GetComponent<BoxCollider2D>().enabled = false;
        hookRb.isKinematic = true;
        Hook.transform.position = OriginalHookPos.transform.position;
        Hook.transform.rotation = OriginalHookPos.transform.rotation;
        Destroy(Hook.GetComponent<DistanceJoint2D>());
        Hook.transform.parent = gameObject.transform;
        Destroy(Hook.GetComponent<FixedJoint2D>());
    }
}
