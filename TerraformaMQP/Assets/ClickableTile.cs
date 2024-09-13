using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    public int TileX;
    public int TileY;
    public TileMap map;
    //Determines how the tile affects characters that stand on it
    public List<string> statsToEffect;
    public List<int> effectAmounts;
    
    void OnMouseUp() {
        map.MoveSelectedUnitTo(TileX, TileY);
    }



}
