using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class Hero_Character_Class : MonoBehaviour
{
    [SerializeField]
    public int mana;  //Current amount of mana the character has - Related Functions - useMana, enoughMana, regenMana
    public stat maxMana; //The maximum amount of mana that a character can have at one time
    public stat magic;  //The amount of damage the character deals with magical attacks - Related Functions - increaseMagic, decreaseMagic
    public bool pickingSpell = false; //If the character is currently picking a spell to cast, false if not, true if so
    public List<Basic_Spell_Class> spellList; //The list of spells that the character can cast
    public Basic_Spell_Class selectedSpell = null; //The currently selected spell the character is trying to cast

    void Start()
    {
        //Checks if the unit has a spellList attached to it
        if (this.gameObject.GetComponent<SpellList>() != null)
        {
            //If so, sets the spellList variable to the attached spellList
            spellList = this.gameObject.GetComponent<SpellList>().spellList;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Checks if the player is picking a spell and has left-clicked
        if (pickingSpell && Input.GetMouseButtonDown(0))
        {
            //If so, the player closes the pick spell menu
            pickingSpell = false;
            gameObject.GetComponent<Basic_Character_Class>().renderer.material.color = Color.red;

        }
        //Checks if the player is picking a spell
        else if (pickingSpell)
        {
            //Set's the color to cyan to designate that they are choosing a spell
            gameObject.GetComponent<Basic_Character_Class>().renderer.material.color = Color.cyan;
            //Checks if the player has pressed a button corresponding to a spell
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Begins to target the selected spell
                gameObject.GetComponent<Basic_Character_Class>().beginTargetingSpell(spellList[0].range, spellList[0]);
                //Sets the selectedSpell to the spell from the spellList
                selectedSpell = spellList[0];
                //Stops picking a spell
                pickingSpell = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)){
                gameObject.GetComponent<Basic_Character_Class>().beginTargetingSpell(spellList[1].range, spellList[1]);
                selectedSpell = spellList[1];
                pickingSpell = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)){
                gameObject.GetComponent<Basic_Character_Class>().beginTargetingSpell(spellList[2].range, spellList[2]);
                selectedSpell = spellList[2];
                pickingSpell = false;
            }
        }
        //If picking a spell and the escape key is pressed, the player stops picking a spell
        if (Input.GetKeyDown(KeyCode.Escape) && pickingSpell == true)
        {
            selectedSpell = null;
            pickingSpell = false;
        }
        
    }

    //Reduces the amount of current mana a character has
    //Input - Amount of mana to use
    //Output - Returns True if the mana was reduced, returns False if not
    public bool useMana(int amount){
        if (enoughMana(amount))
        {
           mana = mana - amount;
           UnityEngine.Debug.Log("Used " + amount + " Mana. Current Mana is: " + mana);
           return true;
        }
        return false;
    }

    //Checks if there is enough mana for an ability to be used
    //Output - Returns True is there is, False if there is not

    bool enoughMana(int amount){
        if (mana - amount >= 0)
        {
            return true;
        }
        UnityEngine.Debug.Log("Not Enough Mana");
        return false;
    }

    //Regenerates Mana for the character, but not over the characters maximum amount
    //Input - Amount of mana to regenerate

    void regenMana(int amount){
        mana = mana + amount;
        if (mana > maxMana.moddedValue)
        {
            mana = maxMana.moddedValue;
            UnityEngine.Debug.Log("At Maximum Mana");
            return;
        }
        UnityEngine.Debug.Log("Regenerated " + amount + " Mana. Current Mana is: " + mana);
        return;
    }

    //Increases the character's magic value
    //Input - Amount of magic to increase by

    public void increaseMagic(int amount){
        magic.moddedValue += amount;
    }

    //Decreases the character's magic value
    //Input - Amount of magic to decrease by

    public void decreaseMagic(int amount){
        magic.moddedValue -= amount;
    }

    //Displays the spellList in the Log when the character is clicked ---FOR TESTING PURPOSES, COULD BE DELETED---
    public void OnMouseDown()
    {
        UnityEngine.Debug.Log("here");
        if (spellList != null)
        {
            for (int i = 0; i < spellList.Count; i++)
            {
                UnityEngine.Debug.Log(spellList[i].name);
            }
        }
    }

    //Called when the player begins to select a spell to cast
    public void openSpellBook()
    {
        //Stops showing the map path
        this.gameObject.GetComponent<Basic_Character_Class>().map.hidePath();
        //Sets pickingSpell to true and then iterates over the spell list, logging the spells it contains
        //---SHOULD BE REPLACED WITH CALLS TO UI DISPLAY---
        pickingSpell = true;
        UnityEngine.Debug.Log("Spellbook for " + gameObject.GetComponent<Basic_Character_Class>().name + " is open. Press the corresponding number to begin targeting the spell");
        for(int i = 0; i < spellList.Count; i++)
        {
            UnityEngine.Debug.Log((i + 1) + ": " + spellList[i].name + ", " + spellList[i].description.Replace("{Damage Value}", magic.moddedValue.ToString()));
        }
    }

    //Casts the selected spell
    //Takes in a list of GameObjects as the targets
    public void castSpell(List<GameObject> targets)
    {
        //Instantiates an instance of the selected spell, then calls the castSpell function within it, passing along the targets and then this GameObject as the spells caster
        Basic_Spell_Class spellInstance = Instantiate(selectedSpell);
        spellInstance.spellPrefab.GetComponent<Cast_Spell>().castSpell(targets, this.gameObject);
    }

}
