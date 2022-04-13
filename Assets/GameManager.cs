using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile pathTile;
    [SerializeField] Tile blockerTile;

    [SerializeField] int height = 32;
    [SerializeField] int width = 32;

    int[,] mapData;

    void Start() {
       mapData = new int[width,height];

       GenerateMap();
       ClearTilemap();
       DrawMap();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            tilemap.SetTile(GetCellPositionFromMouse(), pathTile);
        }

        if(Input.GetMouseButton(1))
        {
            tilemap.SetTile(GetCellPositionFromMouse(), blockerTile);
        }
    }

    private Vector3Int GetCellPositionFromMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return tilemap.WorldToCell(mouseWorldPosition);
    }

    private void GenerateMap()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                mapData[x,y] = 0;
            }
    }

    private void DrawMap()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                if (mapData[x,y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), pathTile);
                }
                else if (mapData[x,y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), blockerTile);
                }    
            }
    }

    private void ClearTilemap()
    {
        tilemap.ClearAllTiles();
    }
}
