using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    int coinValue = 1;
    private void Awake()
    {
        coinValue = Random.Range(1, 11);
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.yellow, Color.red, coinValue);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Gm.MoneyAmount += coinValue;
            gameObject.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
