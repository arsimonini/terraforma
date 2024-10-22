using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class TileEffect : ScriptableObject
{
    public int duration;
    public List<string> statToEffect;
    public List<int> amountToEffect;
    public string source;
    public ClickableTile tile;
    public string name;
    public bool playerTeam;
    public GameObject tileEffectPrefab;
    

    public bool reduceDuration(){
        if (duration > 1){
            duration--;
            return true;
        }
        else{
            tile.removeEffectFromTile(this);
            return false;
        }
    }

    public void createTileEffect(bool newPlayerTeam, ClickableTile newTile, int newDuration = -1, List<string> newStatToEffect = null, List<int> newAmountToEffect = null, string newSource = null, string newName = null){
        if (newDuration != -1){
            duration = newDuration;
        }
        if (newStatToEffect != null){
            statToEffect = newStatToEffect;
        }
        if (newAmountToEffect != null){
            amountToEffect = newAmountToEffect;
        }
        if (newSource != null){
            source = newSource;
        }
        if (newName != null){
            name = newName;
        }
        tile = newTile;
        playerTeam = newPlayerTeam;
        initializeTileEffect();
    }

    public void initializeTileEffect(){
        UnityEngine.Debug.Log("Created Tile Effect");
        if (playerTeam == true){
            UnityEngine.Debug.Log("Added to Player Team tile effects list");
            tile.map.gameObject.GetComponent<StatusEffectController>().playerTeamTileEffects.Add(this);
        }
        else{
            tile.map.gameObject.GetComponent<StatusEffectController>().enemyTeamTileEffects.Add(this);
        }
        tile.addEffectToTile(this);
    }
    



}

public class tileEffectActions : MonoBehaviour
{
    public virtual void performEndOfTurnEffect(ClickableTile tile){
        UnityEngine.Debug.Log("Proccing Tile Effect");
    }

    public virtual void performStartOfTurnEffect(ClickableTile tile){
        UnityEngine.Debug.Log("Performing start of turn effect");
    }

    public virtual void performStepOnEffect(ClickableTile tile){
        UnityEngine.Debug.Log("Performing stepped on effect");
    }
}

