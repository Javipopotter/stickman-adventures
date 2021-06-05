using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    private void Start()
    {
        Color RandomCol = Random.ColorHSV();
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = RandomCol;
        }
    }
}
