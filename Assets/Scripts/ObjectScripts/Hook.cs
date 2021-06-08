using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] Rigidbody2D hookShotRb;
    [SerializeField] GameObject HookShot;
    Rigidbody2D rb;
    public DistanceJoint2D DisJoint;
    FixedJoint2D FJ;
    public bool isHooked;
    [HideInInspector] public float DmgMultiplier;
    [HideInInspector] public bool PickedByEnemy;
    public GameObject col;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHooked == false && !PickedByEnemy)
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
        else if(PickedByEnemy)
        {
            HookShot.GetComponent<HookShot>().UnShot();
        }

        rb = GetComponent<Rigidbody2D>();
        GameManager.Gm.StartCoroutine(GameManager.Gm.DoDamage(collision, rb, DmgMultiplier, PickedByEnemy));
    }
}
