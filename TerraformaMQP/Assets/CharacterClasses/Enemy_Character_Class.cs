using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Character_Class : MonoBehaviour
{
    GameObject target = null;

    //Tells the enemy to take their turn ---SUBJECT TO CHANGES AS AI IS ADDED---
    public void takeTurn()
    {
        //Gets random numbers
        System.Random rand = new System.Random();
        UnityEngine.Debug.Log("Taking turn");
        //Moves the unit to a random location by calling the map's MoveSelectedUnitTo function
        this.gameObject.GetComponent<Basic_Character_Class>().map.MoveSelectedUnitTo(rand.Next(1, 9), rand.Next(1, 9));

        //get list of all heroes in scene
        GameObject[] heroes = this.gameObject.GetComponent<Basic_Character_Class>().map.heroes.ToArray(); 
        int minSteps = 100;
        List<Node> pathToTarget = null;
        target = null;

        foreach (GameObject hero in heroes) {
            int tileX = hero.GetComponent<Basic_Character_Class>().tileX;
            int tileY = hero.GetComponent<Basic_Character_Class>().tileY;
            UnityEngine.Debug.Log("Hero: " + tileX + "," + tileY);
            List<Node> path = this.gameObject.GetComponent<Basic_Character_Class>().map.generatePathTo(tileX, tileY, false, true);
            UnityEngine.Debug.Log("path steps: " + path.Count);
            if ((path != null) && (path.Count < minSteps)) {
                minSteps = path.Count;
                target = hero;
                pathToTarget = path;
                UnityEngine.Debug.Log("step count: " + path.Count);
            }
            
        }
        UnityEngine.Debug.Log("minsteps: " + minSteps);

        //no target selected - all heroes very out of reach, target first in list
        if (target == null) {
            target = heroes[0];
            int tileX = target.GetComponent<Basic_Character_Class>().tileX;
            int tileY = target.GetComponent<Basic_Character_Class>().tileY;
            pathToTarget = this.gameObject.GetComponent<Basic_Character_Class>().map.generatePathTo(tileX, tileY);
            if (pathToTarget != null) {
                minSteps = pathToTarget.Count;
            }
        }

        this.gameObject.GetComponent<Basic_Character_Class>().map.currentPath = pathToTarget;
        this.gameObject.GetComponent<Basic_Character_Class>().path = pathToTarget;
        
    }

    public void attackTarget() {
        //if hero in range do damage
        this.gameObject.GetComponent<Basic_Character_Class>().beginTargeting(this.gameObject.GetComponent<Basic_Character_Class>().attackReach);
        UnityEngine.Debug.Log("ATTACK " + target.name + " MY SCARAB " + this.gameObject.GetComponent<Basic_Character_Class>().map.targetList.Count);
        UnityEngine.Debug.Log("ATTACK WITHIN REACH? " + this.gameObject.GetComponent<Basic_Character_Class>().withinReach(target));
        if (target != null && this.gameObject.GetComponent<Basic_Character_Class>().withinReach(target)) {
            UnityEngine.Debug.Log("Target within reach");
            this.gameObject.GetComponent<Basic_Character_Class>().attackCharacter(target, this.gameObject.GetComponent<Basic_Character_Class>().attack.moddedValue);
        }

        this.gameObject.GetComponent<Basic_Character_Class>().stopTargeting();
        this.gameObject.GetComponent<Basic_Character_Class>().takeAction();
    }

}
