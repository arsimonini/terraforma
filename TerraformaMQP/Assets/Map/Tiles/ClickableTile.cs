using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    //Relevant to lighting up the tile held over
    private Renderer tileRenderer; //The renderer of the tile
    private Color originalColor; //The base color of the tile
    public float highlightMultiplier = 1.2f; //A float that is used to multiply with the original color to create the highlight

    public int TileX; //The X location within the tile map
    public int TileY; //The Y location within the tile map
    public TileMap map; //A reference to the tile map
    public bool isWalkable = true; //Whether units can enter/pass through the tile, true if possible, false if not
    //Determines how the tile affects characters that stand on it
    public List<string> statsToEffect; //List of stats to effect when a character stands on the tile                Ex. ["attack", "speed", "defense"]
    public List<int> effectAmounts; //The amounts to change the listed stats when a character stands on the tile        [5, -2, 3]      Leads to increasing attack by 5, decreasing speed by 2, and increasing defense by 3
    public GameObject characterOnTile = null; //Reference to the character currently standing on the tile, null if no character is on the tile
    public Color color; //The color of the tile
    public int cost;

    public int tileWas = 0; //The tile type it used to be. Defaults to itself.
    public int tileIs = 0;//The tile type it currently is. Defaults, again, to itself.

    public bool isBreakable = false;
    public int hp = 10;
    public int maxHp = 10;

    public GameObject standardHighlight;
    public GameObject canHitHighlight;
    public GameObject cannotHitHightlight;

    public GameObject currentHightlight = null;

    public List<TileEffect> effectsOnTile; //List of effects currently on the tile

    public String tileName;
    
    //Set the renderer and color variables upon level load
    void Start() {
        tileRenderer = GetComponent<Renderer>();
        originalColor = tileRenderer.sharedMaterial.color;
        color = GetComponent<Renderer>().material.color;
    }

    public void OnMouseEnter() {
        if (map.aoeDisplayTiles != null && map.displayingAOE == true && 
            ((map.selectedUnit.GetComponent<Hero_Character_Class>() != null && map.selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell != null && map.selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.targeted == true) 
            || (map.selectedUnit.GetComponent<SummonClass>() != null && map.selectedUnit.GetComponent<SummonClass>().selectedAbility != null && map.selectedUnit.GetComponent<SummonClass>().selectedAbility.targeted == true))){
            map.removeAOEDisplay();
        }
        map.hidePath();
        
        //Highlight the tile upon hover
        Color highlightColor;
        //Check if the player currently has a unit targeting and this tile is within the current target list
        if (map.selectedUnit != null && map.selectedUnitScript.targeting == true && map.targetList.Contains(this.gameObject)) {
            //If so, set the highlight to be darker than normal
            /*
            float darkerHighlight = highlightMultiplier + 0.1f;
            highlightColor = originalColor * darkerHighlight;
            tileRenderer.material.color = highlightColor;
            if (transform.childCount > 0){
                foreach (Renderer rend in GetComponentsInChildren<Renderer>()){
                    if (!rend.gameObject.name.StartsWith("Outline") && !rend.gameObject.name.Contains("oak")){
                        rend.material.color = highlightColor;
                    }
                }   
            }
            */
        }
        else if (map.selectedUnit == null || (map.selectedUnitScript.isMoving && map.moving == false))
        {
            //Else, just do a normal highlight
            if (gameObject.tag == "Wall"){
                highlightColor = originalColor * highlightMultiplier;
                tileRenderer.material.color = highlightColor;
                if (transform.childCount > 0){
                    foreach (Renderer rend in GetComponentsInChildren<Renderer>()){
                        if (!rend.gameObject.name.StartsWith("Outline") && !rend.gameObject.name.Contains("oak")){
                            rend.material.color = highlightColor;
                        }
                    }   
                }
            }
            else {
            currentHightlight = Instantiate(standardHighlight);
            currentHightlight.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.52f, gameObject.transform.position.z);
            }
        }

        if (map.selectedUnitScript != null && map.selectedUnitScript.targeting && map.targetList.Contains(this.gameObject) || (characterOnTile != null && map.targetList.Contains(characterOnTile))){
            if (map.selectedUnitScript.attackType == "Spell"){
                map.displayAOE("Spell", this, size: map.selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.AOEsize, square: map.selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.square, map.selectedUnitScript.tile);
            }
            else if (map.selectedUnitScript.attackType == "Attack"){
                //UnityEngine.Debug.Log("Here");
                map.displayAOE("Attack", this, size: 0);
            }
            else if (map.selectedUnitScript.attackType == "Ability"){
                map.displayAOE("Ability", this, size: map.selectedUnit.GetComponent<SummonClass>().selectedAbility.AOEsize, square: map.selectedUnit.GetComponent<SummonClass>().selectedAbility.square, map.selectedUnitScript.tile);
            }
        }
        else if (map.selectedUnitScript != null && map.selectedUnitScript.targeting){
            if (map.selectedUnitScript.attackType == "Spell" && map.selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.targeted == false){
                map.displayAOE("Spell", this, size: map.selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.AOEsize, square: map.selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.square, map.selectedUnitScript.tile);
            }
            else if (map.selectedUnitScript.attackType == "Ability" && map.selectedUnit.GetComponent<SummonClass>().selectedAbility.targeted == false){
                map.displayAOE("Ability", this, size: map.selectedUnit.GetComponent<SummonClass>().selectedAbility.AOEsize, square: map.selectedUnit.GetComponent<SummonClass>().selectedAbility.square, map.selectedUnitScript.tile);
            }
        }

        //Highlight Path
        //Checks if a unit is selected, the unit is not an enemy, the unit's turn hasn't ended, the unit isn't targeting or moving, and the player isn't picking a spell
        if (map.selectedUnit != null && !map.selectedUnitScript.hasWalked && map.selectedUnit.gameObject.tag != "EnemyTeam" && map.selectedUnitScript.turnEnded == false && map.selectedUnitScript.targeting == false && map.moving == false && ((map.selectedUnit.GetComponent<Hero_Character_Class>() != null && map.selectedUnit.GetComponent<Hero_Character_Class>().pickingSpell == false) || map.selectedUnit.GetComponent<Hero_Character_Class>() == null)) {
            //Basically, if the unit is ready to move, then when hovering over a tile display the visual path to it
            map.visualPathTo(TileX,TileY);
        }
    }

    //Highlights the tile
    public void highlight()
    {
        Color highlightColor = originalColor * highlightMultiplier;
        tileRenderer.material.color = highlightColor;
        if (transform.childCount > 0){
            foreach (Renderer rend in GetComponentsInChildren<Renderer>()){
                if (!rend.gameObject.name.StartsWith("Outline") && !rend.gameObject.name.Contains("oak")){
                    rend.material.color = highlightColor;
                }
            }
        }
    }

    //Ends the highlight by returning the tile to the base color
    public void endHighlight()
    {

        GetComponent<Renderer>().material.color = color;
        if (transform.childCount > 0){
            foreach (Renderer rend in GetComponentsInChildren<Renderer>()){
                if(!rend.gameObject.name.StartsWith("Outline") && !rend.gameObject.name.Contains("oak")){
                    //UnityEngine.Debug.Log(rend.gameObject.name);
                    rend.material.color = color;
                }
            }
        }
    }


    public void OnMouseExit() {
        //Checks if the tile is currently withing the map's target list
        if (map.selectedUnit != null && map.selectedUnitScript.targeting == true && map.targetList.Contains(this.gameObject) || (characterOnTile != null && map.targetList.Contains(characterOnTile)))
        {
            //If so, the tile stays highlighted with the slightly lighter highlight
            //highlight();
            return;
        }
        if (map.displayingAOE == true){
            map.removeAOEDisplay();
        }
        //Else, the tile reverts back to its original color without a highlight
        endHighlight();
        removeHighlight();
    }

    //Adds an effect to the tile
    //Takes in an effect to add
    public void addEffectToTile(TileEffect effect){
        //Adds the effect to the effectsOnTile list
        effectsOnTile.Add(effect);
        //Updates the tile's list of statsToEffect and effectAmounts by either adding new stats to effect if they aren't already within the original list, or adding the new amounts to the pre-existing amounts
        for (int i = 0; i < effect.statToEffect.Count; i++){
            if (statsToEffect.Contains(effect.statToEffect[i])){
                int statLoc = checkList(effect.statToEffect[i]);
                effectAmounts[statLoc] += effect.amountToEffect[i];
            }
            else{
                statsToEffect.Add(effect.statToEffect[i]);
                effectAmounts.Add(effect.amountToEffect[i]);
            }
        }
        cost += effect.movementCostIncrease;
        //If there is a character on the tile then it's tile effect is then updated to reflect the new stats
        if (characterOnTile != null){
            updateTileEffect();
        }
        //effect.tileEffectPrefab.GetComponent<tileEffectActions>().react(effectsOnTile, this, effect);
    }

    //Removes an effect from the tile, functions almost exactly in the same way as the addEffectToTile
    //Takes in the effect to remove
    public void removeEffectFromTile(TileEffect effect){
        //Removes the effect from the effectsOnTile list
        effectsOnTile.Remove(effect);
        //Finds the correct stat and removes the amount that was added when the tile effect was added, if at that point the amount is 0, then the entry is the list is removed
        for (int i = 0; i < effect.statToEffect.Count; i++){
            int statLoc = checkList(effect.statToEffect[i]);
            effectAmounts[statLoc] -= effect.amountToEffect[i];
            if (effectAmounts[statLoc] == 0){
                effectAmounts.Remove(effectAmounts[statLoc]);
                statsToEffect.Remove(statsToEffect[statLoc]);
            }
        }
        if (effect.duration == 0){
            effect.tileEffectPrefab.GetComponent<tileEffectActions>().endOfDurationEffect(this);
        }
        else if (effect.playerTeam){
            map.GetComponent<StatusEffectController>().playerTeamTileEffects.Remove(effect);
        }
        else{
            map.GetComponent<StatusEffectController>().enemyTeamTileEffects.Remove(effect);  
        }
        cost -= effect.movementCostIncrease;
        //If there is a character on the tile then it's tile effect is then updated to reflect the new stats
        if (characterOnTile!= null){
            updateTileEffect();
        }
        effect.removeEffect();

        //remove from appropriate team list
        //map.GetComponent<StatusEffectController>().playerTeamTileEffects.Remove(effect);
        //map.GetComponent<StatusEffectController>().enemyTeamTileEffects.Remove(effect);
    }

    //Recreates the tile effect with the current stat changes, updating the character on the tile
    public void updateTileEffect(){
        StatusEffect newEffect = new StatusEffect();
        newEffect.initializeTileEffect(statsToEffect, name, effectAmounts, characterOnTile, name + " Effect");
    }
    
    //---REDUNDANT, NEED TO DESTROY---
    public int checkList(string statToEffect){
        for (int i = 0; i < statsToEffect.Count; i++){
            if (statsToEffect[i] == statToEffect){
                return i;
            }
        }
        return -99;
    }

    public void breakTile() {
        if (!isBreakable) return;

        switch (tileIs) {
            default: //We're assuming Wold Wall by default
                if (tileWas == 0 || tileWas == 1 || tileWas == 2 || tileWas == 3 || tileWas == 14) { //Grass, Dirt, Mud, Land Ice, Ashen -> Dirt
                    map.swapTiles(map.clickableTiles[TileX,TileY],1,true);
                } else if (tileWas == 4 || tileWas == 9) { //Shallow Water / Ice -> Shallow Water
                    map.swapTiles(map.clickableTiles[TileX,TileY],9,true); 
                } else { //Stone -> Stone, Sand -> Sand, other things shouldn't be in this condition anyways, so this also acts as a failsafe
                    map.swapTiles(map.clickableTiles[TileX,TileY],tileWas,true); 
                }

            break;
        }
    }

    public bool canBecomeWoldWall() {
        switch (tileIs) {
            case 0: return true; break;
            case 1: return true; break;
            case 2: return true; break;
            case 3: return true; break;
            case 4: return true; break;
            case 5: return true; break;
            case 6: return false; break;
            case 7: return false; break;
            case 8: return false; break;
            case 9: return true; break;
            case 10: return false; break;
            case 11: return true; break;
            case 12: return false; break;
            case 13: return false; break;
            case 14: return true; break;
            case 15: return false; break;
            case 16: return false; break;
            case 17: return false; break;
            case 18: return false; break;
            case 19: return true; break;
            case 20: return false; break;
            default: return true; break;
        }
    }
    /*
grass -> dirt
dirt -> dirt
mud -> dirt
land ice -> dirt
water ice -> shallow water
stone/rock -> stone/rock
wood -> ???
light forest -> x
dense forest -> x
shallow water -> shallow water
deep water -> x
sand -> sand
glass -> x
metal -> x
ashen -> dirt
    */

    public void canHit(){
        Destroy(currentHightlight);
        currentHightlight = Instantiate(canHitHighlight);
        currentHightlight.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.52f, gameObject.transform.position.z);
        //currentHightlight.transform.localScale = new Vector3(0.05f,0.05f,0.05f);
    }

    public void removeHighlight(){
        Destroy(currentHightlight);
    }
}
