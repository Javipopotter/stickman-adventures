using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsLifes : MonoBehaviour
{
    public float lifes;
    public bool vitalPoint;
    [HideInInspector] public float OrLifes;
    public SpriteRenderer sr;
    public Color OrColor;

    public virtual void Update()
    {
        sr.color = Color.Lerp(Color.red, OrColor, Percentaje(lifes, OrLifes));
    }

    public virtual void ActiveDeactiveComponents(bool t)
    {
        if(TryGetComponent(out HingeJoint2D HJ))
        {
            HJ.enabled = t;
            GetComponent<PhysicsHelper>().enabled = t;
        }
        if (TryGetComponent(out Balance bal))
        {
            bal.enabled = t;
        }
    }

    float Percentaje(float i, float o)
    {
        return i/o;
    }
}
