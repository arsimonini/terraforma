using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Character_Class : MonoBehaviour
{
    GameObject target = null;
    Basic_Character_Class basic = null;

    void Start() {
        basic = this.gameObject.GetComponent<Basic_Character_Class>();
    }

    //Tells the enemy to take their turn ---SUBJECT TO CHANGES AS AI IS ADDED---
    public void takeTurn()
    {
        //Gets random numbers
        System.Random rand = new System.Random();
        UnityEngine.Debug.Log("Taking turn");
        //Moves the unit to a random location by calling the map's MoveSelectedUnitTo function
        //basic.map.MoveSelectedUnitTo(rand.Next(1, 9), rand.Next(1, 9));

        //get list of all heroes in scene
        GameObject[] heroes = basic.map.heroes.ToArray(); 
        int minSteps = 100;
        List<Node> pathToTarget = null;
        target = null;

        foreach (GameObject hero in heroes) {
            int tileX = hero.GetComponent<Basic_Character_Class>().tileX;
            int tileY = hero.GetComponent<Basic_Character_Class>().tileY;
            UnityEngine.Debug.Log("Hero: " + tileX + "," + tileY);
            List<Node> path = basic.map.generatePathTo(tileX, tileY, false, true);
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
        if (heroes.Length > 0) {
            if (target == null) {
                target = heroes[0];
                int tileX = target.GetComponent<Basic_Character_Class>().tileX;
                int tileY = target.GetComponent<Basic_Character_Class>().tileY;
                pathToTarget = basic.map.generatePathTo(tileX, tileY);
                if (pathToTarget != null) {
                    minSteps = pathToTarget.Count;
                }
            }

            basic.map.currentPath = pathToTarget;
            basic.path = pathToTarget;
        }
        
    }

    public void attackTarget() {
        //if hero in range do damage
        basic.beginTargeting(basic.attackReach);
        if (target != null && basic.withinReach(target)) {
            UnityEngine.Debug.Log("Target within reach");
            basic.attackCharacter(target, basic.attack.moddedValue);
        }
        else {
            basic.stopTargeting();
            basic.takeAction();
        }
    }

}
