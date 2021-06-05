using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public Vector2 Direct;

    public void AutoDestroy()
    {
        Destroy(this.gameObject);
    }
}
