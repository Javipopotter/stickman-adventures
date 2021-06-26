using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartsLifes : PartsLifes
{
    PlayerLifesManager playerLifesManager;
    void Awake()
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
            ActiveDeactiveComponents(false);
            if (vitalPoint)
            {
                StartCoroutine(playerLifesManager.Respawn());
            }
            this.enabled = false;
        }
    }
}
