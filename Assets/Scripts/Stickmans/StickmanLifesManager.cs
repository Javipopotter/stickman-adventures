using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanLifesManager : MonoBehaviour
{
    public List<PartsLifes> partsLifes;
    float MaxLife;
    public float TotalLife;

    private void Start()
    {
        foreach(AIPartLifes part in partsLifes)
        {
            TotalLife += part.lifes;
        }
        MaxLife = TotalLife;
    }

    public void CheckLife()
    {
        TotalLife = 0;
        foreach (AIPartLifes part in partsLifes)
        {
            TotalLife += part.lifes;
        }
        if (TotalLife / MaxLife <= 0.5f)
        {
            Death();
        }
    }

    public void Death()
    {
        foreach(AIPartLifes part in partsLifes)
        {
            if (part.TryGetComponent(out Balance bal))
                bal.enabled = false;
            if (part.IsHand)
            {
                part.AssignedHand.Drop();
            }
            part.enabled = false;
        }
        GetComponent<AI>().enabled = false;
    }
}
