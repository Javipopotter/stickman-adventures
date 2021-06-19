using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    public UnityEvent customEvent;
    public bool side;

    private void Update()
    {
        if(transform.localRotation.z <= -0.50f && side)
        {
            customEvent.Invoke();
            side = false;
        }
        else if(transform.localRotation.z >= 0.50f && !side)
        {
            customEvent.Invoke();
            side = true;
        }
    }
}
