using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartsLifes : PartsLifes
{
    PlayerLifesManager playerLifesManager;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerLifesManager = GetComponentInParent<PlayerLifesManager>();
        playerLifesManager.partsLifes.Add(this);
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
                playerLifesManager.respawn();
            }
            else
            {
                ActiveDeactiveComponents(false);
            }
            this.enabled = false;
        }
    }
}
