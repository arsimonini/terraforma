using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    public int TileX;
    public int TileY;
    public TileMap map;
    public bool isWalkable = true;
    //Determines how the tile affects characters that stand on it
    public List<string> statsToEffect;
    public List<int> effectAmounts;
    public GameObject characterOnTile = null;
    public Color color;
    
    void Start()
    {
        color = GetComponent<Renderer>().material.color;
    }

    void OnMouseUp() {
        map.MoveSelectedUnitTo(TileX, TileY);
    }

    public void highlight()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void endHighlight()
    {
        GetComponent<Renderer>().material.color = color;
    }





}
