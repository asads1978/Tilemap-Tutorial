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
    [SerializeField] int blindWalkMaxSteps = 1000;

    [SerializeField] PlayerController player;
    
    [Range(0,10)]
    [SerializeField] int SightRange = 2;

    int[,] mapData;
    int[,] playerMap;

    int start = 1;

    void Start()
    {
        mapData = new int[width, height];
        playerMap = new int[width,height];

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

    public bool IsValidPosition(Vector3Int targetPosition)
    {
        if(IsInBounds(targetPosition) && mapData[targetPosition.x,targetPosition.y] == 0)
        {
            return true;
        }
        return false;
    }

    public bool IsInBounds(Vector3Int targetPosition)
    {
        if (targetPosition.x < 0 || targetPosition.x >= GetWidth() || targetPosition.y < 0 || targetPosition.y >= GetHeight())
        {
            return false;
        }

        return true;
    }

    public void DrawMap()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                if(playerMap[x,y] == 1 && tilemap.GetTile(new Vector3Int(x,y,0)) == null)
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
        SetStart();
        GenerateMap();
        ClearTilemap();
        PlacePlayer();
        RevealMap(new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y, 0));
        DrawMap();
    }

    private void SetStart()
    {
        start = Random.Range(1, GetWidth()-2);
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
        FillMap(1);
        BlindWalk();
        playerMap = new int[width,height];
    }

    public void RevealMap(Vector3Int centerPosition)
    {
        for(int x = 0; x <= SightRange * 2; x++)
            for(int y = 0; y <= SightRange * 2; y++)
            {
                int xPosition = centerPosition.x + x - SightRange;
                int yPosition = centerPosition.y + y - SightRange;

                if(IsInBounds(new Vector3Int(xPosition,yPosition,0)))
                {
                    playerMap[xPosition, yPosition] = 1;
                }
                
            } 
    }

    private void BlindWalk()
    {
        Vector3Int currentPosition = new Vector3Int(start,0,0);
        Vector3Int targetPosition = currentPosition;
        Vector3Int moveVector = Vector3Int.zero;

        int direction = 0;

        mapData[currentPosition.x,currentPosition.y] = 0;

        int limit = 0;

        while(limit < blindWalkMaxSteps && currentPosition.y != GetHeight() - 1  )
        {
            direction = Random.Range(1,5);

            switch(direction)
            {
                case 1:
                    moveVector = Vector3Int.up;
                    break;
                case 2:
                    moveVector = Vector3Int.down;
                    break;
                case 3:
                    moveVector = Vector3Int.left;
                    break;
                case 4:
                    moveVector = Vector3Int.right;
                    break;
                default:
                    moveVector = Vector3Int.zero;
                    break;    
            }

            targetPosition = currentPosition + moveVector;

            if(IsInFrame(targetPosition))
            {
                currentPosition = targetPosition;
                mapData[currentPosition.x, currentPosition.y] = 0;
                
            }

            limit++;
            

        }


    }

    private bool IsInFrame(Vector3Int targetPosition)
    {
        if (targetPosition.x < 1 || targetPosition.x >= GetWidth() -1 || targetPosition.y < 1 || targetPosition.y >= GetHeight())
        {
            return false;
        }

        return true;
    }

    private void FillMap(int fill)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = fill;
            }
    }

    private void PlacePlayer()
    {
        player.transform.position = new Vector3(start, 0, player.transform.position.z);
    }

    private void ClearTilemap()
    {
        tilemap.ClearAllTiles();
    }


}
