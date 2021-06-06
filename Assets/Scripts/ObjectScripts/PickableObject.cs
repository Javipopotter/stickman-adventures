using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public bool blockRot, doesntCollide, transformsPos, pickaxe;
    public List<Behaviour> Components;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    bool takeOnMe = false;
    public bool Holded;
    GameObject Hand;
    public Material InitMaterial;
    public int OutLineThickness;
    public bool CanPunch, BlockArm;
    public float DmgMultiplier;
    public bool IsPickedByEnemy;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        InitMaterial = sr.material;
        rb = GetComponent<Rigidbody2D>();
    }

    public void ChangeProperties(GameObject hand, bool take, bool PickedByEnemy)
    {
        IsPickedByEnemy = PickedByEnemy;
        Hand = hand;
        if (take)
        {
            takeOnMe = true;
            if (blockRot)
            {
                rb.freezeRotation = true;
            }

            if (doesntCollide)
            {
                gameObject.layer = 9;
                SetLayers(9);
            }

            if (transformsPos)
            {
                transform.position = hand.transform.position;
                transform.rotation = hand.transform.rotation;
            }

            ActiveDeactiveComponents(true);
        }
        else
        {
            if (blockRot)
            {
                rb.freezeRotation = false;
            }

            if (doesntCollide)
            {
                gameObject.layer = 0;
                SetLayers(0);
            }

            ActiveDeactiveComponents(false);
        }
    }

    void SetLayers(int i)
    {
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
            Holded = Switch;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Chunk"))
        {
            transform.parent = collision.transform.parent;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(pickaxe && collision.transform.CompareTag("floor") && takeOnMe)
        {
            takeOnMe = false;
            transform.eulerAngles = new Vector3(0,0,1);
        }
        if (pickaxe && collision.transform.CompareTag("floor") && transformsPos && GetComponent<MoveArms>().enabled)
        {
            transform.position = Hand.transform.position;
        }
        if(Holded == false && collision.transform.CompareTag("floor"))
        {
            IsPickedByEnemy = false;
        }
    }
}
