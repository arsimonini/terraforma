using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Character_Class : MonoBehaviour
{
    GameObject target = null;
    Basic_Character_Class basic = null;
    public string element = "";
    public bool chaseFromFar = false;
    public int chaseSteps = 10;

    public List<Basic_Spell_Class> spellList; //The list of spells that the character can cast

    void Start() {
        basic = this.gameObject.GetComponent<Basic_Character_Class>();
    }

    //Tells the enemy to take their turn ---SUBJECT TO CHANGES AS AI IS ADDED---
    public void takeTurn()
    {
        UnityEngine.Debug.Log("Taking turn");

        if (element == "") {
            basicTurn();
        }
        else {
            magicTurns();
        }
    }

    public void magicTurns() {
        if (element == "water") {
            waterEnemyTurn();
        }
    }

    public void basicTurn() {
        UnityEngine.Debug.Log("BASIC TURN");
        if (basic.health > 5) {
            takePath(findHero());
        }
        else {
            takePath(findCover());
        }
    }

    public void takePath(List<Node> path) {
        if (path.Count > 0) {
            //LOG STUFF HERE FOR MOVEMENT
        }
        else {
            attackTarget();
        }
        basic.map.currentPath = path;
        basic.path = path;
    }

    public List<Node> findHero() {
        GameObject[] heroes = basic.map.heroes.ToArray();

        //get list of all heroes in scene
        int minSteps = chaseSteps;
        List<Node> pathToTarget = new List<Node>();
        target = null;

        foreach (GameObject hero in heroes) {
            int tileX = hero.GetComponent<Basic_Character_Class>().tileX;
            int tileY = hero.GetComponent<Basic_Character_Class>().tileY;
            UnityEngine.Debug.Log("Hero: " + tileX + "," + tileY);
            List<Node> path = basic.map.generatePathTo(tileX, tileY, false, true, setCurrent:false, cutPath:false);
            //UnityEngine.Debug.Log("path steps: " + path.Count);
            if ((path != null) && (path.Count < minSteps)) {
                minSteps = path.Count;
                target = hero;
                pathToTarget = basic.map.cutDownPath(basic.movementSpeed.moddedValue, false, path);
                //UnityEngine.Debug.Log("step count: " + path.Count);
            }
            
        }
        //UnityEngine.Debug.Log("minsteps: " + minSteps);

        //no target selected - all heroes out of reach + chase == true, target first in list
        if (chaseFromFar == true && heroes.Length > 0) {
            if (target == null) {
                target = heroes[0];
                int tileX = target.GetComponent<Basic_Character_Class>().tileX;
                int tileY = target.GetComponent<Basic_Character_Class>().tileY;
                pathToTarget = basic.map.generatePathTo(tileX, tileY);
                if (pathToTarget != null) {
                    minSteps = pathToTarget.Count;
                }
            }

        }
        return pathToTarget;

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

    //get closer to nearest ally - don't go closer than range
    public List<Node> nearAlly(int range) {
        return new List<Node>();
    }

    //run as far from hero units as possible
    public List<Node> run() {
        return new List<Node>();
    }

    public GameObject heroWithName(string name) {
        GameObject[] heroes = basic.map.heroes.ToArray();
        foreach (GameObject hero in heroes) {
            if (hero.name == name) {
                return hero;
            }
        }
        return null;
    }

    public void attackTarget() {
        //if hero in range do damage
        if (element == "") {
            basic.beginTargeting(basic.attackReach);
            if (target != null && basic.withinReach(target)) {
                UnityEngine.Debug.Log("Target within reach");
                basic.attackCharacter(target, basic.attack.moddedValue);
            }
            else {
                basic.stopTargeting();
            }
            UnityEngine.Debug.Log("END TURN PLEASE");
            basic.endTurn();
        }
        else {
            magicTurns();
        }
    }


    //Water enemy functions

    //make water enemy go last (reorder in scene)
    public void waterEnemyTurn() {
        //if on fire, hero adjacent, and mana high enough, do flood
        //if on fire but not fitting other reqs move

        //if on burning tile
        if (basic.map.checkForTileEffect(basic.tileX, basic.tileY, "Burning")) {
            //check for adjacent hero
            basic.beginTargeting(basic.attackReach);
            if (basic.map.targetList.Count > 0) {
                basic.stopTargeting();
                //cast flood
                basic.beginTargetingSpell(spellList[0].range, spellList[0]);
                //Basic_Spell_Class spellInstance = Instantiate(spellList[0]);
                //spellInstance.spellPrefab.GetComponent<Cast_Spell>().castSpell(basic.map.targetList, this.gameObject);
                UnityEngine.Debug.Log("ENEMY CAST FLOOD");
            }
            else {
                basic.stopTargeting();
                if (basic.hasWalked == false) {
                    takePath(findCover());
                }
            }

        }
        else {
            //begintargeting for rain to get targetlist of tiles
            basic.beginTargetingSpell(spellList[1].range, spellList[1]);
            GameObject bestTarget = null;
            int alliesNearLancin = 0;
            int tilesOnFire = 0;

            GameObject lancin = heroWithName("Lancin Bermane");
            int[] mapsize = basic.map.getMapSize();
            int mapX = mapsize[0];
            int mapY = mapsize[1];

            //keep track of ally distance from lancin
            Dictionary<string, bool> lancinNear = new Dictionary<string, bool>();

            foreach (GameObject tile in basic.map.targetList) {
                int tilex = tile.GetComponent<ClickableTile>().TileX;
                int tiley = tile.GetComponent<ClickableTile>().TileY;
                int boxX = tilex-2;
                int boxY = tiley-2;


                if (boxX < 0) boxX = 0;
                if (boxX > mapX - 1) boxX = mapX - 1;
                if (boxY < 0) boxY = 0;
                if (boxY > mapY - 1) boxY = mapY - 1;

                int fire = 0;
                int ally = 0;
                int allyNearL = 0;

                for (int x = boxX; x < boxX+5; x++) {
                    for (int y = boxY; y < boxY+5;y++) {
                        if (basic.map.clickableTiles[x,y] != null) {
                            GameObject characterOnTile = basic.map.clickableTiles[x,y].characterOnTile;
                            if (basic.map.checkForTileEffect(x, y, "Burning")) {
                                fire++;
                            }
                            if (characterOnTile != null && characterOnTile.GetComponent<Enemy_Character_Class>()) {
                                ally++;

                                //check if ally is close to Lancin
                                if (lancin != null) {
                                    if (lancinNear.ContainsKey(characterOnTile.name)){
                                        if (lancinNear[characterOnTile.name]) {
                                            allyNearL++;
                                        }
                                    }
                                    else {
                                        int tileX = characterOnTile.GetComponent<Basic_Character_Class>().tileX;
                                        int tileY = characterOnTile.GetComponent<Basic_Character_Class>().tileY;
                                        List<Node> path = basic.map.generatePathTo(tileX, tileY, false, true, lancin.GetComponent<Basic_Character_Class>().tileX, lancin.GetComponent<Basic_Character_Class>().tileY, setCurrent:false, cutPath:false);
                                        if (path.Count < 8) {
                                            allyNearL++;
                                        }
                                        lancinNear[characterOnTile.name] = path.Count < 8;
                                    }
                                }
                            }
                        }
                    }
                }

                if (fire > tilesOnFire || allyNearL > alliesNearLancin) {
                    bestTarget = tile;
                    tilesOnFire = fire;
                    alliesNearLancin = allyNearL;
                }
            }
            basic.stopTargeting();
            if (alliesNearLancin != 0 || tilesOnFire > 1) {
                List<GameObject> target = new List<GameObject>();
                target.Add(bestTarget);
                UnityEngine.Debug.Log("ENEMY CAST RAIN");
                Basic_Spell_Class spellInstance = Instantiate(spellList[1]);
                spellInstance.spellPrefab.GetComponent<Cast_Spell>().castSpell(target, this.gameObject);
                basic.stopTargeting();
                basic.endTurn();
            }
            else {
                //basic.stopTargeting();
                if (basic.hasWalked == false) {
                    takePath(findCover());
                }
            }


        }

        //if not go towards cover, then check again (first fire in range, then Lancin)
    }

}
