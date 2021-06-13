using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

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
    public bool IsPickedByEnemy;
    public float range;
    public Weapon ThisWeapon;
    public bool CanGetChanged = true;
    [SerializeField] List<Collider2D> WeaponColliders;
    public float WeaponCoolDown = 0;
    public bool activateCoolDown;
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

    public void ChangeProperties(bool take, bool PickedByEnemy, List<Collider2D> list)
    {
        IsPickedByEnemy = PickedByEnemy;
        Holded = take;
        GameManager.Gm.UpdateColliders(WeaponColliders, take, list);
        if (take)
        {
            if (blockRot)
            {
                rb.freezeRotation = true;
            }

            if (doesntCollide)
            {
                SetLayers(9);
            }

            ActiveDeactiveComponents(!PickedByEnemy);
        }
        else
        {
            if (blockRot)
            {
                rb.freezeRotation = false;
            }

            if (doesntCollide)
            {
                SetLayers(0);
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
        if(collision.transform.CompareTag("Chunk"))
        {
            transform.parent = collision.transform.parent;
        }
    }
}
