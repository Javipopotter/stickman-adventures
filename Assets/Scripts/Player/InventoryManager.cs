using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> Items;
    public GameObject[] ItemsAspect;
    [SerializeField]int ItemPos = 0;
    [SerializeField]int LastItemPos = 0;
    [SerializeField] Grab PlayerGrab;

    void Update()
    {
        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            if (PlayerGrab.grabbed && !PlayerGrab.objectGrab)
                PlayerGrab.Drop();

            if (PlayerGrab.pickable != null)
            {
                if (PlayerGrab.pickable.CanGetChanged)
                {
                    InventoryRoll(true);
                }
            } 
            else
            {
                InventoryRoll(false);
            }
        }
    }

    void InventoryRoll(bool t)
    {
        LastItemPos = ItemPos;
        ItemPos += Mathf.RoundToInt(Input.GetAxisRaw("Mouse ScrollWheel"));
        if (ItemPos <= 2 && ItemPos >= 0)
        {
            ChangeItem(t);
        }
        else if (ItemPos < 0)
        {
            ItemPos = 2;
            ChangeItem(t);
        }
        else
        {
            ItemPos = 0;
            ChangeItem(t);
        }
    }

    void ChangeItem(bool t)
    {
        if (t)
        {
            Items[LastItemPos] = PlayerGrab.pickable.gameObject;
            PlayerGrab.Drop();
            Items[LastItemPos].SetActive(false); 
        }
        else
        {
            Items[LastItemPos] = null;
        }

        if(Items[ItemPos] != null)
        {
            Items[ItemPos].SetActive(true);
            Items[ItemPos].transform.SetPositionAndRotation(transform.position, PlayerGrab.transform.rotation);
            PlayerGrab.GrabWeapon(Items[ItemPos].GetComponent<Collider2D>(), true);
        }
    }
}
