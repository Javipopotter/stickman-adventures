using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgText : MonoBehaviour
{
    float timer = 1;
    public TextMeshPro txt;
    float OriginalSize;

    private void Awake()
    {
        txt = GetComponent<TextMeshPro>();
        OriginalSize = txt.fontSize;
    }

    public void TextSetting(Vector2 pos, float value ,Color color)
    {
        transform.position = pos;
        txt.text = value + "";
        txt.color = color;
        //txt.fontSize = OriginalSize * sizeMultiplier;
    }

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
