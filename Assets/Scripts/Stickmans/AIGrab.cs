using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGrab : MonoBehaviour
{
    AI ai;
    public bool grabbed;
    public PickableObject pickableObject;
    public GameObject grabbedObject;
    public int LayerOfTheWeapon;
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
        if (collision.TryGetComponent(out PickableObject Picked) && !Picked.Holded && !grabbed && enabled && Picked.rb.velocity.magnitude < 40)
        {
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
            PickUp(Picked, rb);
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
        Picked.ChangeProperties(true, true, LayerOfTheWeapon, true, ai.torso);
    }

    public void Drop()
    {
        grabbed = false;
        ai.Range = 5;
        Destroy(GetComponent<FixedJoint2D>());
        if (pickableObject != null)
        {
            pickableObject.ChangeProperties(false, false, LayerOfTheWeapon, false, null);
            pickableObject = null;
        }
    }
}
