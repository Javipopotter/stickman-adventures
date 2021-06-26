using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector2 dir;
    [SerializeField]Bounds area;
    [Range(0, 1)]
    public float Hardness;

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        area.center = new Vector3(transform.position.x, transform.position.y, 0);
        dir = transform.position - player.transform.position;
        dir.Normalize();
        if (!area.Contains(player.transform.position))
        {
            transform.Translate(new Vector2(-dir.x * Hardness, 0));
            if (player.transform.position.y >= transform.position.y + area.extents.y)
            {
                YUpdate(area.extents.y);
            }
            if (player.transform.position.y <= transform.position.y - area.extents.y)
            {
                YUpdate(-area.extents.y);
            } 
        }
    }

    public void YUpdate(float y)
    {
        transform.position = new Vector3(transform.position.x, player.transform.position.y - y, transform.position.z); // update y
        //transform.Translate(new Vector2(0, -dir.y * 0.5f));
    }

    public void RestartCameraPosition()
    {
        YUpdate(0);
    }

    private void OnDrawGizmosSelected()
    {
        area.center = transform.position;
        Gizmos.DrawCube(area.center, area.extents * 2);
    }
}
