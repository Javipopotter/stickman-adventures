using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanoidController))]
public class PlayerLifesManager : MonoBehaviour
{
    HumanoidController PlayerController;
    public List<Vector2> positions;
    public List<Quaternion> rotation;
    public Grab[] grabs;
    public List<PartsLifes> partsLifes;
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            positions.Add(transform.GetChild(i).transform.localPosition);
            rotation.Add(transform.GetChild(i).transform.rotation);
        }
        PlayerController = GetComponent<PlataformerMovement>();
    }

    public IEnumerator Respawn()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        PlayerController.speed = PlayerController.OriginalSpeed;
        PlayerController.jumpForce = PlayerController.OriginalJumpForce;
        foreach (PlayerPartsLifes part in partsLifes)
        {
            part.ActiveDeactiveComponents(true);
            part.lifes = part.OrLifes;
            part.enabled = true;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.localPosition = positions[i];
            transform.GetChild(i).transform.rotation = rotation[i];
            transform.GetChild(i).GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        foreach (Grab grab in grabs)
        {
            if (grab.grabbed)
            {
                grab.Drop();
            }
        }

    }
}
