using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField]float coinValue = 1;
    readonly int MaxCoinValue = 10;
    private void Awake()
    {
        coinValue = Random.Range(1, MaxCoinValue);
        coinValue = Mathf.RoundToInt(coinValue);
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.yellow, Color.red, (coinValue - 1) / MaxCoinValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.transform.parent.TryGetComponent(out HumanoidController humanoidController))
        {
            if (humanoidController.enabled == true)
            {
                GameManager.Gm.MoneyAmount += coinValue;
                DmgText txt = ObjectPooler.pool.GetPooledObject(1).GetComponent<DmgText>();
                txt.TextSetting(transform.position, coinValue, Color.green);
                gameObject.SetActive(false); 
            }
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
