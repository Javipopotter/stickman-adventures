using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnTile : MonoBehaviour
{
    Tilemap tilemapToSetUp;
    public RuleTile[] tile;
    GridLayout tilemapGrid;
    void Awake()
    {
        tilemapToSetUp = GameManager.Gm.tilemap;
        tilemapGrid = tilemapToSetUp.layoutGrid;
        tilemapToSetUp.SetTile(tilemapGrid.WorldToCell(transform.position), tile[Random.Range(0, tile.Length)]);
        Destroy(gameObject);
    }
}
