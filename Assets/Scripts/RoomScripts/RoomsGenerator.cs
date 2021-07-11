using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomsGenerator : MonoBehaviour
{
    public List<GameObject> IIOO, IOIO, IOOI, OIIO, OIOI, OOII, IIII;
    public List<GameObject> leftRooms, rightRooms, upRooms, downRooms;
    [SerializeField] GameObject InitialRoom, lastRoom;
    RoomType.RoomStructure lastRoomOrigin;
    Vector2 dir;
    GameObject SpawnRoom;
    public float distance;
    Vector2 roomPos;
    public Vector2[] directions;
    public List<GameObject> TypeOfRoom = null;
    [HideInInspector]public GameObject WorldContainer;
    PlayerLifesManager playerLifesManager;
    [SerializeField] float NumberOfRooms = 100;
    [SerializeField] GameObject Downlim, Uplim;
    public float Uplimit, Downlimit;
    public GameObject HellRoomsGenerator;
    void Start()
    {
        playerLifesManager = GameManager.Gm.PlayerTorso.GetComponentInParent<PlayerLifesManager>();
        Uplimit = transform.position.y + 100;
        Downlimit = transform.position.y - NumberOfRooms * 100;
        StartCoroutine(Produce());
    }

    public IEnumerator Produce()
    {
        Debug.Break();
        GameManager.Gm.SightDistance = 0;
        GameManager.Gm.tilemap.GetComponent<TilemapCollider2D>().enabled = false;
        WorldContainer = Instantiate(new GameObject(), transform.position, Quaternion.identity, transform);
        roomPos = transform.position;
        lastRoom = Instantiate(InitialRoom, roomPos, Quaternion.identity, WorldContainer.transform);
        Downlim = Uplim = lastRoom;
        for (int i = 0; i < NumberOfRooms; i++)
        {
        re1:
            dir = directions[Random.Range(0, directions.Length)];
            roomPos += dir * distance;
            Vector2 originalDir = dir;

            for (int c = 0; c < 4; c++)
            {
                if (GameManager.Gm.ocuppedPos.Contains(roomPos) || roomPos.y <= Downlimit || roomPos.y >= Uplimit)
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
                else break;
            }

            CheckRoomChange();
            CheckTypeOfRoom();
            SpawnRoom = TypeOfRoom[Random.Range(0, TypeOfRoom.Count)];
            lastRoom = Instantiate(SpawnRoom, roomPos, Quaternion.identity, WorldContainer.transform);
            TypeOfRoom.Clear();
            GameManager.Gm.WorldLoadingSlider.value = GameManager.Gm.GeneratedRooms.Count / NumberOfRooms;
            yield return new WaitForFixedUpdate();
        }
        CheckLims();
        if (HellRoomsGenerator != null)
        {
            RoomsGenerator hellRooms;
            hellRooms = Instantiate(HellRoomsGenerator, new Vector2(Downlim.transform.position.x, Downlim.transform.position.y - 100), Quaternion.identity, WorldContainer.transform).GetComponent<RoomsGenerator>();
            hellRooms.Uplimit = Downlim.transform.position.y;
        }
        else
        {
            Fill();
            GameManager.Gm.tilemap.GetComponent<TilemapCollider2D>().enabled = true;
            GameManager.Gm.SightDistance = 150;
            GameManager.Gm.LoadingScreen.SetActive(false);
            playerLifesManager.Respawn();
        }
    }

    void CheckLims()
    {
        foreach(GameObject go in GameManager.Gm.GeneratedRooms)
        {
            if(go.transform.position.y > Uplim.transform.position.y)
            {
                Uplim = go;
            }
            else if(go.transform.position.y < Downlim.transform.position.y)
            {
                Downlim = go;
            }
        }

        dir = Vector2.up;
        RoomChange(Vector2.up, Uplim.GetComponent<RoomType>().TypeOfRoom.Up, IIII, Uplim);
        dir = Vector2.down;
        RoomChange(Vector2.down, Downlim.GetComponent<RoomType>().TypeOfRoom.Down, IIII, Downlim);
    }

    void Fill()
    {
        Vector2 FillPos;
        List<Vector2> AuxiliaryList = new List<Vector2>();
        foreach(Vector2 pos in GameManager.Gm.ocuppedPos)
        {
            dir = Vector2.right;
            FillPos = pos;
            for(int i = 0; i < 8; i++)
            {
                FillPos += dir * distance;
                if(!GameManager.Gm.ocuppedPos.Contains(FillPos))
                {
                    CheckTypeOfFilling(FillPos);
                    AuxiliaryList.Add(FillPos);
                }
                FillPos = pos;
                dir = RotateRoomV2(dir);
            }
        }
        GameManager.Gm.ocuppedPos.AddRange(AuxiliaryList);
    }

    private void CheckTypeOfFilling(Vector2 FillPos)
    {
        if (FillPos.y > Uplimit)
        {
            Instantiate(GameManager.Gm.DungeonFiller, FillPos, Quaternion.identity, WorldContainer.transform); 
        }
        else if(FillPos.y < Uplimit - 100)
        {
            Instantiate(GameManager.Gm.HellFiller, FillPos, Quaternion.identity, WorldContainer.transform);
        }
        else
        {
            Instantiate(GameManager.Gm.MidFiller, FillPos, Quaternion.identity, WorldContainer.transform);
        }
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
        
        RoomChange(Vector2.up, roomType.TypeOfRoom.Up, upFixedType, lastRoom);

        RoomChange(Vector2.down, roomType.TypeOfRoom.Down, downFixedType, lastRoom);

        RoomChange(Vector2.right, roomType.TypeOfRoom.Right, rightFixedType, lastRoom);

        RoomChange(Vector2.left, roomType.TypeOfRoom.Left, leftFixedType, lastRoom);
    }

    void RoomChange(Vector2 vector, bool typeOfRoom, List<GameObject> rooms, GameObject room)
    {
        if (dir == vector && !typeOfRoom)
        {
            room.GetComponent<RoomType>().AutoDestroy();      
            lastRoom = Instantiate(rooms[Random.Range(0, rooms.Count)], room.transform.position, Quaternion.identity, WorldContainer.transform);
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
        else 
            return Vector2.right;
    }

    Vector2 RotateRoomV2(Vector2 v)
    {
        if (v == Vector2.right)
            return Vector2.down;
        else if (v == Vector2.down)
            return Vector2.left;
        else if (v == Vector2.left)
            return Vector2.up;
        else if (v == Vector2.up)
            return new Vector2(1, 1);
        else if (v == new Vector2(1, 1))
            return new Vector2(1, -1);
        else if (v == new Vector2(1, -1))
            return new Vector2(-1, -1);
        else if (v == new Vector2(-1, -1))
            return new Vector2(-1, 1);
        else
            return Vector2.right;
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
