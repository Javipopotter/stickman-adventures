using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public string TagName;
    public UnityEvent customEvent;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(TagName))
        {
            customEvent.Invoke();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
