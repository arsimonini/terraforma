using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

public class Hero_Character_Class : MonoBehaviour
{
    [SerializeField]
    public int mana;  //Related Functions - useMana, enoughMana, regenMana
    public int maxMana;
    public int magic;  //Related Functions - increaseMagic, decreaseMagic

    // Start is called before the first frame update
    void Start()
    {
        
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
        if (mana > maxMana)
        {
            mana = maxMana;
            UnityEngine.Debug.Log("At Maximum Mana");
            return;
        }
        UnityEngine.Debug.Log("Regenerated " + amount + " Mana. Current Mana is: " + mana);
        return;
    }

    //Increases the character's magic value
    //Input - Amount of magic to increase by

    void increaseMagic(int amount){
        magic += amount;
    }

    //Decreases the character's magic value
    //Input - Amount of magic to decrease by

    void decreaseMagic(int amount){
        magic -= amount;
    }

}
