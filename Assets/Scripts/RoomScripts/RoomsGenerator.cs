using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoomsGenerator : MonoBehaviour
{
    public List<GameObject> leftRooms;
    public List<GameObject> rightRooms;
    public List<GameObject> upRooms;
    public List<GameObject> downRooms;
    GameObject lastRoom;
    GameObject SpawnRoom;
    [SerializeField] GameObject Filler;
    public float distance;
    Vector2 roomPos;
    Vector2 dir;
    public Vector2[] directions;
    public List<GameObject> TypeOfRoom = null;
    //public List<Vector2> occupedPositions;
    void Start()
    {
        StartCoroutine(Produce());
    }

    IEnumerator Produce()
    {
        roomPos = transform.position;
        lastRoom = Instantiate(rightRooms[0], roomPos, Quaternion.identity) as GameObject;
        GameManager.Gm.ocuppedPos.Add(roomPos);
        for (int i = 0; i < 100; i++)
        {
            dir = directions[Random.Range(0, directions.Length)];
            re1:
            roomPos += dir * distance;
            Vector2 originalDir = dir;

            for(int c = 0; c < 4; c ++)
            {
                if(GameManager.Gm.ocuppedPos.Contains(roomPos))
                {
                    roomPos -= dir * distance;
                    dir = RotateRoom(dir);
                    if (dir == originalDir)
                    {
                        roomPos = GameManager.Gm.ocuppedPos[Random.Range(0, GameManager.Gm.ocuppedPos.Count)];
                        goto re1;
                    }
                    roomPos += dir * distance;
                }
            }

            if (roomPos.y <= -1000)
            {
                lastRoom.GetComponent<RoomType>().AutoDestroy();
                GameManager.Gm.GeneratedRooms.Remove(lastRoom);
                GameManager.Gm.GeneratedRooms.Add(Instantiate(upRooms[Random.Range(0, upRooms.Count)], roomPos - dir * distance, Quaternion.identity));
                roomPos = GameManager.Gm.ocuppedPos[Random.Range(0, GameManager.Gm.ocuppedPos.Count)];
                goto re1;
            }

            CheckTypeOfRoom();
            CheckRoomChange();
            SpawnRoom = TypeOfRoom[Random.Range(0, TypeOfRoom.Count)];
            lastRoom = Instantiate(SpawnRoom, roomPos, Quaternion.identity) as GameObject;
            GameManager.Gm.GeneratedRooms.Add(lastRoom);
            TypeOfRoom.Clear();
            GameManager.Gm.ocuppedPos.Add(roomPos);
            yield return new WaitForSeconds(0.05f);
        }
        Fill();
    }

    void Fill()
    {
        Vector2 FillPos;
        foreach(Vector2 pos in GameManager.Gm.ocuppedPos)
        {
            dir = Vector2.right;
            FillPos = pos;
            for(int i = 0; i < 4; i++)
            {
                FillPos += dir * distance;
                if(!GameManager.Gm.ocuppedPos.Contains(FillPos))
                {
                    GameManager.Gm.GeneratedRooms.Add(Instantiate(Filler, FillPos, Quaternion.identity));
                }
                FillPos = pos;
                dir = RotateRoom(dir);
            }
        }
    }

    void CheckTypeOfRoom()
    {
        if (dir == Vector2.right)
            TypeOfRoom.AddRange(leftRooms);
        else if (dir == Vector2.up)
            TypeOfRoom.AddRange(downRooms);
        else if (dir == Vector2.down)
            TypeOfRoom.AddRange(upRooms);
        else
            TypeOfRoom.AddRange(rightRooms);

    }

    void CheckRoomChange()
    {
        if(dir == Vector2.up && lastRoom.GetComponent<RoomType>().Direct.y < 0)
        {
            lastRoom.GetComponent<RoomType>().AutoDestroy();
            GameManager.Gm.GeneratedRooms.Remove(lastRoom);
            GameManager.Gm.GeneratedRooms.Add(Instantiate(upRooms[Random.Range(0, upRooms.Count)], roomPos - dir * distance, Quaternion.identity));
        }
    }

    Vector2 RotateRoom(Vector2 v)
    {
        if (v == Vector2.right)
            return Vector2.down;
        else if (v == Vector2.down)
            return Vector2.left;
        else if (v == Vector2.left)
            return Vector2.up;
        else if (v == Vector2.up)
            return Vector2.right;
        else
            return new Vector2(33, 28);
    }
}
