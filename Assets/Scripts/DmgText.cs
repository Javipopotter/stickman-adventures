using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgText : MonoBehaviour
{
    float timer = 1;

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer > 0.33f)
        {
            transform.Translate(0, 0.3f, 0);
        }

        if(timer <= 0)
        {
            timer = 1;
            gameObject.SetActive(false);
        }
    }
}
