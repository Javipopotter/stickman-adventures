using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGrab : MonoBehaviour
{
    AI ai;
    public bool grabbed;
    public PickableObject pickableObject;
    public GameObject grabbedObject;
    [HideInInspector]public bool IsFriend;
    private void Awake()
    {
        ai = GetComponentInParent<AI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GrabWeapon(collision);
    }

    private void GrabWeapon(Collider2D collision)
    {
        if (collision.TryGetComponent(out PickableObject Picked) && !Picked.Holded && !grabbed && collision.GetComponent<Rigidbody2D>().velocity.magnitude < 20 && enabled)
        {
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
            PickUp(Picked, rb);
            if (!IsFriend)
            {
                Picked.SetLayers(6);
            }
        }
    }

    private void PickUp(PickableObject Picked, Rigidbody2D rb)
    {
        grabbed = true;
        pickableObject = Picked;
        grabbedObject = Picked.gameObject;
        ai.Range = Picked.range;
        FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
        fj.autoConfigureConnectedAnchor = false;
        fj.connectedBody = rb;
        Picked.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        fj.anchor = fj.connectedAnchor = Vector2.zero;
        Picked.sr.material = Picked.InitMaterial;
        if (IsFriend)
            Picked.ChangeProperties(true, true, GameManager.Gm.AlliesColliders);
        else
            Picked.ChangeProperties(true, true, GameManager.Gm.EnemyColliders);
    }

    public void Drop()
    {
        grabbed = false;
        ai.Range = 5;
        Destroy(GetComponent<FixedJoint2D>());
        if (pickableObject != null)
        {
            if(IsFriend)
                pickableObject.ChangeProperties(false, false, GameManager.Gm.AlliesColliders);
            else
                pickableObject.ChangeProperties(false, false, GameManager.Gm.EnemyColliders);
            pickableObject.SetLayers(9);
            pickableObject = null;
        }
    }
}
