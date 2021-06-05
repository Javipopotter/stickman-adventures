using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjects : MonoBehaviour
{
    GameObject player;
    [SerializeField]float Distance = 80;
    [SerializeField]bool t = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < Distance && t)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            t = false;
        }

        if (Vector2.Distance(transform.position, player.transform.position) > Distance && t == false)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            t = true;
        }
    }
}
