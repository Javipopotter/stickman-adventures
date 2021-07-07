using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] Rigidbody2D hookShotRb;
    [SerializeField] GameObject HookShot;
    PickableObject HookShootScript;
    Rigidbody2D rb;
    public DistanceJoint2D DisJoint;
    FixedJoint2D FJ;
    public bool isHooked;
    [HideInInspector] public float DmgMultiplier;
    [HideInInspector] public bool PickedByEnemy;
    public GameObject col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        HookShootScript = HookShot.GetComponent<HookShot>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHooked == false && !PickedByEnemy && collision.gameObject != HookShot)
        {
            col = collision.gameObject;
            isHooked = true;
            transform.parent = null;
            FJ = gameObject.AddComponent<FixedJoint2D>();
            DisJoint = gameObject.AddComponent<DistanceJoint2D>();
            if (collision.gameObject.TryGetComponent(out Rigidbody2D ColRb))
            {
                FJ.connectedBody = ColRb;
            }
            DisJoint.connectedBody = hookShotRb;
            DisJoint.autoConfigureDistance = false;
            DisJoint.maxDistanceOnly = true;
        }
        else if(PickedByEnemy && collision.gameObject != HookShot)
        {
            HookShot.GetComponent<HookShot>().UnShot();
        }

        GameManager.Gm.StartCoroutine(GameManager.Gm.DoDamage(collision, rb, DmgMultiplier, 20, HookShootScript.Holder));
    }
}
