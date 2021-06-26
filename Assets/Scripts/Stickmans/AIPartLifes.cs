using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPartLifes : PartsLifes
{
    AI ai;
    StickmanLifesManager stickmanLifesManager;
    public bool IsHand;
    public AIGrab AssignedHand;
    float lifeChecker;
    bool separated;
    void Awake()
    {
        lifeChecker = lifes;
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

        if (stickmanLifesManager.enabled)
        {
            if (lifeChecker != lifes && lifes > 0)
            {
                lifeChecker = lifes;
                stickmanLifesManager.CheckLife();
            }

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
        else if(lifes <= 0)
        {
            ActiveDeactiveComponents(false);
            enabled = false;
        }

        //if(Damager != null)
        //{
        //    ai.GetEnemy(Damager);
        //    Damager = null;
        //}
    }

    public override void ActiveDeactiveComponents(bool t)
    {
        transform.parent = transform.parent.transform.parent;
        separated = true;
        stickmanLifesManager.partsLifes.Remove(this);
        base.ActiveDeactiveComponents(t);
        AssignedHandDrop();
    }

    public void AssignedHandDrop()
    {
        if (IsHand && AssignedHand != null)
        {
            AssignedHand.Drop();
            Destroy(AssignedHand);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chunk"))
        {
            if (!separated)
            {
                transform.parent.transform.parent = collision.transform.parent;
            }
            else
            {
                transform.parent = collision.transform.parent;
            } 
        }
    }
}
