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
        ClickableTile targetTile = targets[0].GetComponent<ClickableTile>();
        ClickableTile casterTile = casterScript.tile.GetComponent<ClickableTile>();

        TileMap map = casterTile.map;
        ReactionController reactController = map.gameObject.GetComponent<ReactionController>();

        reactController.checkReaction(targetTile, caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);

        switch (map.checkDirection(casterTile.gameObject.transform.position, targetTile.gameObject.transform.position, targetTile)){
            case "Right":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;

            case "Left":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;

            case "Top":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;

            case "Bottom":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;
            
            case "TopLeft":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;

            case "TopRight":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY - 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;
            
            case "BottomLeft":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX + 1, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;
            
            case "BottomRight":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX - 1, targetTile.TileY + 1], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;

            case "Failed To Find Direction":
                reactController.checkReaction(map.clickableTiles[targetTile.TileX, targetTile.TileY], caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Searing Wind", true);
                break;
        } 
    }

}
