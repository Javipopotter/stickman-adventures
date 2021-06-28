using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsGenerator : MonoBehaviour
{
    public List<GameObject> IIOO, IOIO, IOOI, OIIO, OIOI, OOII;
    public List<GameObject> leftRooms;
    public List<GameObject> rightRooms;
    public List<GameObject> upRooms;
    public List<GameObject> downRooms;
    [SerializeField] GameObject InitialRoom;
    GameObject lastRoom;
    RoomType.RoomStructure lastRoomOrigin;
    GameObject SpawnRoom;
    [SerializeField] GameObject Filler;
    public float distance;
    Vector2 roomPos;
    Vector2 dir;
    public Vector2[] directions;
    public List<GameObject> TypeOfRoom = null;
    [HideInInspector]public GameObject WorldContainer;
    PlayerLifesManager playerLifesManager;
    float NumberOfRooms = 100;
    //public List<Vector2> occupedPositions;
    void Start()
    {
        playerLifesManager = GameManager.Gm.PlayerTorso.GetComponentInParent<PlayerLifesManager>();
        StartCoroutine(Produce());
    }

    public IEnumerator Produce()
    {
        WorldContainer = new GameObject();
        WorldContainer = Instantiate(WorldContainer, transform.position, Quaternion.identity);
        roomPos = transform.position;
        lastRoom = Instantiate(InitialRoom, roomPos, Quaternion.identity, WorldContainer.transform) as GameObject;
        GameManager.Gm.ocuppedPos.Add(roomPos);
        GameManager.Gm.GeneratedRooms.Add(lastRoom);
        for (int i = 0; i < NumberOfRooms; i++)
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
                GameManager.Gm.GeneratedRooms.Add(Instantiate(upRooms[Random.Range(0, upRooms.Count)], roomPos - dir * distance, Quaternion.identity, WorldContainer.transform));
                roomPos = GameManager.Gm.ocuppedPos[Random.Range(0, GameManager.Gm.ocuppedPos.Count)];
                goto re1;
            }

            CheckTypeOfRoom();
            CheckRoomChange();
            SpawnRoom = TypeOfRoom[Random.Range(0, TypeOfRoom.Count)];
            lastRoom = Instantiate(SpawnRoom, roomPos, Quaternion.identity, WorldContainer.transform) as GameObject;
            GameManager.Gm.GeneratedRooms.Add(lastRoom);
            TypeOfRoom.Clear();
            GameManager.Gm.ocuppedPos.Add(roomPos);
            GameManager.Gm.WorldLoadingSlider.value = GameManager.Gm.GeneratedRooms.Count / NumberOfRooms;
            yield return new WaitForSeconds(0.05f);
        }
        Fill();
        Fill();
        GameManager.Gm.LoadingScreen.SetActive(false);
        playerLifesManager.Respawn();
    }

    void Fill()
    {
        Vector2 FillPos;
        List<Vector2> AuxiliaryList = new List<Vector2>();
        foreach(Vector2 pos in GameManager.Gm.ocuppedPos)
        {
            dir = Vector2.right;
            FillPos = pos;
            for(int i = 0; i < 4; i++)
            {
                FillPos += dir * distance;
                if(!GameManager.Gm.ocuppedPos.Contains(FillPos))
                {
                    GameManager.Gm.GeneratedRooms.Add(Instantiate(Filler, FillPos, Quaternion.identity, WorldContainer.transform));
                    AuxiliaryList.Add(FillPos);
                }
                FillPos = pos;
                dir = RotateRoom(dir);
            }
        }
        GameManager.Gm.ocuppedPos.AddRange(AuxiliaryList);
    }

    void CheckTypeOfRoom()
    {
        if (dir == Vector2.right)
        {
            TypeOfRoom.AddRange(leftRooms);
            lastRoomOrigin = new RoomType.RoomStructure(false, false, false, true);
        }
        else if (dir == Vector2.up)
        {
            TypeOfRoom.AddRange(downRooms);
            lastRoomOrigin = new RoomType.RoomStructure(false, false, true, false);
        }
        else if (dir == Vector2.down)
        {
            TypeOfRoom.AddRange(upRooms);
            lastRoomOrigin = new RoomType.RoomStructure(true, false, false, false);
        }
        else
        {
            TypeOfRoom.AddRange(rightRooms);
            lastRoomOrigin = new RoomType.RoomStructure(false, true, false, false);
        }

    }

    void CheckRoomChange()
    {

        if (lastRoomOrigin.Up)
        {
            TypeOfRoomChange(null, IOIO, IIOO, IOOI); 
        }
        else if(lastRoomOrigin.Down)
        {
            TypeOfRoomChange(IOIO, null, OIIO, OOII);
        }
        else if(lastRoomOrigin.Right)
        {
            TypeOfRoomChange(IIOO, OIIO, null, OIOI);
        }
        else if(lastRoomOrigin.Left)
        {
            TypeOfRoomChange(IOOI, OOII, OIOI, null);
        }
    }

    void TypeOfRoomChange(List<GameObject> upFixedType, List<GameObject> downFixedType, List<GameObject> rightFixedType, List<GameObject> leftFixedType)
    {
        RoomType roomType = lastRoom.GetComponent<RoomType>();
        
        RoomChange(Vector2.up, roomType.TypeOfRoom.Up, upFixedType);

        RoomChange(Vector2.down, roomType.TypeOfRoom.Down, downFixedType);

        RoomChange(Vector2.right, roomType.TypeOfRoom.Right, rightFixedType);

        RoomChange(Vector2.left, roomType.TypeOfRoom.Left, leftFixedType);
    }

    void RoomChange(Vector2 vector, bool typeOfRoom, List<GameObject> rooms)
    {
        if (dir == vector && !typeOfRoom)
        {
            lastRoom.GetComponent<RoomType>().AutoDestroy();
            GameManager.Gm.GeneratedRooms.Remove(lastRoom);
            GameManager.Gm.GeneratedRooms.Add(lastRoom = Instantiate(rooms[Random.Range(0, rooms.Count)], roomPos - dir * distance, Quaternion.identity, WorldContainer.transform));
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
    //RoomType.RoomStructure SumRooms(RoomType.RoomStructure room1, RoomType.RoomStructure room2)
    //{
    //    RoomType.RoomStructure returnRoom = new RoomType.RoomStructure();
    //    if (room1.Up || room2.Up)
    //        returnRoom.Up = true;
    //    if (room1.Right || room2.Right)
    //        returnRoom.Right = true;
    //    if (room1.Down || room2.Down)
    //        returnRoom.Down = true;
    //    if (room1.Left || room2.Left)
    //        returnRoom.Left = true;

    //    return returnRoom;
    //}
