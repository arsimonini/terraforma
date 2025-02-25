using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class Shove : MonoBehaviour, Cast_Spell
{
    
    private LayerMask hittableTilesMask;
    private LayerMask wallsMask;

    void Start(){
        hittableTilesMask = LayerMask.GetMask("Default");
        wallsMask = LayerMask.GetMask("Block Visibility");
    }

    public void castSpell(List<GameObject> targets, GameObject caster)
    {   

        if (targets.Count <= 0) {
            //UnityEngine.Debug.Log("SPELL FAILED");
            return;
        }

        //Figure out if you're a hero or enemy
        bool isGood = true;
        Hero_Character_Class heroCaster = caster.GetComponent<Hero_Character_Class>();
        if (heroCaster == null) {
            isGood = false;
            Enemy_Character_Class enemyCaster = caster.GetComponent<Enemy_Character_Class>();
        }
        Basic_Character_Class basicCaster = caster.GetComponent<Basic_Character_Class>();
        TileMap map = basicCaster.map;

        ClickableTile targetTile = targets[0].GetComponent<ClickableTile>();
        if (targetTile != null) {
            return;
        }

        Basic_Character_Class targetCharacter = targets[0].GetComponent<Basic_Character_Class>();
        if (targetCharacter != null) {
            //Determine direction
            int dX = targetCharacter.tileX-basicCaster.tileX;
            int dY = targetCharacter.tileY-basicCaster.tileY;
            int dist = 3;

            if (dX > 0) {
                moveCharacter(caster,map,targetCharacter,1,0,3);
            } else if (dX < 0) {
                moveCharacter(caster,map,targetCharacter,-1,0,3);
            } else if (dY > 0) {
                moveCharacter(caster,map,targetCharacter,0,1,3);
            } else if (dY < 0) {
                moveCharacter(caster,map,targetCharacter,0,-1,3);
            }
            return;
        }
        

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

    public void moveCharacter(GameObject caster, TileMap map, Basic_Character_Class target, int x, int y, int damage, int times = 3) {
        while (times > 0) {
            int futX = x; //Future position
            int futY = y;

            //Check if position to go to is available
            ClickableTile toTile = map.getTile(target.tileX+futX,target.tileY+futY);

                //Is it a world boundary? Stop right there
                if (toTile == null) {
                    times = 0;
                }
                //Is it a wall?
                
                //Is it a wold wall?
                    //If so,
            
            //Move position to match

            times --;
        }
        
        return;
    }
}
