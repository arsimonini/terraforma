using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    public GameObject selectedUnit;

    public TileType[] tileTypes;
    int[,] tiles;
    Node[,] graph;

    int mapSizeX = 10;
    int mapSizeY = 10;

    //Nodes along the path of shortest path
    public List<Node> currentPath = null;

    void Start() {
        GenerateMapData();
        GenerateGraph();
        GenerateMapVisual();
    }

    void Update() {
        float speed = 2;
        float step = speed * Time.deltaTime;

        if (currentPath != null) {
            if (currentPath.Count > 0)
            {
                int x = currentPath[0].x;
                int y = currentPath[0].y;
                Vector3 nextPos = TileCoordToWorldCoord(x, y);
                if (nextPos != selectedUnit.transform.position)
                {
                    selectedUnit.transform.position = Vector3.MoveTowards(selectedUnit.transform.position, nextPos, step);
                }
                else
                {
                    selectedUnit.GetComponent<Unit>().tileX = x;
                    selectedUnit.GetComponent<Unit>().tileY = y;
                    //Used to apply buff/debuff to the player based on tile type stepped on
                    selectedUnit.GetComponent<Unit>().tile = tileTypes[tiles[x, y]];
                    StatusEffect newEffect = new StatusEffect();
                    newEffect.initializeTileEffect(tileTypes[tiles[x, y]].tileVisualPrefab.GetComponent<ClickableTile>().statsToEffect, tileTypes[tiles[x, y]].name, tileTypes[tiles[x, y]].tileVisualPrefab.GetComponent<ClickableTile>().effectAmounts, selectedUnit, tileTypes[tiles[x, y]].name + "Effect");
                    currentPath.RemoveAt(0);
                }
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
        if (Input.GetKeyDown(KeyCode.N))
        {
            StatusEffect newEffect = new StatusEffect();
            List<string> stats = new List<string>();
            stats.Add("speed");
            List<int> amounts = new List<int>();
            amounts.Add(-5);
            newEffect.initializeStatusEffect(5, stats, "Slow", amounts, selectedUnit, "Slow", true, this.gameObject);
        }
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
        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                TileType tt = tileTypes[tiles[x,y]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x,0,y), Quaternion.identity);

                ClickableTile ct = go.GetComponent<ClickableTile>();
                ct.TileX = x;
                ct.TileY = y;
                ct.map = this;
            }
        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y) {
        return new Vector3(x,1,y);
    }

    public void MoveSelectedUnitTo(int x, int y) {

        //TEST - replace with actual movement implementation

        if(selectedUnit.GetComponent<Unit>().charSelected) {

            generatePathTo(x,y);
            Debug.Log(currentPath.Count);

            selectedUnit.GetComponent<Unit>().charSelected = false;

            //selectedUnit.GetComponent<Unit>().tileX = currentPath[1].x;
            //selectedUnit.GetComponent<Unit>().tileY = currentPath[1].y;
            //selectedUnit.transform.position = TileCoordToWorldCoord(currentPath[1].x,currentPath[1].y);
        }

    }

    public void generatePathTo(int x, int y){

        if (selectedUnit.GetComponent<Unit>().tileX == x && selectedUnit.GetComponent<Unit>().tileY == y){
            currentPath = new List<Node>();
            selectedUnit.GetComponent<Unit>().path = currentPath;
            return;
        }

        selectedUnit.GetComponent<Unit>().path = null;
        currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        Node source = graph[selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileY];
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

        selectedUnit.GetComponent<Unit>().path = currentPath;

    }

    public bool unitCanEnterTile(int x, int y) {

        //add section here for checking if space is occupied by other unit

        return tileTypes[tiles[x, y]].isWalkable;
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

}
