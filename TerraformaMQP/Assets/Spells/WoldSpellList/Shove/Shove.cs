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
            int dX = targetTile.TileX-basicCaster.tileX;
            int dY = targetTile.TileY-basicCaster.tileY;
            int dist = 2;

            if (dX > 0) {
                movePillar(targetTile,"Right",2);
            } else if (dX < 0) {
                movePillar(targetTile,"Left",2);
            } else if (dY > 0) {
                movePillar(targetTile,"Up",2);
            } else {
                movePillar(targetTile,"Down",2);
            }

            return;
        }

        Basic_Character_Class targetCharacter = targets[0].GetComponent<Basic_Character_Class>();
        if (targetCharacter != null) {
            //Determine direction
            int dX = targetCharacter.tileX-basicCaster.tileX;
            int dY = targetCharacter.tileY-basicCaster.tileY;
            int dist = 2;
            int dmg = 6;

            if (dX > 0) {
                map.pushCharacter(targetCharacter,targetCharacter.tileX,targetCharacter.tileY,"Right",dist);
                if (checkBackTile(map,targetCharacter.tileX,targetCharacter.tileY,1,0)) {targetCharacter.takePhysicalDamage(dmg);};
            } else if (dX < 0) {
                map.pushCharacter(targetCharacter,targetCharacter.tileX,targetCharacter.tileY,"Left",dist);
                if (checkBackTile(map,targetCharacter.tileX,targetCharacter.tileY,-1,0)) {targetCharacter.takePhysicalDamage(dmg);};
            } else if (dY > 0) {
                map.pushCharacter(targetCharacter,targetCharacter.tileX,targetCharacter.tileY,"Up",dist);
                if (checkBackTile(map,targetCharacter.tileX,targetCharacter.tileY,0,1)) {targetCharacter.takePhysicalDamage(dmg);};
            } else if (dY < 0) {
                map.pushCharacter(targetCharacter,targetCharacter.tileX,targetCharacter.tileY,"Down",dist);
                if (checkBackTile(map,targetCharacter.tileX,targetCharacter.tileY,0,-1)) {targetCharacter.takePhysicalDamage(dmg);};
            }
            return;
        }

    }

    public void movePillar(ClickableTile ct, string dir, int strength) {
        TileMap map = ct.map;
        int currX = ct.TileX;
        int currY = ct.TileY;

        int str = strength;

        while (str > 0) {
            ClickableTile oct = map.getTile(currX,currY);

            switch (dir) {
                case "Left":
                    currX --;
                break;

                case "Right":
                    currX ++;
                break;

                case "Up":
                    currY ++;
                break;

                case "Down":
                    currY --;
                break;
            }

            //ct.breakTile();

            ClickableTile nct = map.getTile(currX,currY);
            if (nct != null && nct.characterOnTile == null && nct.isWalkable == true) {
                //make a pillar at currX, currY;
                map.swapTiles(nct,20,false);
                if (oct != null) {oct.breakTile();}
                str --;
            } else {
                str = 0;
            }
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

    //Check to see if the block behind them is a wold wall or wall, breaking the tile if breakable
    public bool checkBackTile(TileMap map, int tX, int tY, int xOff, int yOff) {
        ClickableTile ct = map.getTile(tX+xOff,tY+yOff);
        if (ct != null) {
            if (ct.tileName == "Wall") {return true;};
            if (ct.tileName == "Wold Wall") {ct.breakTile(); return true;};
        } 
        return false;
    }


}
