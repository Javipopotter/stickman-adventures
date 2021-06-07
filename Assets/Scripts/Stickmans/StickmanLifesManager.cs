using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanLifesManager : MonoBehaviour
{
    public List<PartsLifes> partsLifes;

    public void Death()
    {
        foreach(AIPartLifes part in partsLifes)
        {
            if (part.TryGetComponent(out Balance bal))
                bal.enabled = false;
            if (part.IsHand)
                part.AssignedHand.Drop();
        }
        GetComponent<AI>().enabled = false;
    }
}
