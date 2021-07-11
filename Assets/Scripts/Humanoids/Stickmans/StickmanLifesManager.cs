using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanLifesManager : MonoBehaviour
{
    AI aI;
    public List<PartsLifes> partsLifes;
    public float MaxLife;
    public float TotalLife;
    public float requiredDmgToDie = 0.5f;

    private void Start()
    {
        aI = GetComponent<AI>();
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
        if (TotalLife / MaxLife <= requiredDmgToDie)
        {
            Death();
        }
    }

    public void Death()
    {
        TotalLife = 0;
        foreach(AIPartLifes part in partsLifes)
        {
            if (part.TryGetComponent(out Balance bal))
                bal.enabled = false;
            if (part.IsHand)
            {
                part.AssignedHandDrop();
            }
        }
        for (int i = 0; i < Random.Range(5, 15); i++)
        {
            GameObject g = ObjectPooler.pool.GetPooledObject(2);
            g.transform.position = aI.torso.transform.position;
            g.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), Random.Range(15, 25)), ForceMode2D.Impulse); 
        }
        enabled = false;
        aI.enabled = false;
    }
}
