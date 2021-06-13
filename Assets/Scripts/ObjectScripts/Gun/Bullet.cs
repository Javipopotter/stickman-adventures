using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    [HideInInspector] float DmgMultiplier = 1f;
    public bool IsPickedByEnemy;
    float lifetime = 3;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            lifetime = 3;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        lifetime = 3;
        rb = GetComponent<Rigidbody2D>();
        GameManager.Gm.StartCoroutine(GameManager.Gm.DoDamage(collision, rb, DmgMultiplier, IsPickedByEnemy));
        GameManager.Gm.UpdateColliders(GetComponent<Collider2D>(), false, GameManager.Gm.AlliesColliders);
        gameObject.SetActive(false);
    }
}
