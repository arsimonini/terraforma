using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Character_Class : MonoBehaviour
{
    //Tells the enemy to take their turn ---SUBJECT TO CHANGES AS AI IS ADDED---
    public void takeTurn()
    {
        //Gets random numbers
        System.Random rand = new System.Random();
        UnityEngine.Debug.Log("Taking turn");
        //Moves the unit to a random location by calling the map's MoveSelectedUnitTo function
        this.gameObject.GetComponent<Basic_Character_Class>().map.MoveSelectedUnitTo(rand.Next(1, 9), rand.Next(1, 9));

    }

}
