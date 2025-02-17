using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class Shatter : MonoBehaviour, Cast_Spell
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
        //UnityEngine.Debug.Log("CAST SHATTER");
        if (targets.Count <= 0) {
            //UnityEngine.Debug.Log("SPELL FAILED");
            return;
        }

        ClickableTile targetTile = targets[0].GetComponent<ClickableTile>();
        shatterWall(targetTile, caster);

        //Shatter adjacent walls
        TileMap map = targetTile.map;
        int x = targetTile.TileX;
        int y = targetTile.TileY;

        if (map.tileExists(x-1,y)) {shatterWall(map.clickableTiles[x-1,y],caster);}
        if (map.tileExists(x+1,y)) {shatterWall(map.clickableTiles[x+1,y],caster);}
        if (map.tileExists(x,y-1)) {shatterWall(map.clickableTiles[x,y-1],caster);}
        if (map.tileExists(x,y+1)) {shatterWall(map.clickableTiles[x,y+1],caster);}
        
        
            //UnityEngine.Debug.Log("Should activate?");
            //Shatter the tile
    }

    public void strikeAt(ClickableTile mainTile, int xOffSet, int yOffset, int damage = 2) {
        //Get the tilemap of ClickableTile and the real grid position of the target to hit
        TileMap map = mainTile.map;
        int RealX = mainTile.TileX + xOffSet;
        int RealY = mainTile.TileY + yOffset;

        if (map.inRange(RealX,RealY)) {
            ClickableTile targetTile = map.clickableTiles[RealX,RealY];
            map.gameObject.GetComponent<ReactionController>().checkReaction(map.clickableTiles[RealX, RealY], "Earth", "Shatterpoint", true);

            if (targetTile == null) { UnityEngine.Debug.Log("TargetTileFail");return; }
            GameObject characterAtTile = targetTile.characterOnTile;
            if (characterAtTile == null) { UnityEngine.Debug.Log("CharacterAtTileFail");return; }

            Basic_Character_Class targetChar = characterAtTile.GetComponent<Basic_Character_Class>(); 

            if (targetChar != null) {
                //UnityEngine.Debug.Log("GetComponent Succeeded");
                targetChar.takeMagicDamage(damage,"Earth");
            } else {
                //UnityEngine.Debug.Log("GetComponent Failed");
            }
        }

    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        List<GameObject> tiles = new List<GameObject>();
        switch (centerTile.map.checkDirection(targetersTile.gameObject.GetComponent<Transform>().position, centerTile.gameObject.GetComponent<Transform>().position, centerTile)){
            case "Top":
                if (centerTile.map.tileExists(centerTile.TileX, centerTile.TileY - 1) && !centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY -1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.tileExists(centerTile.TileX + 1, centerTile.TileY - 1) && !centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY -1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.tileExists(centerTile.TileX - 1, centerTile.TileY - 1) && !centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY -1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].canHit();
                }
                break;
            
            case "Bottom":
                if (centerTile.map.tileExists(centerTile.TileX, centerTile.TileY + 1) && !centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].canHit();
                }
                if (centerTile.map.tileExists(centerTile.TileX + 1, centerTile.TileY + 1) && !centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].canHit();
                }
                if (centerTile.map.tileExists(centerTile.TileX - 1, centerTile.TileY + 1) && !centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].canHit();
                }
                break;

            case "Right":
                if (centerTile.map.tileExists(centerTile.TileX - 1, centerTile.TileY) && !centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].canHit();
                }
                if (centerTile.map.tileExists(centerTile.TileX - 1, centerTile.TileY - 1) && !centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.tileExists(centerTile.TileX - 1, centerTile.TileY + 1) && !centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].canHit();
                }
                break;
            
            case "Left":
                if (centerTile.map.tileExists(centerTile.TileX + 1, centerTile.TileY) && !centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].canHit();
                }
                if (centerTile.map.tileExists(centerTile.TileX + 1, centerTile.TileY - 1) && !centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.tileExists(centerTile.TileX + 1, centerTile.TileY + 1) && !centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject.name.Contains("Wall")){
                    tiles.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].canHit();
                }
                break;
        }
        return tiles;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        for (int i = 0; i < tiles.Count; i++){
            tiles[i].GetComponent<ClickableTile>().removeHighlight();
        }
    }

    public void shatterWall(ClickableTile targetTile, GameObject caster){
        if (targetTile.tileIs != 20) {
            return;
        }
        targetTile.breakTile();

            
        Basic_Character_Class castChar = caster.GetComponent<Basic_Character_Class>();

        //Get the damage
        int damage = 0;
        Hero_Character_Class castHero = caster.GetComponent<Hero_Character_Class>();
        if (castHero != null) {
                //UnityEngine.Debug.Log("Valid Hero Character");
            damage = castHero.magic.moddedValue*3;
        } 
            //Enemy_Character_Class castEnemy = caster.GetComponent<Enemy_Character_Class>();
            //if (castEnemy != null) {
            //    damage = castEnemy.magic.moddedValue;
            //}

            //Figure out whether the wall was north, south, east, or west of Wold
        float cX = castChar.tileX;
        float cY = castChar.tileY;

        float tX = targetTile.TileX;
        float tY = targetTile.TileY;

        float dX = Mathf.Abs(cX-tX);
        float dY = Mathf.Abs(cY-tY);

        //Hit enemies on opposite end of tiles
        if (dX > dY) { 
            if (tX < cX) { //Damage enemies to the left of the tile. e   <- t  <- c
                strikeAt(targetTile,-1,0,damage);
                strikeAt(targetTile,-2,0,damage);
                strikeAt(targetTile,-1,1,damage);
                strikeAt(targetTile,-1,-1,damage);
            } else { //Hit enemies to the right of the tile.
                strikeAt(targetTile,1,0,damage);
                strikeAt(targetTile,2,0,damage);
                strikeAt(targetTile,1,1,damage);
                strikeAt(targetTile,1,-1,damage);
            }

        } else {//if (dY > dX) {
            if (tY < cY) { //Damage enemies to the south of the tile. 
                strikeAt(targetTile,0,-1,damage);
                strikeAt(targetTile,0,-2,damage);
                strikeAt(targetTile,1,-1,damage);
                strikeAt(targetTile,-1,-1,damage);
            } else { //Hit enemies to the north of the tile.
                strikeAt(targetTile,0,1,damage);
                strikeAt(targetTile,0,2,damage);
                strikeAt(targetTile,1,1,damage);
                strikeAt(targetTile,-1,1,damage);
            }
        }
    }
}
