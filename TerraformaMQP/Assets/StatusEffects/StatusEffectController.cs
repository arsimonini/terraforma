using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    [SerializeField]
    public List<StatusEffect> playerTeamEffects;
    public List<StatusEffect> enemyTeamEffects;

    public List<TileEffect> playerTeamTileEffects;
    public List<TileEffect> enemyTeamTileEffects;

    public void playerTeamEffectsAdvance(){
        if (playerTeamEffects != null){
            List<StatusEffect> effectsToRemove = new List<StatusEffect>();
            for (int i = 0; i < playerTeamEffects.Count; i++){
                if (!playerTeamEffects[i].reduceDuration())
                {
                    effectsToRemove.Add(playerTeamEffects[i]);
                }
            }
            if (effectsToRemove != null){
                for (int i = 0; i < effectsToRemove.Count; i++)
                {
                    playerTeamEffects.Remove(effectsToRemove[i]);
                }
            }
        }
        if (playerTeamTileEffects != null){
            List<TileEffect> effectsToRemove = new List<TileEffect>();
            for (int i = 0; i < playerTeamTileEffects.Count; i++){
                if (!playerTeamTileEffects[i].reduceDuration()){
                    effectsToRemove.Add(playerTeamTileEffects[i]);
                }
            }
            if (effectsToRemove != null){
                for (int i = 0; i < effectsToRemove.Count; i++){
                    playerTeamTileEffects.Remove(effectsToRemove[i]);
                }
            }
        }

    }

    public void enemyTeamEffectsAdvance() { 
        if (enemyTeamEffects != null)
        {
            List<StatusEffect> effectsToRemove = new List<StatusEffect>();
            for (int i = 0; i < enemyTeamEffects.Count; i++)
            {
                if (!enemyTeamEffects[i].reduceDuration())
                {
                    effectsToRemove.Add(enemyTeamEffects[i]);
                }
            }
            if (effectsToRemove != null){
                for (int i = 0; i < effectsToRemove.Count; i++)
                {
                    enemyTeamEffects.Remove(effectsToRemove[i]);
                }
            }
        }
        if (enemyTeamTileEffects != null){
            List<TileEffect> effectsToRemove = new List<TileEffect>();
            for (int i = 0; i < enemyTeamTileEffects.Count; i++){
                if (!enemyTeamTileEffects[i].reduceDuration()){
                    effectsToRemove.Add(enemyTeamTileEffects[i]);
                }
            }
            if (effectsToRemove != null){
                for (int i = 0; i < effectsToRemove.Count; i++){
                    enemyTeamTileEffects.Remove(effectsToRemove[i]);
                }
            }
        }
    }
    /*

    public void enemyTeamEndOfTurnTileEffectsAdvance(){
        if (enemyTeamTileEffects != null){
            for (int i = 0; i < enemyTeamTileEffects.Count; i++){
                enemyTeamTileEffects[i].tileEffectPrefab.GetComponent<tileEffectActions>().performEndOfTurnEffect(enemyTeamTileEffects[i].tile);
            }
        }
    }

    public void enemyTeamStartOfTurnTileEffectsAdvance(){
        if (enemyTeamTileEffects != null){
            for (int i = 0; i < enemyTeamTileEffects.Count; i++){
                enemyTeamTileEffects[i].tileEffectPrefab.GetComponent<tileEffectActions>().performStartOfTurnEffect(enemyTeamTileEffects[i].tile);
            }
        }
    }

    public void playerTeamEndOfTurnTileEffectsAdvance(){
        if (playerTeamTileEffects != null){
            for (int i = 0; i < playerTeamTileEffects.Count; i++){
                playerTeamTileEffects[i].tileEffectPrefab.GetComponent<tileEffectActions>().performEndOfTurnEffect(playerTeamTileEffects[i].tile);
            }
        }
    }

    public void playerTeamStartOfTurnTileEffectsAdvance(){
        if (playerTeamTileEffects != null){
            for (int i = 0; i < playerTeamTileEffects.Count; i++){
                playerTeamTileEffects[i].tileEffectPrefab.GetComponent<tileEffectActions>().performStartOfTurnEffect(playerTeamTileEffects[i].tile);
            }
        }
    }
    */


}
