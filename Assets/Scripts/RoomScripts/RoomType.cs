using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomType : MonoBehaviour
{
    public RoomStructure TypeOfRoom;
    public List<Vector3> tilesPos;
    public Tilemap tilemap;
    [SerializeField] bool Filler;

    void Awake()
    {
        foreach(SpawnTile tile in tilemap.GetComponentsInChildren<SpawnTile>())
        {
            tilesPos.Add(tile.transform.position);
        }
        if (!Filler)
        {
            GameManager.Gm.ocuppedPos.Add(transform.position);
        }
        GameManager.Gm.GeneratedRooms.Add(gameObject);
    }

    [System.Serializable]
    public struct RoomStructure
    {
        public bool Up;
        public bool Right;
        public bool Down;
        public bool Left;


        public RoomStructure(bool up, bool right, bool down, bool left)
        {
            Right = right;
            Left = left;
            Up = up;
            Down = down;
        }
    }

    public void AutoDestroy()
    {
        foreach (Vector3 pos in tilesPos)
        {
            GameManager.Gm.tilemap.SetTile(GameManager.Gm.tilemap.layoutGrid.WorldToCell(pos), null);
        }
        GameManager.Gm.GeneratedRooms.Remove(gameObject);
        GameManager.Gm.ocuppedPos.Remove(transform.position);
        Destroy(this.gameObject);
    }
}
