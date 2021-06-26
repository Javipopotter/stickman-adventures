using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsLifes : MonoBehaviour
{
    public float lifes;
    public bool vitalPoint;
    public bool IsLeg;
    [SerializeField] PartsLifes ConnectedPart;
    HumanoidController humanoid;
    [HideInInspector] public float OrLifes;
    public GameObject Damager;
    public SpriteRenderer sr;
    public Color OrColor;

    private void Start()
    {
        humanoid = GetComponentInParent<HumanoidController>();
    }

    public virtual void Update()
    {
        sr.color = Color.Lerp(Color.red, OrColor, Percentaje(lifes, OrLifes));
    }

    public virtual void ActiveDeactiveComponents(bool t)
    {
        if(TryGetComponent(out Joint2D HJ))
        {
            HJ.enabled = t;
            GetComponent<PhysicsHelper>().enabled = t;
        }
        if (TryGetComponent(out Balance bal))
        {
            bal.enabled = t;
        }

        if (ConnectedPart != null && !t)
        {
            ConnectedPart.lifes = 0;
        }

        if (IsLeg && t == false)
        {
            humanoid.jumpForce -= humanoid.jumpForce / humanoid.LegsCount;
            humanoid.speed -= humanoid.speed / humanoid.LegsCount;
        }
    }

    float Percentaje(float i, float o)
    {
        return i/o;
    }
}
