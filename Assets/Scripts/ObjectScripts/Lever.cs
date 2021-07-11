using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    public UnityEvent customEvent;
    public bool side;
    float leverCoolDown;
    public float CoolDownTime;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(transform.localRotation.z <= -0.50f && side)
        {
            ExecuteEvent(false);
        }
        else if(transform.localRotation.z >= 0.50f && !side)
        {
            ExecuteEvent(true);
        }

        if(leverCoolDown > 0)
        {
            leverCoolDown -= Time.deltaTime;
        }
        else if(rb.freezeRotation == true)
        {
            rb.freezeRotation = false;
        }
    }

    private void ExecuteEvent(bool s)
    {
        customEvent.Invoke();
        leverCoolDown = CoolDownTime;
        rb.freezeRotation = true;
        side = s;
    }
}
