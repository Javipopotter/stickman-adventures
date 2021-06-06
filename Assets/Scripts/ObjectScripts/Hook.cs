using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] Rigidbody2D hookShotRb;
    [SerializeField] GameObject HookShot;
    Rigidbody2D rb;
    public DistanceJoint2D DisJoint;
    [HideInInspector] public bool isHooked;
    [HideInInspector] public float DmgMultiplier;
    [HideInInspector] public bool PickedByEnemy;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("floor") && isHooked == false)
        {
            isHooked = true;
            transform.parent = null;
            gameObject.AddComponent<FixedJoint2D>();
            DisJoint = gameObject.AddComponent<DistanceJoint2D>();
            DisJoint.enabled = true;
            DisJoint.connectedBody = hookShotRb;
        }

        rb = GetComponent<Rigidbody2D>();
        GameManager.Gm.StartCoroutine(GameManager.Gm.DoDamage(collision, rb, DmgMultiplier, PickedByEnemy));
    }
}
