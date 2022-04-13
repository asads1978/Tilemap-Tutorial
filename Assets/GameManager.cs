using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile pathTile;
    [SerializeField] Tile blockerTile;

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
}
