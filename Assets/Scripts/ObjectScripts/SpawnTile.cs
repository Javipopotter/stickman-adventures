using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnTile : MonoBehaviour
{
    Tilemap tilemapToSetUp;
    [SerializeField] RuleTile tile;
    GridLayout tilemapGrid;
    void Start()
    {
        tilemapToSetUp = GameManager.Gm.tilemap;
        tilemapGrid = tilemapToSetUp.layoutGrid;
        tilemapToSetUp.SetTile(tilemapGrid.WorldToCell(transform.position), tile);
        Destroy(transform.parent.transform.parent.gameObject);
    }
}
