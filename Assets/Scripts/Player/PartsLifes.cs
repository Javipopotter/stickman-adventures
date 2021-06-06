using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsLifes : MonoBehaviour
{
    StickmanLifesManager stickmanLifesManager;
    AI ai;
    public float lifes;
    [HideInInspector] public float OrLifes;
    public bool vitalPoint;
    [SerializeField] bool IsPlayer;
    [SerializeField] bool IsLeg;
    SpriteRenderer sr;
    Color OrColor;
    [SerializeField]float QuarterJ;
    void Start()
    {
        if (transform.parent.TryGetComponent(out AI aI))
        {
            ai = aI;
            QuarterJ = ai.jumpForce / 4;
        }
        stickmanLifesManager = transform.parent.GetComponent<StickmanLifesManager>();
        stickmanLifesManager.partsLifes.Add(this);
        sr = GetComponent<SpriteRenderer>();
        OrColor = sr.color;
        OrLifes = lifes;
    }

    private void Update()
    {
        if(lifes <= 0)
        {
            if (vitalPoint)
            {
                if (IsPlayer)
                {
                    stickmanLifesManager.respawn();
                }
                else
                {
                    ActiveDeactiveComponents(false);
                    stickmanLifesManager.Death();
                }
            }
            else
            {
                ActiveDeactiveComponents(false);
            }
            this.enabled = false;
        }
        sr.color = Color.Lerp(Color.red, OrColor, Percentaje(lifes, OrLifes));
    }

    public void ActiveDeactiveComponents(bool t)
    {
        if(TryGetComponent(out HingeJoint2D HJ))
        {
            HJ.enabled = t;
        }
        if (TryGetComponent(out Balance bal))
        {
            bal.enabled = t;
        }
        if(IsLeg)
        {
            ai.jumpForce -= QuarterJ;
        }
    }

    float Percentaje(float i, float o)
    {
        return i/o;
    }
}
