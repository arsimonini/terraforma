using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class Wall_Spell : MonoBehaviour, Cast_Spell
{
    public int spread;
    public TileEffect BurningTileEffect;

    private LayerMask hittableTilesMask;
    private LayerMask wallsMask;

    void Start(){
        hittableTilesMask = LayerMask.GetMask("Default");
        wallsMask = LayerMask.GetMask("Block Visibility");
    }

    public void castSpell(List<GameObject> targets, GameObject caster)
    {
        int sumCorrect = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            
            ClickableTile targetTile;
            if (targets[0].GetComponent<ClickableTile>()){
                targetTile = targets[0].GetComponent<ClickableTile>();
                float targetX = targetTile.TileX;
                float targetY = targetTile.TileY;
                TileMap map = targetTile.map;

                Basic_Character_Class casterCharacter = caster.GetComponent<Basic_Character_Class>();    
                
                if (targetTile == null || casterCharacter == null) {
                    return;
                }

                if (acceptableLocation(targetTile)){
                    map.swapTiles(targetTile,20,false);
                }

                float castX = casterCharacter.tileX;
                float castY = casterCharacter.tileY;

                float castDistanceX = Mathf.Abs(castX - targetX);
                float castDistanceY = Mathf.Abs(castY - targetY);

                if (castDistanceX > castDistanceY) {
                    //North
                    if (targetY < map.clickableTiles.GetLength(1)-1) {
                        ClickableTile north = map.clickableTiles[(int) targetX,(int) targetY+1];
                        if (acceptableLocation(north)) {
                            map.swapTiles(north,20,false);
                        }
                    }                

                    //South
                    if (targetY > 0) {
                        ClickableTile south = map.clickableTiles[(int) targetX,(int) targetY-1];
                        if (acceptableLocation(south)) {
                            map.swapTiles(south,20,false);
                        }
                    }
                } else {

                    //East
                    if (targetX > 0) {
                        ClickableTile east = map.clickableTiles[(int) targetX-1,(int) targetY];
                        if (acceptableLocation(east)) {
                            map.swapTiles(east,20,false);
                        }
                    }

                    //West
                    if (targetX < map.clickableTiles.GetLength(0)-1) {
                        ClickableTile west = map.clickableTiles[(int) targetX+1,(int) targetY];
                        if (acceptableLocation(west)) {
                            map.swapTiles(west,20,false);
                        }
                    }
                }


                
            }




        }
    }

    public bool acceptableLocation(ClickableTile t) {
        
        if (t.isWalkable && t.gameObject.tag != "Wall") { 
            //Must be one of these types of walls
            if (t.tileIs == 0 || t.tileIs == 1 || t.tileIs == 2 || t.tileIs == 3 || t.tileIs == 4 || t.tileIs == 5 || t.tileIs == 9 || t.tileIs == 11 || t.tileIs == 14) {           
                return true;
            }
        }
        return false;
    }
}
