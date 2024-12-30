using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class SearingWind_Spell : MonoBehaviour, Cast_Spell
{

    public void castSpell(List<GameObject> targets, GameObject caster){
        Basic_Character_Class casterScript = caster.GetComponent<Basic_Character_Class>();
        ClickableTile targetTile;
        if (targets[0].GetComponent<ClickableTile>()){
            targetTile = targets[0].GetComponent<ClickableTile>();
        }
        else {
            targetTile = targets[0].GetComponent<Basic_Character_Class>().tile;
        }
        ClickableTile casterTile = casterScript.tile.GetComponent<ClickableTile>();

        TileMap map = casterTile.map;
        ReactionController reactController = map.gameObject.GetComponent<ReactionController>();

        reactController.checkReaction(targetTile, caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
        bool push = false;
        if (targetTile.characterOnTile != null){
            push = true;
        }

        switch (map.checkDirection(casterTile.gameObject.transform.position, targetTile.gameObject.transform.position, targetTile)){
            case "Right":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                if (push){
                    map.pushCharacter(targetTile.characterOnTile.GetComponent<Basic_Character_Class>(), targetTile.TileX, targetTile.TileY, "Left", 1);
                }
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);

                break;

            case "Left":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                if (push){
                    map.pushCharacter(targetTile.characterOnTile.GetComponent<Basic_Character_Class>(), targetTile.TileX, targetTile.TileY, "Right", 1);
                }
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;

            case "Top":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                if (push){
                    map.pushCharacter(targetTile.characterOnTile.GetComponent<Basic_Character_Class>(), targetTile.TileX, targetTile.TileY, "Down", 1);
                }
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;

            case "Bottom":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                if (push){
                    map.pushCharacter(targetTile.characterOnTile.GetComponent<Basic_Character_Class>(), targetTile.TileX, targetTile.TileY, "Up", 1);
                }
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;
            
            case "TopLeft":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                if (push){
                    map.pushCharacter(targetTile.characterOnTile.GetComponent<Basic_Character_Class>(), targetTile.TileX, targetTile.TileY, "RightDown", 1);
                }
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;

            case "TopRight":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                if (push){
                    map.pushCharacter(targetTile.characterOnTile.GetComponent<Basic_Character_Class>(), targetTile.TileX, targetTile.TileY, "DownLeft", 1);
                }
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;
            
            case "BottomLeft":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                if (push){
                    map.pushCharacter(targetTile.characterOnTile.GetComponent<Basic_Character_Class>(), targetTile.TileX, targetTile.TileY, "RightUp", 1);
                }
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;
            
            case "BottomRight":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                if (push){
                    map.pushCharacter(targetTile.characterOnTile.GetComponent<Basic_Character_Class>(), targetTile.TileX, targetTile.TileY, "LeftUp", 1);
                }
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;

            case "Failed To Find Direction":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;
        } 
    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        List<GameObject> list = new List<GameObject>();
        centerTile.canHit();
        list.Add(centerTile.gameObject);
        switch (centerTile.map.checkDirection(targetersTile.gameObject.transform.position, centerTile.gameObject.transform.position, centerTile)){
            case "Right":
                if (centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].canHit();
                }
                break;

            case "Left":
                if (centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].canHit();
                }
                break;

            case "Top":
                if (centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].canHit();        
                }
                if (centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].canHit();        
                }
                if (centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].canHit();        
                }
                break;

            case "Bottom":
                if (centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].canHit();        
                }
                if (centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].canHit();        
                }
                if (centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].canHit();        
                }
                break;
            
            case "TopLeft":
                if (centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].canHit();
                }
                break;

            case "TopRight":
                if (centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY - 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].canHit();
                }
                break;
            
            case "BottomLeft":
                if (centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY + 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].canHit();
                }
                break;
            
            case "BottomRight":
                if (centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY + 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX, centerTile.TileY + 1].canHit();
                }
                if (centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].gameObject.tag != "Wall"){
                    list.Add(centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].gameObject);
                    centerTile.map.clickableTiles[centerTile.TileX - 1, centerTile.TileY].canHit();
                }
                break;

            case "Failed To Find Direction":
                UnityEngine.Debug.Log("You broke it, SearingWind's targeting display hasn't found a direction");
                break;
        }
        return list;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        for (int i = 0; i < tiles.Count; i++){
            tiles[i].GetComponent<ClickableTile>().removeHighlight();
        }
    }

}
