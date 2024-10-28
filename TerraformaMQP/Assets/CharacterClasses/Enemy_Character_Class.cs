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


        //get list of all heroes in scene
        GameObject[] heroes = this.gameObject.GetComponent<Basic_Character_Class>().map.heroes.ToArray(); 
        int minSteps = 100;
        GameObject target = null;
        List<Node> pathToTarget = null;

        foreach (GameObject hero in heroes) {
            int tileX = hero.GetComponent<Basic_Character_Class>().tileX;
            int tileY = hero.GetComponent<Basic_Character_Class>().tileY;
            UnityEngine.Debug.Log("Hero: " + tileX + "," + tileY);
            List<Node> path = this.gameObject.GetComponent<Basic_Character_Class>().map.generatePathTo(tileX, tileY);
            if ((path != null) && (path.Count < minSteps)) {
                minSteps = path.Count;
                target = hero;
                pathToTarget = path;
            }
            UnityEngine.Debug.Log("step count: " + path.Count);
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

        //remove last from list (hero's position)
        pathToTarget.RemoveAt(minSteps-1);
        UnityEngine.Debug.Log("final dest: " + pathToTarget[minSteps-2].x + "," + pathToTarget[minSteps-2].y);

        this.gameObject.GetComponent<Basic_Character_Class>().map.setRemainingSteps(this.gameObject.GetComponent<Basic_Character_Class>().movementSpeed.value);
        this.gameObject.GetComponent<Basic_Character_Class>().map.currentPath = pathToTarget;
        this.gameObject.GetComponent<Basic_Character_Class>().path = pathToTarget;
        //this.gameObject.GetComponent<Basic_Character_Class>().map.MoveSelectedUnitTo(pathToTarget[minSteps-2].x, pathToTarget[minSteps-2].y);
    }

}
