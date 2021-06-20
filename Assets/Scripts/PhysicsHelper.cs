using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHelper : MonoBehaviour
{
    void Update()
    {
        if (TryGetComponent(out HingeJoint2D HJ))
        {
            if (Vector2.Distance(transform.position, HJ.connectedBody.transform.position) > 3)
            {
                transform.position = HJ.connectedBody.transform.position;
            }
        }
    }
}
