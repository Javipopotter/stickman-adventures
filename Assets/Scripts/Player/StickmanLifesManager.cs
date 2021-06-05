using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanLifesManager : MonoBehaviour
{
    public List<Vector2> positions;
    public List<Quaternion> rotation;
    public List<Grab> grabs;
    public List<PartsLifes> partsLifes;
    bool IsPlayer;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            positions.Add(transform.GetChild(i).transform.localPosition);
            rotation.Add(transform.GetChild(i).transform.rotation);
        }
    }

    public void respawn()
    {
        if (IsPlayer)
        {
            foreach (PartsLifes part in partsLifes)
            {
                part.ActiveDeactiveComponents(true);
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).transform.localPosition = positions[i];
                transform.GetChild(i).transform.rotation = rotation[i];
                transform.GetChild(i).GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            foreach (FixedJoint2D fixedJoint2D in GetComponentsInChildren<FixedJoint2D>())
            {
                Destroy(fixedJoint2D);
            }

            foreach (Grab grab in grabs)
            {
                grab.ActiveDeactivePunches(true, false);
                if (grab.pickable != null)
                {
                    grab.pickable.ChangeProperties(grab.gameObject, false);
                }
            }
        }
    }
}
