using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Character_Class : MonoBehaviour
{
    GameObject target = null;
    Basic_Character_Class basic = null;
    public string element = "";
    public bool chaseFromFar = false;
    public int chaseSteps = 10;

    public int[] waterCooldown = new int[2]; 

    public List<Basic_Spell_Class> spellList; //The list of spells that the character can cast

    void Start() {
        basic = this.gameObject.GetComponent<Basic_Character_Class>();
        waterCooldown[0] = 0;
        waterCooldown[1] = 0;
    }

    //Tells the enemy to take their turn ---SUBJECT TO CHANGES AS AI IS ADDED---
    public void takeTurn()
    {
        UnityEngine.Debug.Log("Taking turn");
        lowerWaterCooldown();

        if (element == "") {
            basicTurn();
        }
        else {
            magicTurns();
        }
    }

    public void magicTurns() {
        if (element == "Water") {
            waterEnemyTurn();
        }
    }

    public void basicTurn() {
        UnityEngine.Debug.Log("BASIC TURN");
        if (basic.health > 5) {
            takePath(findHero());
        }
        else {
            takePath(findCover(run: true));
        }
    }

    public void takePath(List<Node> path) {
        if (path.Count > 1) {
            //LOG STUFF HERE FOR MOVEMENT
            basic.comlog.addText("-> " + basic.name + " has Moved");
            if (path[0].x == basic.tileX && path[0].y == basic.tileY) {
                basic.endTurn();
            }
        }
        else {
            //add stuff here about leaving if standing on fire
            if (basic.map.checkForTileEffect(basic.tileX, basic.tileY, "Burning")) {
                if (target != null) {
                    int tileX = target.GetComponent<Basic_Character_Class>().tileX;
                    int tileY = target.GetComponent<Basic_Character_Class>().tileY;
                    int[] adjCoords = new int[] {tileX-1,tileY,tileX+1,tileY,tileX,tileY+1,tileX,tileY-1};

                    for (int i = 0; i < 8; i = i+2) {
                        if (!basic.map.checkForTileEffect(adjCoords[i], adjCoords[i+1], "Burning")) {
                            path = basic.map.generatePathTo(adjCoords[i], adjCoords[i+1], false, false, setCurrent:false);
                        }
                    }
                }
            }

            basic.endTurn();
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
        float closestHeroCost = 20;
        bool ignoreSummon = false;

        foreach (GameObject hero in heroes) {
            int tileX = hero.GetComponent<Basic_Character_Class>().tileX;
            int tileY = hero.GetComponent<Basic_Character_Class>().tileY;
            //UnityEngine.Debug.Log("Hero: " + tileX + "," + tileY);
            List<Node> path = basic.map.generatePathTo(tileX, tileY, false, true, setCurrent:false, cutPath:false);
            //UnityEngine.Debug.Log("step count: " + path.Count + " to hero " + hero.name);
            if (path != null && hero.GetComponent<Hero_Character_Class>() != null) {
                path.RemoveAt(path.Count - 1);
                float mCost = basic.map.pathMovementCost(path);
                if (mCost <= basic.movementSpeed.moddedValue && mCost < closestHeroCost) {
                    target = hero;
                    minSteps = path.Count;
                    pathToTarget = path;
                    ignoreSummon = true;
                    //break;
                }
            }
            if ((path != null) && (path.Count < minSteps) && !ignoreSummon) {
                minSteps = path.Count;
                target = hero;
                pathToTarget = basic.map.cutDownPath(basic.movementSpeed.moddedValue, false, path);
                //UnityEngine.Debug.Log("step count: " + path.Count + " to hero " + hero.name);
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

    public List<Node> findCover(bool run = false, bool runIfAdjToHero = true) {
        GameObject[] heroes = basic.map.heroes.ToArray(); 
        //get list of spaces adjacent to walls
        int[,] tiles = basic.map.tiles;
        Node[,] graph = basic.map.graph;
        int[] wallNums = basic.map.wallNums;

        List<Node> wallAdj = new List<Node>();

        //check for cover within 10x10 area?
        int startX = Math.Max(basic.tileX-5, 0);
        int endX = Math.Min(basic.tileX+5, tiles.GetLength(0));
        int startY = Math.Max(basic.tileY-5, 0);
        int endY = Math.Min(basic.tileY+5, tiles.GetLength(1));

        //iterate through map tiles
        for (int i = startX; i < endX; i++) { 
            for (int j = startY; j < endY; j++) { 
                //if tile is wall, add adjacent non-walls to wallAdj
                if (Array.IndexOf(wallNums, tiles[i,j]) != -1) {
                    foreach (Node n in graph[i,j].neighbors) {
                        if (!(wallAdj.Contains(n)) && (Array.IndexOf(wallNums, tiles[n.x, n.y]) == -1) && !(basic.map.checkForTileEffect(n.x, n.y, "Burning")))
                            wallAdj.Add(n);
                    }
                } 
            } 
        }

        List<Node> coverTiles = new List<Node>();
        List<Node> shortestCoverTilePath = new List<Node>();
        //refine list to walls that put distance between enemy and hero
        bool first = true;
        foreach (Node n in wallAdj) {
            int coveredFrom = 0;
            List<Node> coverTilePath = new List<Node>();
            foreach (GameObject hero in heroes) {
                int heroX = hero.GetComponent<Basic_Character_Class>().tileX;
                int heroY = hero.GetComponent<Basic_Character_Class>().tileY;
                List<Node> pathWithWall = basic.map.generatePathTo(heroX, heroY, false, true, n.x, n.y, false, setCurrent:false, cutPath: false);
                List<Node> pathNoWall = basic.map.generatePathTo(heroX, heroY, false, true, n.x, n.y, true, setCurrent:false, cutPath: false);
                if (pathNoWall != null && pathWithWall != null) {
                    if (pathWithWall.Count > pathNoWall.Count || pathWithWall.Count > 8) {
                        coveredFrom += 1;
                    }
                }
            }
            if (coveredFrom == heroes.Length) {
                coverTiles.Add(n);

                if (n.x == basic.tileX && n.y == basic.tileY) {
                    coverTilePath = new List<Node>();
                    shortestCoverTilePath = coverTilePath;
                    break;
                    //UnityEngine.Debug.Log("ALREADY ON COVER TILE: " + n.x + "," + n.y);
                }
                else {
                    coverTilePath = basic.map.generatePathTo(n.x, n.y, false, false, setCurrent:false);
                }

                if (first && coverTilePath != null) {
                    shortestCoverTilePath = coverTilePath;
                    first = false;
                }
                else if (coverTilePath != null && coverTilePath.Count < shortestCoverTilePath.Count) {
                    shortestCoverTilePath = coverTilePath;
                    //UnityEngine.Debug.Log("CLOSEST: " + n.x + "," + n.y + "; to go: " + coverTilePath.Count);
                }
                //UnityEngine.Debug.Log("FOUND COVER TILE: " + n.x + "," + n.y + "; to go: " + coverTilePath.Count);
            }
        }
        //UnityEngine.Debug.Log("VALID COVER TILES: " + coverTiles.Count);
        //UnityEngine.Debug.Log("CLOSE COVER TILE: " + shortestCoverTilePath.Count);

        //reject paths that land directly next to hero

        //if no valid cover tiles run (bool for run maybe?)
        if (coverTiles.Count == 0) {
            if (run) {
                shortestCoverTilePath = runPath();
            }
            else if (runIfAdjToHero) {
                basic.beginTargeting(basic.attackReach);
                if (basic.map.targetList.Count > 0) {
                    shortestCoverTilePath = runPath();
                }
                basic.stopTargeting();
            }
        }

        return shortestCoverTilePath;
    }

    //get closer to nearest ally - don't go closer than range
    public List<Node> nearAlly(int range) {
        return new List<Node>();
    }

    //run as far from hero units as possible
    public List<Node> runPath() {
        int[] mapsize = basic.map.getMapSize();
        int mapX = mapsize[0];
        int mapY = mapsize[1];

        int[] randX = new int[5];
        int[] randY = new int[5];

        int bestTile = 0;
        float score = 0;
        GameObject[] heroes = basic.map.heroes.ToArray();
        List<Node> pathToTarget = new List<Node>();

        for (int i = 0; i < 5; i++) {
            randX[i] = Random.Range(basic.tileX-4, basic.tileX+5);
            randY[i] = Random.Range(basic.tileY-4, basic.tileY+5);

            if (randX[i] >= 0 && randY[i] >=0 && randX[i] < mapX && randY[i] < mapY){
                if (basic.map.clickableTiles[randX[i],randY[i]] != null && basic.map.clickableTiles[randX[i],randY[i]].isWalkable && !basic.map.checkForTileEffect(randX[i],randY[i], "Burning")) {

                    float tileScore = 0;
                    foreach (GameObject hero in heroes) {
                        int tileX = hero.GetComponent<Basic_Character_Class>().tileX;
                        int tileY = hero.GetComponent<Basic_Character_Class>().tileY;
                        List<Node> path = basic.map.generatePathTo(randX[i], randY[i], false, true, tileX, tileY, setCurrent:false, cutPath:false);
                        if (path != null && path.Count < 10) {
                            path.RemoveAt(path.Count - 1);
                            tileScore += basic.map.pathMovementCost(path);
                        }
                    }
                    if (tileScore > score) {
                        bestTile = i;
                        score = tileScore;
                    }
                }
            }
        }
        pathToTarget = basic.map.generatePathTo(randX[bestTile], randY[bestTile], false, false, setCurrent:false);

        return pathToTarget;
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
            basicAttack();
        }
        else {
            magicTurns();
        }
    }

    void basicAttack() {
        basic.beginTargeting(basic.attackReach);
        if (target != null && basic.withinReach(target)) {
            UnityEngine.Debug.Log("Target within reach");
            basic.attackCharacter(target, basic.attack.moddedValue);
        }
        else {
            basic.stopTargeting();
        }
        basic.endTurn();
    }

    //Water enemy functions

    void lowerWaterCooldown() {
        waterCooldown[0] -= 1;
        waterCooldown[1] -= 1;
    }

    //make water enemy go last (reorder in scene)
    public void waterEnemyTurn() {

        //if on burning tile
        if (basic.map.checkForTileEffect(basic.tileX, basic.tileY, "Burning")) {
            //check for adjacent hero
            basic.beginTargeting(basic.attackReach);
            if (basic.map.targetList.Count > 0) {
                basic.stopTargeting();
                //cast flood
                basic.beginTargetingSpell(spellList[0].range, spellList[0]);
                Basic_Spell_Class spellInstance = Instantiate(spellList[0]);
                spellInstance.spellPrefab.GetComponent<Cast_Spell>().castSpell(basic.map.targetList, this.gameObject);
                UnityEngine.Debug.Log("ENEMY CAST FLOOD");
                basic.comlog.addText("-> " + basic.name + " has Cast Flood");
                waterCooldown[0] = 3;
                basic.stopTargeting();
                basic.endTurn();
            }
            else {
                basic.stopTargeting();
                if (basic.hasWalked == false) {
                    takePath(findCover());
                }
            }

        }
        else if (waterCooldown[1] <= 0){
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
                        if (x < mapX - 1 && y < mapY - 1 && basic.map.clickableTiles[x,y] != null) {
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
                basic.comlog.addText("-> " + basic.name + " has Cast Rain");
                waterCooldown[1] = 3;
                basic.stopTargeting();
                basic.endTurn();
            }
            else {
                //basic.stopTargeting();
                if (basic.hasWalked == false) {
                    takePath(findCover(runIfAdjToHero: true));
                }
                else
                {
                    basicAttack();
                }
            }
        }
        else {
            if (basic.hasWalked == false) {
                //UnityEngine.Debug.Log("HERE?????");
                takePath(findCover(runIfAdjToHero: true));
            }
            else
            {
                basicAttack();
            }
        }

        //if not go towards cover, then check again (first fire in range, then Lancin)
    }

}
