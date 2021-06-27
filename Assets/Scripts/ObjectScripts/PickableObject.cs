using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

[DisallowMultipleComponent]
public class PickableObject : MonoBehaviour
{
    public bool blockRot, doesntCollide;
    public List<Behaviour> Components;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool Holded;
    public Material InitMaterial;
    public int OutLineThickness;
    public bool CanPunch, BlockArm;
    public float DmgMultiplier;
    public bool PickedByAI;
    public float range;
    public Weapon ThisWeapon;
    public bool CanGetChanged = true;
    public float WeaponCoolDown = 0;
    public bool activateCoolDown;
    [SerializeField] float minVel;
    public bool PickedByAllie;
    public GameObject Holder;
    public enum Weapon
    {
        Spear, Sword, HookShot, Gun
    }
    public virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        InitMaterial = sr.material;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        if(rb.velocity.magnitude < minVel * 2 && gameObject.layer != 0 && !Holded)
        {
            SetLayers(0);
        }
    }

    public void ChangeProperties(bool take, bool PickedByAI, int layerToIgnore, bool PickedByAllie, GameObject Holder)
    {
        this.Holder = Holder;
        this.PickedByAI = PickedByAI;
        this.PickedByAllie = PickedByAllie;
        Holded = take;
        if (take)
        {
            if (!PickedByAI)
            {
                transform.parent = null; 
            }

            if (blockRot)
            {
                rb.freezeRotation = true;
            }

            if (doesntCollide)
            {
                SetLayers(layerToIgnore);
            }

            ActiveDeactiveComponents(!PickedByAI);
        }
        else
        {
            if (blockRot)
            {
                rb.freezeRotation = false;
            }

            if (doesntCollide)
            {
                SetLayers(layerToIgnore);
            }

            ActiveDeactiveComponents(false);
        }
    }

    public void SetLayers(int i)
    {
        gameObject.layer = i;
        foreach (Transform trans in GetComponentsInChildren<Transform>())
        {
            trans.gameObject.layer = i;
        }
    }

    public void ActiveDeactiveComponents(bool Switch)
    {
        foreach (Behaviour component in Components)
        {
            component.enabled = Switch;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Chunk") && !Holded && !PickedByAI)
        {
            transform.parent = collision.transform.parent;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Holded)
        {
            GameManager.Gm.StartCoroutine(GameManager.Gm.DoDamage(collision, rb, DmgMultiplier, PickedByAI, minVel, Holder)); 
        }
        else
        {
            GameManager.Gm.StartCoroutine(GameManager.Gm.DoDamage(collision, rb, DmgMultiplier * 8, PickedByAI, minVel * 2, Holder));
        }
    }
}
