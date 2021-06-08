using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjects : MonoBehaviour
{
    GameObject player;
    [SerializeField] float Distance = 80;

    private void Awake()
    {
        player = GameManager.Gm.PlayerTorso;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        } 
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < Distance)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        if (Vector2.Distance(transform.position, player.transform.position) > Distance)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
