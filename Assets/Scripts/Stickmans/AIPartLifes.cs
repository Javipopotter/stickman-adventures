using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPartLifes : PartsLifes
{
    AI ai;
    StickmanLifesManager stickmanLifesManager;
    public bool IsHand;
    public AIGrab AssignedHand;
    public bool IsLeg;
    float QuarterJ;
    void Start()
    {
        if (transform.parent.TryGetComponent(out AI aI))
        {
            ai = aI;
            QuarterJ = ai.jumpForce / 4;
        }
        sr = GetComponent<SpriteRenderer>();
        stickmanLifesManager = GetComponentInParent<StickmanLifesManager>();
        stickmanLifesManager.partsLifes.Add(this);
        OrColor = sr.color;
        OrLifes = lifes;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (lifes <= 0)
        {
            if (vitalPoint)
            {
                ActiveDeactiveComponents(false);
                stickmanLifesManager.Death();
            }
            else
            {
                ActiveDeactiveComponents(false);
            }
            this.enabled = false;
        }
    }

    public override void ActiveDeactiveComponents(bool t)
    {
        base.ActiveDeactiveComponents(t);
        if (IsLeg)
            ai.jumpForce -= QuarterJ;

        if (IsHand)
            AssignedHand.Drop();
    }
}
