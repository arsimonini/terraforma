using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TileType
{
    public string name; //Name of the TileType
    public GameObject tileVisualPrefab; //A reference to the prefab asset
    public bool isWalkable; //If the tile type is walkable or not

    public int cost = 1; //The cost to walk through or step on the tile

    void Start()
    {

    }

}
