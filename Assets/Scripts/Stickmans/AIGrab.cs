using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGrab : MonoBehaviour
{
    bool grabbed;
    public PickableObject pickableObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PickableObject pickable) && !pickable.Holded && !grabbed && collision.GetComponent<Rigidbody2D>().velocity.magnitude < 20)
        {
            pickableObject = pickable;
            grabbed = true;
            collision.transform.gameObject.layer = 6;
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
        Destroy(GetComponent<FixedJoint2D>()); 
        pickableObject.Holded = false;
        pickableObject.transform.gameObject.layer = 9;
    }
}
