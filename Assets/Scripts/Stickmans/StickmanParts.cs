using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanParts : MonoBehaviour
{
    bool separated;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TryGetComponent(out PartsLifes PL) && PL.lifes <= 0)
        {
            separated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Chunk") && separated)
        {
            transform.parent = collision.transform.parent;
        }
    }
}
