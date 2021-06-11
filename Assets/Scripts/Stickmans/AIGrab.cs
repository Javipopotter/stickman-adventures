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
        if(collision.TryGetComponent(out PickableObject pickable) && !pickable.Holded && !grabbed && collision.GetComponent<Rigidbody2D>().velocity.magnitude < 20 && enabled)
        {
            grabbed = true;
            pickableObject = pickable;
            grabbedObject = pickable.gameObject;
            GameManager.Gm.UpdateColliders(grabbedObject.GetComponent<Collider2D>(), true, IsFriend);
            ai.Range = pickable.range;
            if (!IsFriend)
            {
                collision.transform.gameObject.layer = 6;
                pickable.SetLayers(6); 
            }
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb;
            }
            pickable.ChangeProperties(gameObject, true, true);
        }
    }

    public void Drop()
    {
        grabbed = false;
        ai.Range = 5;
        Destroy(GetComponent<FixedJoint2D>());
        if (pickableObject != null)
        {
            pickableObject.Holded = false;
            GameManager.Gm.UpdateColliders(grabbedObject.GetComponent<Collider2D>(), false, IsFriend);
            pickableObject.transform.gameObject.layer = 9;
            pickableObject.SetLayers(9);
        }
    }
}
