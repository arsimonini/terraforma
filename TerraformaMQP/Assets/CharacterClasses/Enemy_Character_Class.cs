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
        UnityEngine.Debug.Log("Taking turn");

        List<Node> enemyPath = new List<Node>();
        
        if (basic.health > 5) {
            enemyPath = findHero();
        }
        else {
            enemyPath = findCover();
        }

        basic.map.currentPath = enemyPath;
        basic.path = enemyPath;
    }

    public List<Node> findHero() {
        GameObject[] heroes = basic.map.heroes.ToArray();

        //get list of all heroes in scene
        int minSteps = 20;
        List<Node> pathToTarget = null;
        target = null;

        foreach (GameObject hero in heroes) {
            int tileX = hero.GetComponent<Basic_Character_Class>().tileX;
            int tileY = hero.GetComponent<Basic_Character_Class>().tileY;
            UnityEngine.Debug.Log("Hero: " + tileX + "," + tileY);
            List<Node> path = basic.map.generatePathTo(tileX, tileY, false, true, setCurrent:false);
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

            //basic.map.currentPath = pathToTarget;
            //basic.path = pathToTarget;
            return pathToTarget;

        }
        return new List<Node>();

    }

    public List<Node> findCover() {
        GameObject[] heroes = basic.map.heroes.ToArray(); 
        //get list of spaces adjacent to walls
        int[,] tiles = basic.map.tiles;
        Node[,] graph = basic.map.graph;
        int[] wallNums = basic.map.wallNums;

        List<Node> wallAdj = new List<Node>();

        //iterate through map tiles
        for (int i = 0; i < tiles.GetLength(0); i++) { 
            for (int j = 0; j < tiles.GetLength(1); j++) { 
                //if tile is wall, add adjacent non-walls to wallAdj
                if (Array.IndexOf(wallNums, tiles[i,j]) != -1) {
                    foreach (Node n in graph[i,j].neighbors) {
                        if (!(wallAdj.Contains(n)) && (Array.IndexOf(wallNums, tiles[n.x, n.y]) == -1))
                            wallAdj.Add(n);
                    }
                } 
            } 
        }  

        List<Node> coverTiles = new List<Node>();
        List<Node> shortestCoverTilePath = new List<Node>();
        //refine list to walls that put distance between enemy and hero
        foreach (Node n in wallAdj) {
            int coveredFrom = 0;
            bool found = false;
            List<Node> coverTilePath = new List<Node>();
            foreach (GameObject hero in heroes) {
                int heroX = hero.GetComponent<Basic_Character_Class>().tileX;
                int heroY = hero.GetComponent<Basic_Character_Class>().tileY;
                List<Node> pathWithWall = basic.map.generatePathTo(heroX, heroX, false, true, n.x, n.y, false, setCurrent:false);
                List<Node> pathNoWall = basic.map.generatePathTo(heroX, heroX, false, true, n.x, n.y, true, setCurrent:false);
                if (pathNoWall != null && pathWithWall != null) {
                    if (pathWithWall.Count > pathNoWall.Count || pathWithWall.Count > 15) {
                        coveredFrom += 1;
                    }
                }
            }
            if (coveredFrom == heroes.Length) {
                coverTiles.Add(n);
                coverTilePath = basic.map.generatePathTo(n.x, n.y, false, false, setCurrent:false);

                if (!found) {
                    shortestCoverTilePath = coverTilePath;
                    found = true;
                }

                if (coverTilePath != null && coverTilePath.Count < shortestCoverTilePath.Count) {
                    shortestCoverTilePath = coverTilePath;
                }
                UnityEngine.Debug.Log("FOUND COVER TILE: " + n.x + "," + n.y);
            }
        }
        UnityEngine.Debug.Log("VALID COVER TILES: " + coverTiles.Count);

        //reject paths that land directly next to hero

        //if no valid cover tiles find hero instead (SWAP THIS OUT FOR RUN)
        if (coverTiles.Count == 0) {
            shortestCoverTilePath = findHero();
        }

        return shortestCoverTilePath;
    }

    //run as far from hero units as possible
    public void run() {

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
