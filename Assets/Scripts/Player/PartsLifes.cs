using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsLifes : MonoBehaviour
{
    StickmanLifesManager stickmanLifesManager;
    public int lifes;
    [HideInInspector] public int OrLifes;
    public bool vitalPoint;
    SpriteRenderer sr;
    Color OrColor;
    void Start()
    {
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
                stickmanLifesManager.respawn();
            }
            else
            {
                ActiveDeactiveComponents(false);
            }
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
    }

    float Percentaje(float i, float o)
    {
        return i/o;
    }
}
