using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public RoomStructure TypeOfRoom;

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
        Destroy(this.gameObject);
    }
}
