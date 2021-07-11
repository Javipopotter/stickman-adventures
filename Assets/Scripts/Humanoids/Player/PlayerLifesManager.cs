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
    InventoryManager inventory;
    void Awake()
    {
        inventory = GetComponent<InventoryManager>();
        for (int i = 0; i < transform.childCount; i++)
        {
            positions.Add(transform.GetChild(i).transform.localPosition);
            rotation.Add(transform.GetChild(i).transform.rotation);
        }
        PlayerController = GetComponent<PlataformerMovement>();
    }

    public void Respawn()
    {
        GameManager.Gm.PlayerTorso.transform.parent.gameObject.SetActive(true);
        SetPlayerMovement(true);
        GameManager.Gm.PlayerEnemy = null;
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
    }

    public void SetPlayerMovement(bool active)
    {
        PlayerController.enabled = active;
        inventory.enabled = active;
        foreach (MoveArms moveArms in GetComponentsInChildren<MoveArms>())
        {
            moveArms.enabled = active;
        }
    }

    public void Death()
    {
        inventory.Items.Clear();
        for(int i = 0; i < 3; i++)
        {
            inventory.Items.Add(null);
        }
        foreach (Grab grab in grabs)
        {
            if (grab.grabbed)
            {
                grab.Drop();
            }
        }
        GameManager.Gm.DeathScreen.SetActive(true);
        for (int i = 0; i < Random.Range(5, 15); i++)
        {
            GameObject g = ObjectPooler.pool.GetPooledObject(2);
            g.transform.position = PlayerController.torso.transform.position;
            g.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), Random.Range(15, 25)), ForceMode2D.Impulse);
        }
        GameManager.Gm.MoneyAmount = 0;
        SetPlayerMovement(false);
    }
}
