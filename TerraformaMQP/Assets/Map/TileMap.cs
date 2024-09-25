using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    public GameObject selectedUnit;
    public Basic_Character_Class selectedUnitScript;

    public TileType[] tileTypes;
    int[,] tiles;
    Node[,] graph;
    ClickableTile[,] clickableTiles;

    public bool movingEnemy = false;

    int mapSizeX = 10;
    int mapSizeY = 10;

    //Nodes along the path of shortest path
    public List<Node> currentPath = null;
    public List<Node> visualPath = null;
    public GameObject circleArrowPrefab;

    //Sets color to tiles

    void Start() {
        GenerateMapData();
        GenerateGraph();
        GenerateMapVisual();
    }

    void Update() {
        float speed = 2;
        float step = speed * Time.deltaTime;

        if (currentPath != null){
            if (currentPath.Count > 0)
            {
                if (selectedUnit.GetComponent<Enemy_Character_Class>())
                {
                    movingEnemy = true;
                }
                int x = currentPath[0].x;
                int y = currentPath[0].y;
                Vector3 nextPos = TileCoordToWorldCoord(x, y);
                if (nextPos != selectedUnit.transform.position)
                {
                    selectedUnit.transform.position = Vector3.MoveTowards(selectedUnit.transform.position, nextPos, step);
                }
                else
                {

                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY].characterOnTile = null;
                    //Makes the tile passable again when the unit moves off it
                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY].isWalkable = true;

                    selectedUnitScript.tileX = x;
                    selectedUnitScript.tileY = y;
                    //Used to apply buff/debuff to the player based on tile type stepped on
                    selectedUnitScript.tileType = tileTypes[tiles[x, y]];
                    selectedUnitScript.tile = clickableTiles[x, y];
                    clickableTiles[x, y].characterOnTile = selectedUnit;

                    //Makes the tile impassable when a character stands on it
                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY].isWalkable = false;
                    
                    StatusEffect newEffect = new StatusEffect();
                    newEffect.initializeTileEffect(tileTypes[tiles[x, y]].tileVisualPrefab.GetComponent<ClickableTile>().statsToEffect, tileTypes[tiles[x, y]].name, tileTypes[tiles[x, y]].tileVisualPrefab.GetComponent<ClickableTile>().effectAmounts, selectedUnit, tileTypes[tiles[x, y]].name + "Effect");
                    currentPath.RemoveAt(0);
                }
            }
            else if (movingEnemy == true)
            {
                movingEnemy = false;
                currentPath = null;
            }
            else
            {
                currentPath = null;
            }
        }

        // FOR TESTING PURPOSES ----- CAN BE UNCOMMENTED TO APPLY A DEBUFF TO THE SELECTED CHARACTER BY PRESSING B OR N, PRESSING M WILL ADVANCE EVERY EFFECT IN THE PLAYER TEAM EFFECTS LIST
        /*
        if (Input.GetKeyDown(KeyCode.B))
        {
            StatusEffect newEffect = new StatusEffect();
            List<string> stats = new List<string>();
            stats.Add("attack");
            stats.Add("speed");
            stats.Add("maxHealth");
            stats.Add("movementSpeed");
            stats.Add("resistence");
            stats.Add("defense");
            stats.Add("criticalChance");
            stats.Add("accuracy");
            stats.Add("totalActions");
            List<int> amounts = new List<int>();
            amounts.Add(-10);
            amounts.Add(-10);
            amounts.Add(-10);
            amounts.Add(-10);
            amounts.Add(-10);
            amounts.Add(-10);
            amounts.Add(-10);
            amounts.Add(-10);
            amounts.Add(-5);
            newEffect.initializeStatusEffect(2, stats, "Cripple", amounts, selectedUnit, "Cripple", true, this.gameObject);
        }
        */
        if (Input.GetKeyDown(KeyCode.N))
        {
            StatusEffect newEffect = new StatusEffect();
            List<string> stats = new List<string>();
            stats.Add("speed");
            List<int> amounts = new List<int>();
            amounts.Add(-5);
            newEffect.initializeStatusEffect(5, stats, "Slow", amounts, selectedUnit, "Slow", true, this.gameObject);
        }
        /*
        if (Input.GetKeyDown(KeyCode.M))
        {
            this.gameObject.GetComponent<StatusEffectController>().playerTeamEffectsAdvance();
        }
        */
        

        

    }


    void GenerateMapData() {
        //allocate map tiles
        tiles = new int[mapSizeX, mapSizeY];

        //initialize map tiles as grass
        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                tiles[x,y] = 0;
            }
        }

        //map for testing
        tiles[4,4] = 2;
        tiles[5,4] = 2;
        tiles[6,2] = 2;
        tiles[7,4] = 2;
        tiles[8,4] = 2;
        tiles[7,5] = 2;
        tiles[8,5] = 2;

        tiles[4,3] = 1;
        tiles[5,3] = 1;
        tiles[3,4] = 1;
        
    }

    void GenerateGraph() {
        graph = new Node[mapSizeX, mapSizeY];

        //initialize graph 
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }

        //add neighbors
        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {

                if (x > 0)
                    graph[x,y].neighbors.Add(graph[x-1, y]);
                if (x < mapSizeX-1)
                    graph[x,y].neighbors.Add(graph[x+1, y]);
                if (y > 0)
                    graph[x,y].neighbors.Add(graph[x, y-1]);
                if (y < mapSizeY-1)
                    graph[x,y].neighbors.Add(graph[x, y+1]);
            }
        }
    }

    void GenerateMapVisual() {

        clickableTiles = new ClickableTile[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                TileType tt = tileTypes[tiles[x,y]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x,0,y), Quaternion.identity);

                ClickableTile ct = go.GetComponent<ClickableTile>();
                ct.TileX = x;
                ct.TileY = y;
                ct.map = this;
                clickableTiles[x, y] = ct;
            }
        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y) {
        return new Vector3(x,1,y);
    }

    public void MoveSelectedUnitTo(int x, int y) {

        //TEST - replace with actual movement implementation
        if (selectedUnit != null && clickableTiles[x, y].isWalkable)
        {
            if (selectedUnitScript.targeting == true)
            {
                selectedUnitScript.targeting = false;
            }
            else if (selectedUnitScript.charSelected || selectedUnit.GetComponent<Enemy_Character_Class>())
            {
                hidePath();
                generatePathTo(x, y);
                UnityEngine.Debug.Log(currentPath.Count);

                //selectedUnitScript.charSelected = false;

                //selectedUnitScript.tileX = currentPath[1].x;
                //selectedUnitScript.tileY = currentPath[1].y;
                //selectedUnit.transform.position = TileCoordToWorldCoord(currentPath[1].x,currentPath[1].y);
            }
        }

    }

    public void generatePathTo(int x, int y){

        if (selectedUnitScript.tileX == x && selectedUnitScript.tileY == y){
            currentPath = new List<Node>();
            selectedUnitScript.path = currentPath;
            return;
        }

        selectedUnitScript.path = null;
        currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        Node source = graph[selectedUnitScript.tileX, selectedUnitScript.tileY];
        Node target = graph[x, y];
        dist[source] = 0;
        prev[source] = null;

        //unchecked nodes
        List<Node> unvisited = new List<Node>();

        foreach (Node n in graph){
            //Initialize to infite distance
            if (n != source){
                dist[n] = Mathf.Infinity;
                prev[n] = null;
            }
            unvisited.Add(n);
        }

        //if there is a node in unvisited list check it
        while (unvisited.Count > 0){
            //unvisited node with shortest distance
            Node u = null;

            foreach (Node possibleU in unvisited){
                if (u == null || dist[possibleU] < dist[u]){
                    u = possibleU;
                }
            }

            if (u == target){
                break;
            }

            unvisited.Remove(u);

            foreach (Node n in u.neighbors){

                float alt = dist[u] + costToEnterTile(n.x, n.y);
                if (alt < dist[n]){
                    dist[n] = alt;
                    prev[n] = u;
                }
            }
        }
        if (prev[target] == null){
            return;
        }
        currentPath = new List<Node>();
        Node curr = target;

        //step through current path and add it to chain
        while (curr != null){
            currentPath.Add(curr);
            curr = prev[curr];
        }
        
        currentPath.Reverse();

        selectedUnitScript.path = currentPath;

        //showPath();
    }

    public bool unitCanEnterTile(int x, int y) {

        //add section here for checking if space is occupied by other unit

        return clickableTiles[x, y].isWalkable;
    }

    // public void selectedChar() {
    //     if(Input.GetMouseButtonDown(0)) {
    //         selectedUnit.setCharSelected(true);
    //     }
    // }

    
    public float costToEnterTile(int x, int y) {

        if (unitCanEnterTile(x, y) == false) {
            return Mathf.Infinity;
        }

        TileType t = tileTypes[tiles[x, y]];
        float dist = t.cost;

        return dist;
    }

    public void showPath() {
        hidePath();
 
        //Create path of CircleArrows
        for (int i = 0; i < visualPath.Count; i++) {
            GameObject ca = Instantiate(circleArrowPrefab);
            ca.transform.position = new Vector3(visualPath[i].x,0.6f,visualPath[i].y);
            ca.transform.localRotation = Quaternion.Euler(90f,0,0);
            if (i != visualPath.Count - 1) {
                ca.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            } else {
                ca.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }

    public void hidePath() {
        //Delete all instances of CircleArrow
        GameObject[] existingArrows = GameObject.FindGameObjectsWithTag("CircleArrow");
        foreach (GameObject arrow in existingArrows) {
            Destroy(arrow);
        }
    }
    public void visualPathTo(int x, int y){

        if (circleArrowPrefab == null) {
            UnityEngine.Debug.LogError("circleArrowPrefab has not been assigned in the Inspector!");
            return;
        }

        if (selectedUnitScript.tileX == x && selectedUnitScript.tileY == y){
            visualPath = new List<Node>();
            selectedUnitScript.path = currentPath;
            return;
        }

        selectedUnitScript.path = null;
        visualPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        Node source = graph[selectedUnitScript.tileX, selectedUnitScript.tileY];
        Node target = graph[x, y];
        dist[source] = 0;
        prev[source] = null;

        //unchecked nodes
        List<Node> unvisited = new List<Node>();

        foreach (Node n in graph){
            //Initialize to infite distance
            if (n != source){
                dist[n] = Mathf.Infinity;
                prev[n] = null;
            }
            unvisited.Add(n);
        }

        //if there is a node in unvisited list check it
        while (unvisited.Count > 0){
            //unvisited node with shortest distance
            Node u = null;

            foreach (Node possibleU in unvisited){
                if (u == null || dist[possibleU] < dist[u]){
                    u = possibleU;
                }
            }

            if (u == target){
                break;
            }

            unvisited.Remove(u);

            foreach (Node n in u.neighbors){

                float alt = dist[u] + costToEnterTile(n.x, n.y);
                if (alt < dist[n]){
                    dist[n] = alt;
                    prev[n] = u;
                }
            }
        }
        if (prev[target] == null){
            return;
        }
        visualPath = new List<Node>();
        Node curr = target;

        //step through current path and add it to chain
        while (curr != null){
            visualPath.Add(curr);
            curr = prev[curr];
        }
        
        visualPath.Reverse();

        selectedUnitScript.path = currentPath;

        showPath();
    }
    //Current placeholder function that searches for nearby characters based on a character's reach (Ex. Reach of 1 will search the tiles immediately next to the character)
    //Needs to be expanded depending on how ranged characters operate

    public void drawReach(int reach)
    {
        //Checks to the right of player
        if (clickableTiles[selectedUnitScript.tileX + reach, selectedUnitScript.tileY].characterOnTile != null && clickableTiles[selectedUnitScript.tileX + reach, selectedUnitScript.tileY].characterOnTile.gameObject.tag == "EnemyTeam")
        {
            clickableTiles[selectedUnitScript.tileX + reach, selectedUnitScript.tileY].highlight();
        }
        //Checks to the left of player
        if (clickableTiles[selectedUnitScript.tileX - reach, selectedUnitScript.tileY].characterOnTile != null && clickableTiles[selectedUnitScript.tileX - reach, selectedUnitScript.tileY].characterOnTile.gameObject.tag == "EnemyTeam")
        {
            clickableTiles[selectedUnitScript.tileX - reach, selectedUnitScript.tileY].highlight();
        }
        //Checks above the player
        if (clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + reach].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + reach].characterOnTile.gameObject.tag == "EnemyTeam")
        {
            clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + reach].highlight();
        }
        //Checks below the player
        if (clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - reach].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - reach].characterOnTile.gameObject.tag == "EnemyTeam")
        {
            clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - reach].highlight();
        }
    }

    //Current placeholder to set the tiles back to their original colors

    public void removeReach(int reach)
    {

        clickableTiles[selectedUnitScript.tileX + reach, selectedUnitScript.tileY].endHighlight();
        clickableTiles[selectedUnitScript.tileX - reach, selectedUnitScript.tileY].endHighlight();
        clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + reach].endHighlight();
        clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - reach].endHighlight();

    }

    public void drawSpellReach(int reach, Basic_Spell_Class spell)
    {
        //Draw spell reach
    }

    public void removeSpellReach(int reach)
    {
        //Remove spell reach
    }

    //Use this function when changing the selectedUnit variable
    //Also sets the selectedUnitScript variable making references to it's script much easier
    
    public void updateSelectedCharacter(GameObject newCharacter)
    {
        if (newCharacter != null)
        {
            selectedUnit = newCharacter;
            selectedUnitScript = selectedUnit.GetComponent<Basic_Character_Class>();
        }
        else
        {
            selectedUnit = null;
            selectedUnitScript = null;
        }
    }

    public bool checkForTarget(GameObject selectedTarget, int reach)
    {
        if (clickableTiles[selectedUnitScript.tileX + reach, selectedUnitScript.tileY].characterOnTile == selectedTarget)
        {
            UnityEngine.Debug.Log("Within Reach");
            return true;
        }
        else if (clickableTiles[selectedUnitScript.tileX - reach, selectedUnitScript.tileY].characterOnTile == selectedTarget)
        {
            return true;
        }
        else if (clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + reach].characterOnTile == selectedTarget)
        {
            return true;
        }
        else if (clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - reach].characterOnTile == selectedTarget)
        {
            return true;
        }
        return false;
    }
}




