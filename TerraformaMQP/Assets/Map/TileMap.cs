using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{

    public GameObject selectedUnit; //The currently selectedUnit, is null if there is no selected unit
    public Basic_Character_Class selectedUnitScript; //The Basic Class script attached to the selectedUnit, is null if there is no selected unit
    public Tilemap tilemap; //Reference to the tile map
    public List<GameObject> heroes = new List<GameObject>(); //list of player controlled units

    public TileType[] tileTypes; //Array that contains a list of all the tiletypes
    public int[,] tiles; //2D array of integers, the integer values correspond with a TileType in the tileTypes array. Ex. a 0 in the tiles array corresponds with the tileGrass TileType in the tileTypes array
    Node[,] graph; //2D array of Nodes that represent the map
    public ClickableTile[,] clickableTiles; //2D array that contains the instances of tiles contained within the map

    public bool movingEnemy = false; //Variable that checks if the map is currently moving an enemy, true if so
    public bool moving = false; //Variable that checks if the map is currently moving a unit, true if so

    public List<ClickableTile> targetList = null; //Used when targeting, contains all of the possible valid targets within range of the currently being targeted spell/attack, otherwise is null

    public bool mapCreated = false; //If the map is fully created or not, used when instantiating the map upon level load to ensure the map is created before the units

    int mapSizeX = 10; //The maximum X dimension
    int mapSizeY = 10; //The maximum Y dimension

    float xOffset; //An offset that can be set to account for maps not starting at location 0,0
    float yOffset; //^

    //A Dictionary the contains the tiles and their corresponding integer value used to find their type in the tileTypes array
    Dictionary<string, int> tileNames = new Dictionary<string, int>(){
        {"tileGrass", 0},
        {"tileDirt", 1},
        {"tileMud", 2},
        {"tileIce", 3},
        {"tileStone", 4},
        {"tileWoodPlank", 5},
        {"tileDenseForest", 6},
        {"tileLightForest", 7},
        {"tileShallowWater", 8},
        {"tileDeepWater", 9},
        {"tileSand", 10},
        {"tileGlass", 11},
        {"tileMetal", 12},
        {"tileAshen", 13},
        {"tileMountain", 14},
        {"tileHill", 15},
        {"tileWall", 16},
        {"tileWhiteVoid", 17}
    };

    //Nodes along the path of shortest path
    public List<Node> currentPath = null; //A List of Nodes that is the current path the selected unit is moving along, null if no unit is moving
    public List<Node> visualPath = null; //A List of Nodes that is the path to the currently hovered tile, null if the unit isn't trying to move or if no unit is selected
    public GameObject circleArrowPrefab; //Reference to the arrowPrefab used to display the path

    int remainingSteps = 0; //steps remaining before reaching movement limit

    //Upon level load begin creating the map
    void Start() {
        createMap();
        //GenerateMapVisual();
        findHeroes();
    }

    //Called to create list of player controlled units
    public void findHeroes() {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        //UnityEngine.Debug.Log("Total objects: " + allObjects.Length);
        string[] heroNames = {"Wold", "Lancin", "BasicCharacter"};
        foreach (GameObject obj in allObjects) {
            //UnityEngine.Debug.Log(obj.name.GetType());
            if (Array.IndexOf(heroNames, obj.name) >= 0) {
                heroes.Add(obj);
            }
        }

    }

    //Called when setting up the map
    public void createMap(){
        UnityEngine.Debug.Log("Map Created");
        //Generates the map, then fills in the Graph with the created map, sets the mapCreated variable to true to allow the units to be created
        GenerateMapData();
        GenerateGraph();
        mapCreated = true;
    }

    void Update() {
        float speed = 2;
        float step = speed * Time.deltaTime;

        //Checks if the selected unit is targeting an attack/spell
        if (selectedUnit != null && selectedUnitScript.targeting == true)
        {
            //Hides the path
            hidePath();
        }
        if (remainingSteps == 0) {
            currentPath = null;
        }

        //Checks if the path isn't null, then checks if the path still has nodes left to traverse
        if (currentPath != null){
            if (currentPath.Count > 0)
            {
                //Ensures that a unit is actually selected
                if (selectedUnit != null)
                {
                    //Checks if the unit is an enemy, if so set's the moving enemy variable to true
                    if (selectedUnit.GetComponent<Enemy_Character_Class>())
                    {
                        movingEnemy = true;
                    }
                    moving = true;
                    int x = currentPath[0].x;
                    int y = currentPath[0].y;
                    Vector3 nextPos = TileCoordToWorldCoord(x+(int)xOffset, y+(int)yOffset);
                    //Checks if the unit's position is equal to the position it is trying to move to
                    if (nextPos != selectedUnit.transform.position)
                    {
                        //If not, the unit moves closer to the desired location
                        selectedUnit.transform.position = Vector3.MoveTowards(selectedUnit.transform.position, nextPos, step);
                    }
                    //If the unit is at the desired location, the unit's tileX and tileY variables are updated
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

                        addTileEffect(x, y, selectedUnit);
                        //Removes the node the unit just travelled to from the path
                        currentPath.RemoveAt(0);
                    }
                    remainingSteps -= 1;
                }
            }
            //If the current path has no nodes left, then the path has been fully traversed 
            else
            {
                //The moving variables are set to false and the currentPath becomes null
                movingEnemy = false;
                moving = false;
                currentPath = null;
            }
        }     

    }

    public int getRemainingSteps() {
        return remainingSteps;
    }
    public void setRemainingSteps(int steps) {
        remainingSteps = steps;
    }


    //Used to manually generate a tile map by setting the tileTypes using a 2D array
    void GenerateMapDataManual() {
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

    void GenerateMapData() {
        UnityEngine.Debug.Log(tilemap.GetUsedTilesCount());

        Transform[] allObj = tilemap.GetComponentsInChildren<Transform>();

        float[] bounds = new float[4];
        bounds[0] = allObj[0].position.x;
        bounds[1] = allObj[0].position.z;
        bounds[2] = allObj[0].position.x;
        bounds[3] = allObj[0].position.z;

        UnityEngine.Debug.Log(bounds[3]);
        foreach (Transform tile in allObj) {
            if (tileNames.ContainsKey(tile.name)) {
                if (tile.position.x < bounds[0]) {
                    bounds[0] = tile.position.x;
                }
                if (tile.position.z < bounds[1]) {
                    bounds[1] = tile.position.z;
                }
                if (tile.position.x > bounds[2]) {
                    bounds[2] = tile.position.x;
                }
                if (tile.position.z > bounds[3]) {
                    bounds[3] = tile.position.z;
                }
            }
        }
        UnityEngine.Debug.Log(bounds[0] + "," + bounds[1] + "," + bounds[2] + "," + bounds[3]);

        xOffset = bounds[0];
        yOffset = bounds[1];

        mapSizeX = (int) (bounds[2] - bounds[0] + 1);
        mapSizeY = (int) (bounds[3] - bounds[1] + 1);

        //allocate map tiles
        tiles = new int[mapSizeX, mapSizeY];
        clickableTiles = new ClickableTile[mapSizeX, mapSizeY];

        //initialize map tiles as void
        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                tiles[x,y] = 17;
                clickableTiles[x, y] = null;
            }
        }

        foreach (Transform tile in allObj) {
            if (tileNames.ContainsKey(tile.name)) {
                //UnityEngine.Debug.Log((int)(tile.position.x-xOffset) + "," + (int)(tile.position.z-yOffset));
                int x = (int)(tile.position.x-xOffset);
                int y = (int)(tile.position.z-yOffset);
                tiles[x, y] = tileNames[tile.name];

                ClickableTile ct = tile.gameObject.GetComponent<ClickableTile>();
                ct.TileX = x;//(int)tile.position.x;
                ct.TileY = y;//(int)tile.position.z;
                ct.map = this;
                ct.isWalkable = tileTypes[tileNames[tile.name]].isWalkable;
                clickableTiles[x, y] = ct;

                UnityEngine.Debug.Log(clickableTiles[x, y].isWalkable);
            }
        }
        
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
                remainingSteps = selectedUnitScript.movementSpeed.value;
                generatePathTo(x, y);
                UnityEngine.Debug.Log(currentPath.Count);

                //selectedUnitScript.charSelected = false;

                //selectedUnitScript.tileX = currentPath[1].x;
                //selectedUnitScript.tileY = currentPath[1].y;
                //selectedUnit.transform.position = TileCoordToWorldCoord(currentPath[1].x,currentPath[1].y);
            }
        }

    }

    public List<Node> generatePathTo(int x, int y){

        if (selectedUnitScript.tileX == x && selectedUnitScript.tileY == y){
            currentPath = new List<Node>();
            selectedUnitScript.path = currentPath;
            return null;
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
            return null;
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

        return currentPath;
        //showPath();
    }

    public bool unitCanEnterTile(int x, int y) {

        //add section here for checking if space is occupied by other unit
        bool walkable = false;
        if (clickableTiles[x, y] != null) {
            walkable = clickableTiles[x, y].isWalkable;
        }
        return walkable;
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
            ca.transform.position = new Vector3(visualPath[i].x+(int)xOffset,0.6f,visualPath[i].y+(int)yOffset);
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


    //Used to find possible targets and then highlight the tiles   ---WILL BE REWORKED AGAIN---
    //Takes in the reach of the selected attack/spell, it's abiltiy to target tiles, and it's ability to target allies
    /*
    The targeting creates a diamond shape around the character. Ex. a character has a reach of 2
    -----              --0--
    -----              -000-
    --X--     --->     00X00
    -----              -000-
    -----              --0--

    To create this targeting, the reach is used along with a width variable to calculate how large of an area the character should be able to hit
    It then starts moving right, starting from the bottom of the column and moving up
    After finishing the right side, it then repeats for the left
    At this point, all that remains is the the player is located on, so that is filled out, starting by moving up, then down
    Here's how it draws out for a reach of 3:

    -------     -------     -------     -------                                 -------     ---0---     ---0---
    -------     ----0--     ----0--     ----0--                                 --0-0--     --000--     --000--
    -------     ----0--     ----00-     ----00-   (Skipping ahead a few steps)  -00-00-     -00000-     -00000-
    ---X--- ->  ---X0-- ->  ---X00- ->  ---X000        ------------------->     000X000 ->  000X000 ->  000X000
    -------     ----0--     ----00-     ----00-                                 -00-00-     -00-00-     -00000-
    -------     ----0--     ----0--     ----0--                                 --0-0--     --0-0--     --000--
    -------     -------     -------     -------                                 -------     -------     ---0---

    */
    public void drawReach(int reach, bool targetTiles, bool targetAllies)
    {
        int width = 1;
        //Find the maximum width that needs to be checked
        for (int i = 2; i <= reach; ++i)
        {
            width += 2;
        }
        //Find the partial width that'll be used to check half of the width at a time
        int partialWidth = (width - 1) / 2;
        int storeWidth = partialWidth;

        UnityEngine.Debug.Log(width);
        UnityEngine.Debug.Log(partialWidth);
        //This for loop iterates through the columns that need to be checked for targeting, a reach of 3 means it will check through 3 columns, the width shrinking as it moves to a new column
        for (int i = 1; i < reach + 1; i++)
        {
            //This for loop iterates up from the bottom 
            for (int j = partialWidth; j > ((-1 * partialWidth) - 1); --j)
            {
                if (checkIndex(selectedUnitScript.tileX + i, selectedUnitScript.tileY + j) && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j] != null)
                {
                    if (targetTiles == true)
                    {
                        clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j]);
                    }
                    else if (targetAllies == true && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile != null && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject.tag == "PlayerTeam"){
                        clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j]);
                    }
                    else if (clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile != null && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject.tag == "EnemyTeam")
                    {
                        clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j]);
                    }

                }
            }
            partialWidth--;
        }
        partialWidth = storeWidth;
        for (int i = -1; i > -reach - 1; i--)
        {
            for (int j = partialWidth; j > ((-1 * partialWidth) - 1); --j)
            {
                if (checkIndex(selectedUnitScript.tileX + i, selectedUnitScript.tileY + j) && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j] != null)
                {
                    if (targetTiles == true)
                    {
                        clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j]);
                    }
                    else if (targetAllies == true && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile != null && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject.tag == "PlayerTeam"){
                        clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j]);    
                    }
                    else if (clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile != null && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject.tag == "EnemyTeam")
                    {
                        clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j]);
                    }

                }
            }
            partialWidth--;
        }
        for (int i = 0; i <= reach; i++)
        {
            if (checkIndex(selectedUnitScript.tileX, selectedUnitScript.tileY + i) && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i] != null)
            {
                if (targetTiles == true)
                {
                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                    targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i]);
                }
                else if (targetAllies == true && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile.gameObject.tag == "PlayerTeam"){
                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                    targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i]);
                }
                else if (clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile.gameObject.tag == "EnemyTeam")
                {
                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                    targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i]);
                }
            }
        }
        for (int i = 1; i <= reach; i++)
        {
            if (checkIndex(selectedUnitScript.tileX, selectedUnitScript.tileY - i) && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i] != null)
            {
                if (targetTiles == true)
                {
                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                    targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i]);
                }
                else if (targetAllies == true && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile.gameObject.tag == "PlayerTeam"){
                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                    targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i]);
                }
                else if (clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile.gameObject.tag == "EnemyTeam")
                {
                        clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i]);
                }
            }
        }

    }

    //Checks if the x , y input is within the tile map
    //If the  coords are in the map, true is returned, otherwise false is returned
    public bool checkIndex(int x, int y)
    {
        if (x >= 0 && x < clickableTiles.GetLength(0))
        {
            if (y >= 0 && y < clickableTiles.GetLength(1))
            {
                return true;
            }
        }
        return false;
    }

    //Current placeholder to set the tiles back to their original colors
    //Iterates over the list of possible targets and ends their highlight before resetting the targetList to empty
    public void removeReach()
    {

        for (int i = 0; i < targetList.Count; i++)
        {
            targetList[i].endHighlight();
        }
        targetList = new List<ClickableTile>();

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

    //Checks the targetList for the inputted target ---REACH MAY NOT BE NEEDED HERE---
    //Takes in a GameObject that will be the selectedTarget that is searched for
    public bool checkForTarget(GameObject selectedTarget, int reach)
    {
        //Iterates over all entires in the targetList
        for (int i = 0; i < targetList.Count; ++i)
        {
            //Checks if the selectedTarget is the current entry in the targetList or if it is the character on the entry
            if (targetList[i] == selectedTarget || (targetList[i].characterOnTile != null && targetList[i].characterOnTile == selectedTarget))
            {
                //If the target is found return true
                return true;
            }
        }
        //If the target can't be found return false
        return false;
    }


    //Swaps two different tiles on the map
    //Takes in the previous tile and the new tile to switch to, as well as the integer value of the TileType in the tileTypes array
    public void swapTiles (ClickableTile previousTile, ClickableTile newTile, int tileNumber)
    {
        //transfers the values of the previous tile to the new tile
        newTile.characterOnTile = previousTile.characterOnTile;
        newTile.map = this;
        newTile.TileX = previousTile.TileX;
        newTile.TileY = previousTile.TileY;
        //Sets the tile arrays in the map to contain the new tile
        tiles[previousTile.TileX, previousTile.TileY] = tileNumber;
        clickableTiles[previousTile.TileX, previousTile.TileY] = newTile;
        //Checks if there was a character on the previous tile
        if (previousTile.characterOnTile != null)
        {
            //If so, also updates the character on the tile, adding the new effect and linking the character to the new tile
            Basic_Character_Class unitAffected = previousTile.characterOnTile.GetComponent<Basic_Character_Class>();
            unitAffected.tile = newTile;
            unitAffected.removeStatus(unitAffected.tileEffect, true);
            StatusEffect newEffect = new StatusEffect();
            newEffect.initializeTileEffect(newTile.statsToEffect, newTile.name, newTile.effectAmounts, unitAffected.gameObject, newTile.name + "Effect");
            unitAffected.tileType = newTile.map.tileTypes[tileNumber];
        }
        //Destroys the old tile
        Destroy(previousTile.gameObject);
    }

    //Adds a status effect to a unit as they enter a tile, takes in the x, y location of the tile and the unit the effect will be applied to
    public void addTileEffect(int x, int y, GameObject unitToAffect){
        //Creates the status effect and then applies it to the character
        StatusEffect newEffect = new StatusEffect();
        newEffect.initializeTileEffect(tileTypes[tiles[x, y]].tileVisualPrefab.GetComponent<ClickableTile>().statsToEffect, tileTypes[tiles[x, y]].name, tileTypes[tiles[x, y]].tileVisualPrefab.GetComponent<ClickableTile>().effectAmounts, unitToAffect, tileTypes[tiles[x, y]].name + "Effect");
    }
}




