using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    public static GameManager Gm;
    public List<Vector2> ocuppedPos;
    public List<GameObject> GeneratedRooms;
    public GameObject PlayerTorso;

    void Awake()
    {
        Gm = this;
    }

    public Vector2 GetMouseVector(Vector3 pos)
    {
        Vector2 dir;
        dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos;
        dir.Normalize();
        return dir;
    }
}