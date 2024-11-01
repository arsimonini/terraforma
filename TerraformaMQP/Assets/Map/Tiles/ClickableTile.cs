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

    public List<TileEffect> effectsOnTile; //List of effects currently on the tile
    
    //Set the renderer and color variables upon level load
    void Start() {
        tileRenderer = GetComponent<Renderer>();
        originalColor = tileRenderer.sharedMaterial.color;
        color = GetComponent<Renderer>().material.color;
    }




    void OnMouseEnter() {
        //Highlight the tile upon hover
        Color highlightColor;
        //Check if the player currently has a unit targeting and this tile is withing the current target list
        if (map.selectedUnit != null && map.selectedUnitScript.targeting == true && map.targetList.Contains(this.gameObject))
        {
            //If so, set the highlight to be darker than normal
            float darkerHighlight = highlightMultiplier + 0.1f;
            highlightColor = originalColor * darkerHighlight;
            tileRenderer.material.color = highlightColor;
        }
        else
        {
            //Else, just do a normal highlight
            highlightColor = originalColor * highlightMultiplier;
            tileRenderer.material.color = highlightColor;
        }

        //Highlight Path
        //Checks if a unit is selected, the unit is not an enemy, the unit's turn hasn't ended, the unit isn't targeting or moving, and the player isn't picking a spell
        if (map.selectedUnit != null && map.selectedUnit.gameObject.tag != "EnemyTeam" && map.selectedUnitScript.turnEnded == false && map.selectedUnitScript.targeting == false && map.moving == false && ((map.selectedUnit.GetComponent<Hero_Character_Class>() != null && map.selectedUnit.GetComponent<Hero_Character_Class>().pickingSpell == false) || map.selectedUnit.GetComponent<Hero_Character_Class>() == null)) {
            //Basically, if the unit is ready to move, then when hovering over a tile display the visual path to it
            map.visualPathTo(TileX,TileY);
        }
    }

    //Highlights the tile
    public void highlight()
    {
        Color highlightColor = originalColor * highlightMultiplier;
        tileRenderer.material.color = highlightColor;
    }

    //Ends the highlight by returning the tile to the base color
    public void endHighlight()
    {

        GetComponent<Renderer>().material.color = color;
    }


    void OnMouseExit() {
        //Checks if the tile is currently withing the map's target list
        if (map.selectedUnit != null && map.selectedUnitScript.targeting == true && map.targetList.Contains(this.gameObject))
        {
            //If so, the tile stays highlighted with the slightly lighter highlight
            Color highlightColor = originalColor * highlightMultiplier;
            tileRenderer.material.color = highlightColor;
            return;
        }
        //Else, the tile reverts back to its original color without a highlight
        tileRenderer.material.color = originalColor;
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
        if (characterOnTile!= null){
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
        cost -= effect.movementCostIncrease;
        //If there is a character on the tile then it's tile effect is then updated to reflect the new stats
        if (characterOnTile!= null){
            updateTileEffect();
        }
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

}
