using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    [SerializeField]
    public List<StatusEffect> playerTeamEffects; //List of effects that have been created by the player team
    public List<StatusEffect> enemyTeamEffects; //List of effects that have been created by the enemy team

    public List<TileEffect> playerTeamTileEffects; //List of TileEffects that have been created by the player team
    public List<TileEffect> enemyTeamTileEffects; //List of TileEffects that have been created by the enemy team

    //Advances all of the player team effects
    public void playerTeamEffectsAdvance(){
        //Checks if the list of player effects is empty
        if (playerTeamEffects != null){
            //Creates a list that can be filled with effects that have run out
            List<StatusEffect> effectsToRemove = new List<StatusEffect>();
            for (int i = 0; i < playerTeamEffects.Count; i++){
                //For each item in the list of player effects, reduces their duration and if the duration has ended they are added to the effectsToRemove list
                if (!playerTeamEffects[i].reduceDuration())
                {
                    effectsToRemove.Add(playerTeamEffects[i]);
                }
            }
            //If there are effects to remove, they are removed from the list
            if (effectsToRemove != null){
                for (int i = 0; i < effectsToRemove.Count; i++)
                {
                    playerTeamEffects.Remove(effectsToRemove[i]);
                }
            }
        }
        //Operates much like the above section
        //Checks if the list of player tile effects is empty
        if (playerTeamTileEffects != null){
            //Creates a list that can be filled with effects that have run out
            List<TileEffect> effectsToRemove = new List<TileEffect>();
            for (int i = 0; i < playerTeamTileEffects.Count; i++){
                //For each item in the list of player tile effects, reduces their duration and if the duration has ended thay are added to the effectsToRemove list
                if (!playerTeamTileEffects[i].reduceDuration()){
                    effectsToRemove.Add(playerTeamTileEffects[i]);
                }
            }
            //If there are effects to remove, they are removed from the list
            if (effectsToRemove != null){
                for (int i = 0; i < effectsToRemove.Count; i++){
                    playerTeamTileEffects.Remove(effectsToRemove[i]);
                }
            }
        }

    }

    //Functions exactly the same as the above function but for the enemy team lists of effects
    public void enemyTeamEffectsAdvance() { 
        //Checks if the list of enemy effects is empty
        if (enemyTeamEffects != null)
        {
            //Creates a list that can be filled with effects that have run out
            List<StatusEffect> effectsToRemove = new List<StatusEffect>();
            for (int i = 0; i < enemyTeamEffects.Count; i++)
            {
                //For each item in the list of enemy effects, reduces their duration and if the duration has ended thay are added to the effectsToRemove list
                if (!enemyTeamEffects[i].reduceDuration())
                {
                    effectsToRemove.Add(enemyTeamEffects[i]);
                }
            }
            //If there are effects to remove, they are removed from the list
            if (effectsToRemove != null){
                for (int i = 0; i < effectsToRemove.Count; i++)
                {
                    enemyTeamEffects.Remove(effectsToRemove[i]);
                }
            }
        }
        //Checks if the list of enemy tile effects is empty
        if (enemyTeamTileEffects != null){
            //Creates a list that can be filled with effects that have run out
            List<TileEffect> effectsToRemove = new List<TileEffect>();
            for (int i = 0; i < enemyTeamTileEffects.Count; i++){
                //For each item in the list of enemy tile effects, reduces their duration and if the duration has ended thay are added to the effectsToRemove list
                if (!enemyTeamTileEffects[i].reduceDuration()){
                    effectsToRemove.Add(enemyTeamTileEffects[i]);
                }
            }
            //If there are effects to remove, they are removed from the list
            if (effectsToRemove != null){
                for (int i = 0; i < effectsToRemove.Count; i++){
                    enemyTeamTileEffects.Remove(effectsToRemove[i]);
                }
            }
        }
    }

}
