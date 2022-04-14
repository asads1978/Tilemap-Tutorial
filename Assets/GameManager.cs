using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class RandomTile
    {
        public Tile tile;
        [Range(0f,1f)] public float weight;
    }

    [SerializeField] Tilemap tilemap;
    [SerializeField] RandomTile[] pathTiles;
    [SerializeField] Tile blockerTile;

    [SerializeField] int height = 32;
    [SerializeField] int width = 32;

    [SerializeField] PlayerController player;

    int[,] mapData;

    void Start()
    {
        mapData = new int[width, height];

        Restart();

    }

    void Update()
    {
        CheckVictory();
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }


    private Vector3Int GetCellPositionFromMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return tilemap.WorldToCell(mouseWorldPosition);
    }



    public bool IsInBounds(Vector3Int targetPosition)
    {
        if (targetPosition.x < 0 || targetPosition.x >= GetWidth() || targetPosition.y < 0 || targetPosition.y >= GetHeight())
        {
            return false;
        }

        return true;
    }

    private void DrawMap()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                if (mapData[x,y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), GetRandomTile(pathTiles));
                }
                else if (mapData[x,y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), blockerTile);
                }    
            }
    }

    private Tile GetRandomTile(RandomTile[] RandomTiles)
    {

        float totalWeight = 0f;

        foreach(RandomTile randomTile in RandomTiles)
        {
            totalWeight += randomTile.weight;
        }

        float rand = Random.Range(0,totalWeight);

        foreach (RandomTile randomTile in RandomTiles)
        {
            if (rand < randomTile.weight)
            {
                return randomTile.tile;
            }

            rand -= randomTile.weight;
        }

        return RandomTiles[0].tile;

    }

    private void Restart()
    {
        GenerateMap();
        ClearTilemap();
        DrawMap();
        SetPlayerStart();
    }

    private void CheckVictory()
    {
        if (player.transform.position.y >= GetHeight() - 1)
        {
            Restart();
        }
    }

    private void GenerateMap()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = 0;
            }
    }

    private void SetPlayerStart()
    {
        int start = Random.Range(0, GetWidth());

        player.transform.position = new Vector3(start, 0, player.transform.position.z);
    }

    private void ClearTilemap()
    {
        tilemap.ClearAllTiles();
    }


}
