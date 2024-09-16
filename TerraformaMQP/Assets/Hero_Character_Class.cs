using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

public class Hero_Character_Class : MonoBehaviour
{
    [SerializeField]
    public int mana;  //Related Functions - useMana, enoughMana, regenMana
    public stat maxMana;
    public stat magic;  //Related Functions - increaseMagic, decreaseMagic
    public List<Basic_Spell_Class> spellList;

    void Start()
    {
        if (this.gameObject.GetComponent<SpellList>() != null)
        {
            spellList = this.gameObject.GetComponent<SpellList>().spellList;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //FOR TESTING PURPOSES ------ REDUCES THE CURRENT MANA COUNT BY 10 WHEN A IS PRESSED AND INCREASES THE CURRENT MANA COUNT BY 10 WHEN S IS PRESSED
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            useMana(10);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            regenMana(10);
        }
        
        
    }

    //Reduces the amount of current mana a character has
    //Input - Amount of mana to use
    //Output - Returns True if the mana was reduced, returns False if not

    bool useMana(int amount){
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

    void increaseMagic(int amount){
        magic.moddedValue += amount;
    }

    //Decreases the character's magic value
    //Input - Amount of magic to decrease by

    void decreaseMagic(int amount){
        magic.moddedValue -= amount;
    }

    public void mouseDown()
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

}
