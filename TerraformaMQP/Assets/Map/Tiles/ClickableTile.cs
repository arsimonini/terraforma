using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    //Relevant to lighting up the tile held over
    private Renderer tileRenderer;
    private Color originalColor;
    public float highlightMultiplier = 1.2f; 

    public int TileX;
    public int TileY;
    public TileMap map;
    //Determines how the tile affects characters that stand on it
    public List<string> statsToEffect;
    public List<int> effectAmounts;
    
    void Start() {
        tileRenderer = GetComponent<Renderer>();
        originalColor = tileRenderer.material.color;
    }

    void OnMouseUp() {
        map.MoveSelectedUnitTo(TileX, TileY);
    }

    void OnMouseEnter() {

        Color highlightColor = originalColor * highlightMultiplier;
        tileRenderer.material.color = highlightColor;

        //Highlight Path
        if (map.selectedUnit != null) {
            map.visualPathTo(TileX,TileY);
            //UnityEngine.Debug.Log(currentPath[0].x)
        }
    }

    void OnMouseExit() {
        
        tileRenderer.material.color = originalColor;
    }

}
