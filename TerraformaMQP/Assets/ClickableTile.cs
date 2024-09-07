using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    public int TileX;
    public int TileY;
    public TileMap map;
    
    void OnMouseUp() {
        map.MoveSelectedUnitTo(TileX, TileY);
    }
}
