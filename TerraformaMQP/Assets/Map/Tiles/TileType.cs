using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TileType {
    public string name; //Name of the TileType
    public GameObject tileVisualPrefab; //A reference to the prefab asset
    public bool isWalkable; //If the tile type is walkable or not
    public bool isBreakable = false; //If breakable, can be attacked and as such can revert to another form
    public int cost = 1; //The cost to walk through or step on the tile
    public int breakBecome = 0; //The type needed

    public float hp = 20;
    public float maxHp;
    void Start() {
        maxHp = hp;
    }

    void takeDamage(float dmg = 0f) {
        if (!isBreakable) {
            hp = hp - dmg;
        }
    }

}
