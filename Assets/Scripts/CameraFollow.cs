using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject camaraaddjust;
    float CamaraRadius;
    // Start is called before the first frame update
    void Start()
    {
        CamaraRadius = camaraaddjust.transform.localPosition.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        if (player.transform.position.y >= transform.position.y + CamaraRadius)
        {
            YUpdate(0, CamaraRadius);
        }
        if (player.transform.position.y <= transform.position.y - CamaraRadius)
        {
            YUpdate(0, -CamaraRadius);
        }
    }
    public void YUpdate(float x, float y)
    {
        transform.position = new Vector3(player.transform.position.x - x, player.transform.position.y - y, transform.position.z); // update y
    }
    public void restartCameraPosition()
    {
        YUpdate(0, 0);
    }
}
        /*if (Mathf.Abs(transform.position.y - player.transform.position.y) >= 25)
        {
            if(transform.position.y < player.transform.position.y)
                transform.Translate(new Vector2(0, 50));
            else
                transform.Translate(new Vector2(0, -50));
        }
        if (Mathf.Abs(transform.position.x - player.transform.position.x) >= 44.461)
        {
            if (transform.position.x < player.transform.position.x)
                transform.Translate(new Vector2(88.922f, 0));
            else
                transform.Translate(new Vector2(-88.922f, 0));
        }*/
        /*if(player.transform.position.x >= transform.position.x + CamaraRadius)
        {
            YUpdate(CamaraRadius, 0);
        }
        if (player.transform.position.x <= transform.position.x - CamaraRadius)
        {
            YUpdate(-CamaraRadius, 0);
        }*/
