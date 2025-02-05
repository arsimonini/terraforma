using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class TileEffect : ScriptableObject
{
    public int duration; //The duration the effect will last for
    public List<string> statToEffect; //A list of the stats the effect will apply to
    public List<int> amountToEffect; //A list of the amounts the effect will impact
    public string source; //The source that has applied the effect
    public ClickableTile tile; //A reference to the tile the effect is applied to
    public string name; //The name of the effect
    public bool playerTeam; //Whether the effect was applied by the player team or the enemy team. True if the player team, false if the enemy team
    public int movementCostIncrease = 0; //Amount to increase the cost to move onto the tile by. If left at 0 doesn't have any effect
    public GameObject tileEffectPrefab; //Reference to the tileEffectPrefab
    
    //Reduces the duration of the effect by 1
    //Returns true if the effect still has duration, false if the effect has reached a duration of 0 and has ended  ---PROBABLY NEEDS A DESTROY STATEMENT TO CLEAN UP---
    public bool reduceDuration(){
        if (duration > 1){
            duration--;
            return true;
        }
        else{
            duration--;
            tile.removeEffectFromTile(this);
            return false;
        }
    }

    public void removeEffect(){
        Destroy(tileEffectPrefab);
    }

    //Basically a constructor to create a tileEffect
    //Although an asset will already be created, some inputs will want to be given while running, and so some of the parameters can be specified when calling the funtion
    /*
    These include:
        newDuration
        newStatToEffect
        newAmountToEffect
        newSource
        newName
    These change their respective variables, and if left blank will use the default values in the asset
    Two of these parameters must be utilized:
        newPlayerTeam
        newTile
    These are unable to be specified in the asset menu as they must be given at runtime, and so are required
    */
    public void createTileEffect(bool newPlayerTeam, ClickableTile newTile, int newDuration = -1, List<string> newStatToEffect = null, List<int> newAmountToEffect = null, string newSource = null, string newName = null, bool fromReact = false){
        //These if statements check if the user has inputted values for the optional parameters, if not then the variables aren't changed from the base asset
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
        //Sets the required parameters
        tile = newTile;
        playerTeam = newPlayerTeam;
        //Calls the initializeTileEffect function to add the effect to the StatusEffectController
        initializeTileEffect(fromReact: fromReact);
    }

    //Adds this to the StatusController's list of effects
    public void initializeTileEffect(bool fromReact = false){
        UnityEngine.Debug.Log("Created Tile Effect");
        //Checks if the effect should be added to the player team or enemy team effect list
        if (playerTeam == true){
            tile.map.gameObject.GetComponent<StatusEffectController>().playerTeamTileEffects.Add(this);
        }
        else{
            tile.map.gameObject.GetComponent<StatusEffectController>().enemyTeamTileEffects.Add(this);
        }
        //Updates the tile to incorperate the new effect's stat changes
        tile.addEffectToTile(this, fromReact: fromReact);
        GameObject newVisual = Instantiate(tileEffectPrefab);
        tileEffectPrefab = newVisual;
        newVisual.transform.position = new Vector3 (tile.transform.position.x, tile.transform.position.y + 0.52f, tile.transform.position.z);
        int rand = Random.Range(0, 3);
        float rotate = 0f;
        switch (rand){
            case 0:
                rotate = 90f;
                break;
            case 1:
                rotate = 180f;
                break;
            case 2:
                rotate = 270f;
                break;
        }
        newVisual.transform.Rotate(0f, 0f, rotate);
    }
    



}

//Class that contains virtual functions that individual tile effect functions inherit
//If an effect chooses not to override one of the functions, then it means it will do nothing if that case occurs.   Ex. Doesn't override performEndOfTurnEffect -> Has no end of turn effect
public class tileEffectActions : MonoBehaviour
{
    //Virtual function that an effect can override to have an effect at the end of a unit's turn
    public virtual void performEndOfTurnEffect(ClickableTile tile){
        UnityEngine.Debug.Log("Proccing Tile Effect");
    }

    //Virtual function that an effect can override to have an effect at the start of a unit's turn
    public virtual void performStartOfTurnEffect(ClickableTile tile){
        UnityEngine.Debug.Log("Performing start of turn effect");
    }

    //Virtual function that an effect can override to have an effect when a unit steps onto the tile
    public virtual void performStepOnEffect(ClickableTile tile){
        UnityEngine.Debug.Log("Performing stepped on effect");
    }

    //Virtual function that causes the effect to react with any effects already on the tile
    public virtual void react(List<TileEffect> effectsOnTile, ClickableTile tile, TileEffect thisEffect){
        UnityEngine.Debug.Log("Performing Reactions");
    }

    //Virtual function that causes an effect when the duration of an effect runs out
    public virtual void endOfDurationEffect(ClickableTile tile){
        UnityEngine.Debug.Log("Performing end of duration effect");
    }
}

