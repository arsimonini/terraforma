using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : ScriptableObject
{
    public int duration; //Duration the effect will last for
    public List<string> statToEffect; //A List of the stats the effect will change
    public string source; //The source of the effect 
    public List<int> amount; //A list of the amounts for each stat that will be changed
    public GameObject selectedObject; //The GameObject that this effect is being applied to
    public string name; //The name of the effect
    public bool playerTeam; //If the effect was created by the player or enemy team, true if the player team, false if the enemy team

    //Constructor type function for a status effect that is not coming from a tile
    /*
    List of parameters needed:
        newDuration          --
        newStatToEffect        |
        newSource              |
        newAmount              |-> Correspond to variables
        newSelectedObject      |
        newName                |
        newPlayerTeam        --
        statusEffectController -> Reference to the statusEffectController
    */
    public void initializeStatusEffect(int newDuration, List<string> newStatToEffect, string newSource, List<int> newAmount, GameObject newSelectedObject, string newName, bool newPlayerTeam, GameObject statusEffectController)
    {
        //Sets the variables
        duration = newDuration;
        statToEffect = newStatToEffect;
        source = newSource;
        amount = newAmount;
        selectedObject = newSelectedObject;
        name = newName;
        playerTeam = newPlayerTeam;
        //Adds the status to the selectedObject
        selectedObject.GetComponent<Basic_Character_Class>().addStatus(this, false);
        //Adds the effect to the StatusEffectController's respective player or enemy team effects list
        if (playerTeam == true)
        {
            statusEffectController.GetComponent<StatusEffectController>().playerTeamEffects.Add(this);
        }
        else
        {
            statusEffectController.GetComponent<StatusEffectController>().enemyTeamEffects.Add(this);
        }

    }

    //Simplier constructor used when creating Status Effects that are linked to a tile
    /*
        List of parameters needed:
        newStatToEffect      --
        newSource              |
        newAmount              |-> Correspond to variables
        newSelectedObject      |
        newName              --
    */
    public void initializeTileEffect(List<string> newStatToEffect, string newSource, List<int> newAmount, GameObject newSelectedObject, string newName)
    {
        //Sets the variables
        statToEffect = newStatToEffect;
        source = newSource;
        amount = newAmount;
        selectedObject = newSelectedObject;
        name = newName;
        //Adds the status to the selectedObject
        selectedObject.GetComponent<Basic_Character_Class>().addStatus(this, true);
    }

    //Reduces the remaining duration of the effect by 1
    //Returns true if the effect still has remaining duration left, false if the effect has run out
    public bool reduceDuration()
    {
            if (duration > 1)
            {
                duration--;
                return true;
            }
            else
            {
                //Removes the effect from the selectedObject
                if (selectedObject != null){
                    selectedObject.GetComponent<Basic_Character_Class>().removeStatus(this, false);
                }
                return false;
            }
    }

}
