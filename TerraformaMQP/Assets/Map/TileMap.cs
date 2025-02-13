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
    public Node[,] graph; //2D array of Nodes that represent the map
    public ClickableTile[,] clickableTiles; //2D array that contains the instances of tiles contained within the map

    public bool movingEnemy = false; //Variable that checks if the map is currently moving an enemy, true if so
    public bool moving = false; //Variable that checks if the map is currently moving a unit, true if so

    public List<GameObject> targetList = null; //Used when targeting, contains all of the possible valid targets within range of the currently being targeted spell/attack, otherwise is null

    public bool mapCreated = false; //If the map is fully created or not, used when instantiating the map upon level load to ensure the map is created before the units

    public int mapSizeX = 10; //The maximum X dimension
    public int mapSizeY = 10; //The maximum Y dimension

    public List<GameObject> aoeDisplayTiles = null;
    public bool displayingAOE = false;
    public List<GameObject> allTargets = null;

    public List<GameObject> movementDisplayLimit = null;
    private bool displayMovementDisplay = false;

    public bool heroAvoidFire = false;

    //For Move Button
    public bool moveButtonPressed = false;
    public bool activelyMoving = false;

    public List<GameObject> movementDisplayTiles = null;

    float xOffset; //An offset that can be set to account for maps not starting at location 0,0
    float yOffset; //^

    LayerMask mask; //A mask that is used to find objects that block visibility

    public CombatLog comlog;

    [SerializeField] private AudioClip[] movementSounds;

    //A Dictionary the contains the tiles and their corresponding integer value used to find their type in the tileTypes array
    Dictionary<string, int> tileNames = new Dictionary<string, int>(){
        {"tileGrass", 0},
        {"tileDirt", 1},
        {"tileMud", 2},
        {"tileLandIce", 3},
        {"tileWaterIce", 4},
        {"tileStone", 5},
        {"tileWoodPlank", 6},
        {"tileDenseForest", 7},
        {"tileLightForest", 8},
        {"tileShallowWater", 9},
        {"tileDeepWater", 10},
        {"tileSand", 11},
        {"tileGlass", 12},
        {"tileMetal", 13},
        {"tileAshen", 14},
        {"tileMountain", 15},
        {"tileHill", 16},
        {"tileWall", 17},
        {"tileWhiteVoid", 18},
        {"tileWoldWall", 20}
    };

    public int[] wallNums = {17,20};

    public string[] coverNames = {"tileWall", "tileWoldWall"};

    //Nodes along the path of shortest path
    public List<Node> currentPath = null; //A List of Nodes that is the current path the selected unit is moving along, null if no unit is moving
    public List<Node> visualPath = null; //A List of Nodes that is the path to the currently hovered tile, null if the unit isn't trying to move or if no unit is selected
    public GameObject circleArrowPrefab; //Reference to the arrowPrefab used to display the path

    public int phase = 0; //Checks to see whether it is the player's turn or not. ONLY make this match what GameController's saying about the phase

    //Upon level load begin creating the map
    void Start() {
        Time.timeScale = 1f;
        createMap();
        //GenerateMapVisual();
        //swapTiles(clickableTiles[2,3],20,true);
        mask = LayerMask.GetMask("BlockVisibility");
        findHeroes();
    }

    //Called to create list of player controlled units + set coords for characters
    public void findHeroes() {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        //UnityEngine.Debug.Log("Total objects: " + allObjects.Length);
        foreach (GameObject obj in allObjects) {
            //set tilemap coords
            if (obj.GetComponent<Basic_Character_Class>() != null) {
                //UnityEngine.Debug.Log("TEST: " + obj.name + " " + Math.Floor(obj.transform.position.x - xOffset) + "," + Math.Floor(obj.transform.position.z - yOffset));
                obj.GetComponent<Basic_Character_Class>().tileX = (int) Math.Floor(obj.transform.position.x - xOffset);
                obj.GetComponent<Basic_Character_Class>().tileY = (int) Math.Floor(obj.transform.position.z - yOffset);
            }
            if (obj.GetComponent<Basic_Character_Class>() != null && obj.GetComponent<Enemy_Character_Class>() == null) {
                heroes.Add(obj);
                //UnityEngine.Debug.Log("HERO FOUND: " + obj.name);
            }
        }
    }

    public GameObject findCharacterWithName(string name) {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {
            if (obj.GetComponent<Basic_Character_Class>() != null && obj.name == name) {
                return obj;
            }
        }
        return null;
    }

    //Called when setting up the map
    public void createMap(){
        //UnityEngine.Debug.Log("Map Created");
        //Generates the map, then fills in the Graph with the created map, sets the mapCreated variable to true to allow the units to be created
        GenerateMapData();
        GenerateGraph();
        mapCreated = true;
    }

    void Update() {

        //-----FOR TESTING ONLY-----
        if (Input.GetKeyDown(KeyCode.L) && displayMovementDisplay == true){
            displayMovementDisplay = false;
        }
        else if (Input.GetKeyDown(KeyCode.L)){
            displayMovementDisplay = true;
        }

        float speed = 2;
        float step = speed * Time.deltaTime;

        //Checks if the selected unit is targeting an attack/spell
        if (selectedUnit != null && selectedUnitScript.targeting == true)
        {
            //Hides the path
            hidePath();
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
                        selectedUnitScript.OnMouseExit();
                    }
                    //If the unit is at the desired location, the unit's tileX and tileY variables are updated
                    else
                    {

                    
                        SFXController.instance.PlayRandomSFXClip(movementSounds, transform, 1f);
                

                        selectedUnitScript.tile.OnMouseExit();
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

                        if (clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY].effectsOnTile.Count > 0){
                            for(int i = 0; i < clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY].effectsOnTile.Count; i++){
                                clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY].effectsOnTile[i].tileEffectPrefab.GetComponent<tileEffectActions>().performStepOnEffect(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY]);
                                UnityEngine.Debug.Log(selectedUnit);
                            }
                        }
                        selectedUnitScript.updateCharStats();
                    }
                }
                else {
                    if(moving == true) {
                        SFXController.instance.PlayRandomSFXClip(movementSounds, transform, 1f);
                    }
                    if(selectedUnitScript != null){
                        selectedUnitScript.isMoving = false;
                    }
                    movingEnemy = false;
                    moving = false;
                    currentPath = null;
                }
            }
            //If the current path has no nodes left, then the path has been fully traversed 
            else
            {
                //The moving variables are set to false and the currentPath becomes null
                if (movingEnemy == true && selectedUnitScript != null) {
                    selectedUnit.GetComponent<Enemy_Character_Class>().attackTarget();
                }
                if(selectedUnitScript != null){
                    selectedUnitScript.isMoving = false;
                }
                if(moving == true) {
                    SFXController.instance.PlayRandomSFXClip(movementSounds, transform, 1f);
                }
                movingEnemy = false;
                moving = false;
                currentPath = null;
            }
        }     

    }

    public int[] getMapSize() {
        int[] size = new int[2];
        size[0] = mapSizeX;
        size[1] = mapSizeY;
        return size;
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

    //checks if name of object is a valid tile, returns tile arr number
    int checkTileName(string name) {
        foreach(var nameKey in tileNames.Keys){
            if (name.StartsWith(nameKey)) {
                return tileNames[nameKey];
            }
        }
        return -1;
    }

    public float[] getMapOffsets() {
        float[] offsets = new float[] {xOffset, yOffset};
        return offsets;
    }

    void GenerateMapData() {
        //UnityEngine.Debug.Log(tilemap.GetUsedTilesCount());

        Transform[] allObj = tilemap.GetComponentsInChildren<Transform>();

        float[] bounds = new float[4];
        bounds[0] = allObj[0].position.x;
        bounds[1] = allObj[0].position.z;
        bounds[2] = allObj[0].position.x;
        bounds[3] = allObj[0].position.z;

        //UnityEngine.Debug.Log(bounds[3]);
        foreach (Transform tile in allObj) {
            if (checkTileName(tile.name) != -1) {
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
        //UnityEngine.Debug.Log(bounds[0] + "," + bounds[1] + "," + bounds[2] + "," + bounds[3]);

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
            if (checkTileName(tile.name) != -1) {
                //UnityEngine.Debug.Log((int)(tile.position.x-xOffset) + "," + (int)(tile.position.z-yOffset));
                int x = (int)(tile.position.x-xOffset);
                int y = (int)(tile.position.z-yOffset);
                tiles[x, y] = checkTileName(tile.name);

                ClickableTile ct = tile.gameObject.GetComponent<ClickableTile>();
                ct.TileX = x;//(int)tile.position.x;
                ct.TileY = y;//(int)tile.position.z;
                ct.map = this;
                ct.isWalkable = tileTypes[checkTileName(tile.name)].isWalkable;
                clickableTiles[x, y] = ct;

                //UnityEngine.Debug.Log(clickableTiles[x, y].isWalkable);
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

        //checks if move UI was clicked before moving unit
        if (moveButtonPressed == true || selectedUnit.GetComponent<Enemy_Character_Class>() != null) {

            setMoveButtonPressed(false);
            moveButtonPressed = false;
            removeMovementLimit();

            //UnityEngine.Debug.Log(x + "," + y);

            //TEST - replace with actual movement implementation
            if (selectedUnit != null && clickableTiles[x, y].isWalkable)
            {
                if (selectedUnitScript.targeting == true || selectedUnitScript.hasWalked)
                {
                    selectedUnitScript.targeting = false;
                }
                else if (selectedUnitScript.charSelected || selectedUnit.GetComponent<Enemy_Character_Class>())
                {
                    hidePath();
                    generatePathTo(x, y);
                    //UnityEngine.Debug.Log(currentPath.Count);

                    //selectedUnitScript.charSelected = false;

                    //selectedUnitScript.tileX = currentPath[1].x;
                    //selectedUnitScript.tileY = currentPath[1].y;
                    //selectedUnit.transform.position = TileCoordToWorldCoord(currentPath[1].x,currentPath[1].y);

                    comlog.addText("-> " + selectedUnit.name + " has Moved");
                }
            }
        }

    }

    public bool checkForTileEffect(int x, int y, string effectName) {
        if (clickableTiles[x,y] == null) {
            return false;
        }
        foreach (TileEffect effect in clickableTiles[x, y].effectsOnTile) {
            if (effect.name == effectName) {
                return true;
            }
        }
        return false;
    }

    public void moveCharTo(GameObject t, int x, int y) {
        Basic_Character_Class target = t.GetComponent<Basic_Character_Class>();

        //check if new pos is walkable
        ClickableTile newTile = clickableTiles[x,y];
        if (newTile != null && newTile.isWalkable) {
            newTile.characterOnTile = t;
            target.tile.characterOnTile = null;
            target.tile = newTile;
            target.tileX = x;
            target.tileY = y;

            float newX = (float) x + xOffset;
            float newY = (float) y + yOffset;
            Vector3 tileVec = new Vector3(newX, 1.0f, newY);
            //UnityEngine.Debug.Log("Prev pos: " + t.transform.position + " new pos: " + tileVec);
            t.transform.position = tileVec;
        }
    }

    public float getPathLength(Dictionary<Node,float> dist) {
        float len = 0f;

        for (int i = 0; i < currentPath.Count-1; i++) {

        }

        return len;
    }
    public List<Node> generatePathTo(int x, int y, bool visual = false, bool ignoreTargetWalkable = false, int startX = -1, int startY = -1, bool noWalls = false, bool setCurrent = true, bool cutPath = true, bool fromDisplay = false){

        if (startX == -1) {
            if (selectedUnitScript.tileX == x && selectedUnitScript.tileY == y){
                currentPath = new List<Node>();
                selectedUnitScript.path = currentPath;
                return null;
            }
        }

        List<Node> currentPathTemp = null;

        if (setCurrent) {
            selectedUnitScript.path = null;
            currentPath = null;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        Node source;
        if (startX == -1) {
            source = graph[selectedUnitScript.tileX, selectedUnitScript.tileY];
        }
        else {
            source = graph[startX, startY];
        }
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
                float alt = 0;
                if (n == target) {
                    alt = dist[u] + costToEnterTile(n.x, n.y, ignoreTargetWalkable, noWalls);
                }
                else {
                    alt = dist[u] + costToEnterTile(n.x, n.y, false, noWalls);
                }
                if (alt < dist[n]){
                    dist[n] = alt;
                    prev[n] = u;
                }
            }
        }
        if (prev[target] == null){
            return null;
        }

        currentPathTemp = new List<Node>();
        Node curr = target;

        //step through current path and add it to chain
        while (curr != null){
            currentPathTemp.Add(curr);
            curr = prev[curr];
        }
        
        currentPathTemp.Reverse();

        //UnityEngine.Debug.Log("Path Count: " + currentPathTemp.Count);
        //UnityEngine.Debug.Log("Movement: " + selectedUnitScript.movementSpeed.moddedValue);

        if (selectedUnit != null && cutPath) {
            currentPathTemp = cutDownPath(selectedUnitScript.movementSpeed.moddedValue, false, currentPathTemp);
        }

        if (setCurrent && fromDisplay == false) {
            currentPath = currentPathTemp;
            selectedUnitScript.path = currentPath;
            //Disables them from walking again
            if (currentPath != null && currentPath.Count > 1) {
                selectedUnitScript.hasWalked = true;
            }
        }

        return currentPathTemp;
        //showPath();
    }

    //Despite its existing name, this does a little more than that
    public List<Node> cutDownPath(int range, bool visual = false, List<Node> path = null, bool cutDown = true, bool noWalls = false) {    //This is recursive, so do take that into consideration
        //List<Node> l = currentPath;
        //if (visual) l = visualPath;
        List<Node> l = path;

        if ((l == null) || (l.Count <= 0)) {
            return l; //Nowhere to go 
        }

        //First calculate l's max range
        float lCutoff = 0;//l.Count - 1;
        //Loop through each node, adding it's tile's value to 
        for (int i = 1; i < l.Count; i++) {
            int nodeX = l[i].x;
            int nodeY = l[i].y;
            lCutoff += costToEnterTile(nodeX,nodeY, false, noWalls, true);
        }


        if (visual) {
            //If long enough, good enough. If not, red
            if (range >= lCutoff) {
                showPath(l.Count);
            } else {
                l.RemoveAt(l.Count - 1);
                cutDownPath(range,true,l);
            }

            return l;
        }
        
        //If character movement isn't enough, cut down path by one layer and try again. Basically, this is so that the enemy will move in the direction they mean to go even if it's out of range
        if (range < lCutoff) {

            if (phase != 0) {
                l.RemoveAt(l.Count - 1);
                cutDownPath(range,false,l);
            } else {
                l.Clear();//Clears the list
                currentPath = null;
                return l;
            }
        }
        return l;

    }
    public bool unitCanEnterTile(int x, int y) {

        //add section here for checking if space is occupied by other unit
        bool walkable = false;
        if (clickableTiles[x, y] != null) {
            walkable = clickableTiles[x, y].isWalkable;
        }
        return walkable;
    }

    public bool tileExists(int x, int y){
        if (checkIndex(x, y) && clickableTiles[x, y] != null){
            return true;
        }
        return false;
    }

    // public void selectedChar() {
    //     if(Input.GetMouseButtonDown(0)) {
    //         selectedUnit.setCharSelected(true);
    //     }
    // }

    public float pathMovementCost(List<Node> path) {
        float cost = 0;
        for (int i = 1; i < path.Count; i++) {
            cost += costToEnterTile(path[i].x,path[i].y,false, false, false, true);
        }
        return cost;
    }

    
    public float costToEnterTile(int x, int y, bool ignoreCanEnter = false, bool noWalls = false, bool cut = false, bool realCost = false) {

        if (!ignoreCanEnter) {
            if (unitCanEnterTile(x, y) == false) {
                return Mathf.Infinity;
            }
        }
        if (clickableTiles[x, y] == null) {
            return Mathf.Infinity;
        }
        int cost = clickableTiles[x, y].cost;
        if (noWalls && Array.IndexOf(wallNums, tiles[x,y]) != -1) {
            cost = 1;
        }

        if (cost <= 0){
            cost = 1;
        }
        if ((cut == false && selectedUnit.GetComponent<Enemy_Character_Class>() != null) || (selectedUnit.GetComponent<Hero_Character_Class>() != null && heroAvoidFire && !realCost && !cut)) {
            if (checkForTileEffect(x,y,"Burning")) {
                cost += 10;
            }
        }
        float dist = cost;

        return dist;
    }

    public void showPath(int blue = 999) { //This displays the finalized visualpath created by visualpathTo
        if (moveButtonPressed) {
            hidePath();
        }

            //Create path of CircleArrows
            for (int i = 0; i < visualPath.Count; i++) {
                GameObject ca = Instantiate(circleArrowPrefab);
            if (i >= blue) {
                    SpriteRenderer sr = ca.GetComponent<SpriteRenderer>();
                    Sprite nS = Resources.Load<Sprite>("spr_circle_red");
                    sr.sprite = nS;
                }
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
    public void visualPathTo(int x, int y) {
        visualToX = x;
        visualToY = y;
        if (!moveButtonPressed) {
            
            return;
        }

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

        if ((selectedUnit != null)) {
            List<Node> bluePath = new List<Node>(visualPath);
            //UnityEngine.Debug.Log("BluePath Count:" + bluePath.Count);
            cutDownPath(selectedUnitScript.movementSpeed.moddedValue,true,bluePath);
        }
        
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
    public void drawReach(int reach, bool targetTiles, bool targetAllies, bool targetEnemies, bool targetWalls, bool hyperSpecificTargeting, bool needSpecificTileEffects, List<string> specificTileEffects, bool needSpecificTiles, List<string> specificTiles, ClickableTile tile, bool targetBreakableTiles = false)
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
        GameObject newTile = tile.gameObject;

        allTargets = new List<GameObject>();

        if (selectedUnit.GetComponent<Enemy_Character_Class>())
        {
            targetAllies = !targetAllies;
            targetEnemies = !targetEnemies;
        } 

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
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(newTile.transform.position, clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.position - newTile.transform.position, Vector3.Distance(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.position, newTile.transform.position), layerMask: mask);
                    //!Physics.Linecast(newTile.transform.position, clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.position, mask)
                    if(hits.Length == 0){
                        if (targetTiles == true)
                        {
                            //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                            targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                        }
                        else{ 
                            if (targetAllies == true && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile != null && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject.tag == "PlayerTeam"){
                                //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                                targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject);
                            }
                            if (targetEnemies == true && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile != null && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject.tag == "EnemyTeam")
                            {
                                //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                                targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject);
                            }       
                        }
                        if (hyperSpecificTargeting){
                            if (needSpecificTiles){
                                if (specificTiles.Contains(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.name)){
                                    if (!targetList.Contains(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject)){
                                        //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                                    }
                                }
                            }
                            if (needSpecificTileEffects){
                                for (int k = 0; k < clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].effectsOnTile.Count; k++){
                                    if(specificTileEffects.Contains(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].effectsOnTile[k].name)){
                                        if (!targetList.Contains(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject)){
                                            //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                                            targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                                        }
                                    }
                                }
                            }
                        }
                        allTargets.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                        /*
                        if (j == partialWidth){
                            clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineT").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineR").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            if (i == reach){
                                clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            }
                            rangeDisplay.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                        }
                        else if (j == ((-1 * partialWidth))){
                            clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineR").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            rangeDisplay.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                        }
                        */
                    } else if (targetBreakableTiles == true && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].isBreakable == true) {
                        clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                    } else if (targetWalls && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.tag == "Wall" && hits.Length <= 1){
                        clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                    }
                    //else {
                    //    behindWall(tile, clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j]);
                    //}
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
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(newTile.transform.position, clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.position - newTile.transform.position, Vector3.Distance(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.position, newTile.transform.position), mask);
                    //!Physics.Linecast(newTile.transform.position, clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.position, mask)
                    if (hits.Length == 0){
                        if (targetTiles == true)
                        {
                            //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                            targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                        }
                        else{ 
                            if (targetAllies == true && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile != null && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject.tag == "PlayerTeam"){
                                //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                                targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject);
                            }
                            if (targetEnemies == true && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile != null && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject.tag == "EnemyTeam")
                            {
                                //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                                targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].characterOnTile.gameObject);
                            }
                        }
                        if (hyperSpecificTargeting){
                            if (needSpecificTiles){
                                if (specificTiles.Contains(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.name)){
                                    if (!targetList.Contains(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject)){
                                        //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                                    }
                                }
                            }
                            if (needSpecificTileEffects){
                                for (int k = 0; k < clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].effectsOnTile.Count; k++){
                                    if(specificTileEffects.Contains(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].effectsOnTile[k].name)){
                                        if (!targetList.Contains(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject)){
                                            //clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                                            targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                                        }
                                    }
                                }
                            }
                        }
                        allTargets.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                        /*
                        if (j == partialWidth){
                            clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineT").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineL").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            if (i == -reach){
                                clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            }
                            rangeDisplay.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                        }
                        else if (j == ((-1 * partialWidth))){
                            clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.transform.Find("OutlineL").gameObject.GetComponent<MeshRenderer>().enabled = true;
                            rangeDisplay.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                        }
                        */
                    } else if (targetBreakableTiles == true && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].isBreakable == true) {
                                clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                                targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);
                    }
                    else if (targetWalls && clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject.tag == "Wall" && hits.Length <= 1){
                        clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j].gameObject);                    
                    }
                    //else {
                    //    behindWall(tile, clickableTiles[selectedUnitScript.tileX + i, selectedUnitScript.tileY + j]);
                    //}
                }
            }
            partialWidth--;
        }
        for (int i = 0; i <= reach; i++)
        {
            if (checkIndex(selectedUnitScript.tileX, selectedUnitScript.tileY + i) && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i] != null)
            {
                RaycastHit[] hits;
                hits = Physics.RaycastAll(newTile.transform.position, clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject.transform.position - newTile.transform.position, Vector3.Distance(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject.transform.position, newTile.transform.position), mask);
                //!Physics.Linecast(newTile.transform.position, clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject.transform.position, mask)
                if (hits.Length == 0){
                    if (targetTiles == true)
                    {
                        //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject);
                    }
                    else{ 
                        if (targetAllies == true && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile.gameObject.tag == "PlayerTeam"){
                            //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                            targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile.gameObject);
                        }
                        if (targetEnemies == true && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile.gameObject.tag == "EnemyTeam")
                        {
                            //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                            targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].characterOnTile.gameObject);
                        }
                        
                    }
                    if (hyperSpecificTargeting){
                        if (needSpecificTiles){
                            if (specificTiles.Contains(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject.name)){
                                if (!targetList.Contains(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject)){
                                    //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                                    targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject);
                                }
                            }
                        }
                        if (needSpecificTileEffects){
                            for (int k = 0; k < clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].effectsOnTile.Count; k++){
                                if(specificTileEffects.Contains(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].effectsOnTile[k].name)){
                                    if (!targetList.Contains(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject)){
                                        //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                                        targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject);
                                    }
                                }
                            }
                        }
                    }
                    allTargets.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject);
                    /*
                    if (i == reach){
                        clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject.transform.Find("OutlineT").gameObject.GetComponent<MeshRenderer>().enabled = true;
                        clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject.transform.Find("OutlineL").gameObject.GetComponent<MeshRenderer>().enabled = true;
                        clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject.transform.Find("OutlineR").gameObject.GetComponent<MeshRenderer>().enabled = true;
                        rangeDisplay.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject);
                    }
                    */
                } else if (targetBreakableTiles == true && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].isBreakable == true) {
                            clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                            targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject);
                }
                else if (targetWalls && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject.tag == "Wall" && hits.Length <= 1){
                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].highlight();
                    targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i].gameObject);
                }
                //else {
                //    behindWall(tile, clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY + i]);
                //}
            }
        }
        for (int i = 1; i <= reach; i++)
        {
            if (checkIndex(selectedUnitScript.tileX, selectedUnitScript.tileY - i) && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i] != null)
            {
                RaycastHit[] hits;
                hits = Physics.RaycastAll(newTile.transform.position, clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject.transform.position - newTile.transform.position, Vector3.Distance(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject.transform.position, newTile.transform.position), mask);
                //!Physics.Linecast(newTile.transform.position, clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject.transform.position, mask)
                if (hits.Length == 0){
                    if (targetTiles == true)
                    {
                        //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                        targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject);
                    }
                    else{ 
                        if (targetAllies == true && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile.gameObject.tag == "PlayerTeam"){
                            //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                            targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile.gameObject);
                        }
                        else if (targetEnemies == true && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile != null && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile.gameObject.tag == "EnemyTeam")
                        {
                            //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                            targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].characterOnTile.gameObject);
                        }
                        
                    }
                    if (hyperSpecificTargeting){
                        if (needSpecificTiles){
                            if (specificTiles.Contains(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject.name)){
                                if (!targetList.Contains(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject)){
                                    //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                                    targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject);
                                }
                            }
                        }
                        if (needSpecificTileEffects){
                            for (int k = 0; k < clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].effectsOnTile.Count; k++){
                                if(specificTileEffects.Contains(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].effectsOnTile[k].name)){
                                    if (!targetList.Contains(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject)){
                                        //clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                                        targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject);
                                    }
                                }
                            }
                        }
                    }
                    allTargets.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject);
                    /*
                    if (i == reach){
                        clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject.transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = true;
                        clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject.transform.Find("OutlineL").gameObject.GetComponent<MeshRenderer>().enabled = true;
                        clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject.transform.Find("OutlineR").gameObject.GetComponent<MeshRenderer>().enabled = true;
                        rangeDisplay.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject);
                    }
                    */
                } else if (targetBreakableTiles == true && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].isBreakable == true) {
                            clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                            targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject);
                }
                else if (targetWalls && clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject.tag == "Wall" && hits.Length <= 1){
                    clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].highlight();
                    targetList.Add(clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i].gameObject);
                }
                //else {
                //    behindWall(tile, clickableTiles[selectedUnitScript.tileX, selectedUnitScript.tileY - i]);
                //}
            }
        }
        //if (selectedUnit.gameObject.tag == "PlayerTeam"){
            displayRange();
            if (selectedUnitScript.attackType == "Spell" && selectedUnit.GetComponent<Hero_Character_Class>() != null){
                displayAOE(selectedUnitScript.attackType, tile, size: selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.AOEsize, square: selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.square, targetersTile: selectedUnitScript.tile);
            }
            else if (selectedUnitScript.attackType == "Ability" && selectedUnit.GetComponent<SummonClass>() != null){
                displayAOE(selectedUnitScript.attackType, tile, size: selectedUnit.GetComponent<SummonClass>().selectedAbility.AOEsize, square: selectedUnit.GetComponent<SummonClass>().selectedAbility.square, targetersTile: selectedUnitScript.tile);
            }
        //}
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
            if (targetList[i].GetComponent<ClickableTile>()){
                targetList[i].GetComponent<ClickableTile>().endHighlight();
            }
            else{
                targetList[i].GetComponent<Basic_Character_Class>().tile.endHighlight();
            }
        }
        targetList = new List<GameObject>();

        for (int i = 0; i < allTargets.Count; i++){
            allTargets[i].gameObject.transform.Find("OutlineL").gameObject.GetComponent<MeshRenderer>().enabled = false;
            allTargets[i].gameObject.transform.Find("OutlineR").gameObject.GetComponent<MeshRenderer>().enabled = false;
            allTargets[i].gameObject.transform.Find("OutlineT").gameObject.GetComponent<MeshRenderer>().enabled = false;
            allTargets[i].gameObject.transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        allTargets = new List<GameObject>();

    }

    public void displayAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        UnityEngine.Debug.Log("Display AOE");
        displayingAOE = true;
        aoeDisplayTiles = new List<GameObject>();
        if (attackType == "Spell" && selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.alternateAOEDisplay){
            GameObject prefab = Instantiate(selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.spellPrefab);
            aoeDisplayTiles = prefab.GetComponent<Cast_Spell>().displaySpecificAOE(attackType, centerTile, size, square, targetersTile);
            Destroy(prefab);
        }
        else if (attackType == "Ability" && selectedUnit.GetComponent<SummonClass>().selectedAbility.alternateAOEDisplay){
            GameObject prefab = Instantiate(selectedUnit.GetComponent<SummonClass>().selectedAbility.spellPrefab);
            aoeDisplayTiles = prefab.GetComponent<Cast_Spell>().displaySpecificAOE(attackType, centerTile, size, square, targetersTile);
            Destroy(prefab);
        }
        else if ((attackType == "Spell" || attackType == "Ability") && ((selectedUnit.GetComponent<Hero_Character_Class>() != null && selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.targeted == false) || (selectedUnit.GetComponent<SummonClass>() != null && selectedUnit.GetComponent<SummonClass>().selectedAbility.targeted == false))){
            if (targetList != null){
                for (int i = 0; i < targetList.Count; i++){
                    if (targetList[i].GetComponent<ClickableTile>() != null){
                        targetList[i].GetComponent<ClickableTile>().canHit();
                        aoeDisplayTiles.Add(targetList[i]);
                    }
                    else {
                        targetList[i].GetComponent<Basic_Character_Class>().tile.canHit();
                        aoeDisplayTiles.Add(targetList[i].GetComponent<Basic_Character_Class>().tile.gameObject);
                    }
                }
            }
        }
        else if (size == 0 && centerTile.gameObject.tag != "Wall"){
            centerTile.canHit();
            aoeDisplayTiles.Add(centerTile.gameObject);
        }
        else if (square){
            if (centerTile.gameObject.tag == "Wall"){
                centerTile = shuntOver(centerTile, targetersTile);
            }
            centerTile.canHit();
            aoeDisplayTiles.Add(centerTile.gameObject);
            for (int j = 0; j < size + 1; j++){
                for (int i = 0; i < j; i++){
                    if (tileExists(centerTile.TileX + i, centerTile.TileY + j) && !clickableTiles[centerTile.TileX + i, centerTile.TileY + j].gameObject.name.Contains("Wall") /*&& checkVisible(targetersTile.gameObject.transform.position, clickableTiles[centerTile.TileX + i, centerTile.TileY + j].gameObject.transform.position)*/){
                        clickableTiles[centerTile.TileX + i, centerTile.TileY + j].canHit();
                        aoeDisplayTiles.Add(clickableTiles[centerTile.TileX + i, centerTile.TileY + j].gameObject);
                    }
                }
                if (tileExists(centerTile.TileX + j, centerTile.TileY + j) && !clickableTiles[centerTile.TileX + j, centerTile.TileY + j].gameObject.name.Contains("Wall") /*&& checkVisible(targetersTile.gameObject.transform.position, clickableTiles[centerTile.TileX + j, centerTile.TileY + j].gameObject.transform.position)*/){
                    clickableTiles[centerTile.TileX + j, centerTile.TileY + j].canHit();
                    aoeDisplayTiles.Add(clickableTiles[centerTile.TileX + j, centerTile.TileY + j].gameObject);
                }
                for (int i = 0; i < j * 2; i++){
                    if (tileExists(centerTile.TileX + j, centerTile.TileY + j - i) && !clickableTiles[centerTile.TileX + j, centerTile.TileY + j - i].gameObject.name.Contains("Wall")  /*&& checkVisible(targetersTile.gameObject.transform.position, clickableTiles[centerTile.TileX + j, centerTile.TileY + j - i].gameObject.transform.position)*/){
                        clickableTiles[centerTile.TileX + j, centerTile.TileY + j - i].canHit();
                        aoeDisplayTiles.Add(clickableTiles[centerTile.TileX + j, centerTile.TileY + j - i].gameObject);
                    }
                }
                if (tileExists(centerTile.TileX + j, centerTile.TileY - j) && !clickableTiles[centerTile.TileX + j, centerTile.TileY - j].gameObject.name.Contains("Wall") /*&& checkVisible(targetersTile.gameObject.transform.position, clickableTiles[centerTile.TileX + j, centerTile.TileY - j].gameObject.transform.position)*/){
                    clickableTiles[centerTile.TileX + j, centerTile.TileY - j].canHit();
                    aoeDisplayTiles.Add(clickableTiles[centerTile.TileX + j, centerTile.TileY - j].gameObject);
                }
                for (int i = 0; i < j * 2; i++){
                    if (tileExists(centerTile.TileX + j - i, centerTile.TileY - j) && !clickableTiles[centerTile.TileX + j - i, centerTile.TileY - j].gameObject.name.Contains("Wall") /*&& checkVisible(targetersTile.gameObject.transform.position, clickableTiles[centerTile.TileX + j - i, centerTile.TileY - j].gameObject.transform.position)*/){
                        clickableTiles[centerTile.TileX + j - i, centerTile.TileY - j].canHit();
                        aoeDisplayTiles.Add(clickableTiles[centerTile.TileX + j - i, centerTile.TileY - j].gameObject);
                    }
                }
                if (tileExists(centerTile.TileX - j, centerTile.TileY - j) && !clickableTiles[centerTile.TileX - j, centerTile.TileY - j].gameObject.name.Contains("Wall") /*&& checkVisible(targetersTile.gameObject.transform.position, clickableTiles[centerTile.TileX - j, centerTile.TileY - j].gameObject.transform.position)*/){
                    clickableTiles[centerTile.TileX - j, centerTile.TileY - j].canHit();
                    aoeDisplayTiles.Add(clickableTiles[centerTile.TileX - j, centerTile.TileY - j].gameObject);
                }
                for (int i = 0; i < j * 2; i++){
                    if (tileExists(centerTile.TileX - j, centerTile.TileY - j + i) && !clickableTiles[centerTile.TileX - j, centerTile.TileY - j + i].gameObject.name.Contains("Wall") /*&& checkVisible(targetersTile.gameObject.transform.position, clickableTiles[centerTile.TileX - j, centerTile.TileY - j + i].gameObject.transform.position)*/){
                        clickableTiles[centerTile.TileX - j, centerTile.TileY - j + i].canHit();
                        aoeDisplayTiles.Add(clickableTiles[centerTile.TileX - j, centerTile.TileY - j + i].gameObject);
                    }
                }
                if (tileExists(centerTile.TileX - j, centerTile.TileY + j) && !clickableTiles[centerTile.TileX - j, centerTile.TileY + j].gameObject.name.Contains("Wall") /*&& checkVisible(targetersTile.gameObject.transform.position, clickableTiles[centerTile.TileX - j, centerTile.TileY + j].gameObject.transform.position)*/){
                    clickableTiles[centerTile.TileX - j, centerTile.TileY + j].canHit();
                    aoeDisplayTiles.Add(clickableTiles[centerTile.TileX - j, centerTile.TileY + j].gameObject);
                }
                for (int i = 0; i < j - 1; i++){
                    if (tileExists(centerTile.TileX - j + 1 + i, centerTile.TileY + j) && !clickableTiles[centerTile.TileX - j + 1 + i, centerTile.TileY + j].gameObject.name.Contains("Wall") /*&& checkVisible(targetersTile.gameObject.transform.position, clickableTiles[centerTile.TileX - j + 1 + i, centerTile.TileY + j].gameObject.transform.position)*/){
                        clickableTiles[centerTile.TileX - j + 1 + i, centerTile.TileY + j].canHit();
                        aoeDisplayTiles.Add(clickableTiles[centerTile.TileX - j + 1 + i, centerTile.TileY + j].gameObject);
                    }
                }
            }
        }
    }

    public void removeAOEDisplay(){
        /*
        if (aoeDisplayTiles != null){
            for (int i = 0; i < aoeDisplayTiles.Count; i++){
                aoeDisplayTiles[i].transform.Find("OutlineT").gameObject.GetComponent<MeshRenderer>().enabled = false;
                aoeDisplayTiles[i].transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = false;
                aoeDisplayTiles[i].transform.Find("OutlineL").gameObject.GetComponent<MeshRenderer>().enabled = false;
                aoeDisplayTiles[i].transform.Find("OutlineR").gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        */
        if (aoeDisplayTiles != null){
            if (selectedUnitScript.attackType == "Spell" && selectedUnit.GetComponent<Hero_Character_Class>() != null && selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.alternateAOEDisplay){
                GameObject prefab = Instantiate(selectedUnit.GetComponent<Hero_Character_Class>().selectedSpell.spellPrefab);
                prefab.GetComponent<Cast_Spell>().removeAOEDisplay(aoeDisplayTiles);
                Destroy(prefab);
            }
            else{
                for (int i = 0; i < aoeDisplayTiles.Count; i++){
                    aoeDisplayTiles[i].GetComponent<ClickableTile>().removeHighlight();
                }
            }
        }
        displayingAOE = false;
        aoeDisplayTiles = null;
    }

    public bool checkVisible(Vector3 position1, Vector3 position2){
        if (Physics.Linecast(position1, position2, mask)){
            return false;
        }
        return true;
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
        if (targetList.Contains(selectedTarget) || (selectedTarget.GetComponent<ClickableTile>() != null && selectedTarget.GetComponent<ClickableTile>().characterOnTile != null && targetList.Contains(selectedTarget.GetComponent<ClickableTile>().characterOnTile)) || (selectedTarget.GetComponent<Basic_Character_Class>() != null && targetList.Contains(selectedTarget.GetComponent<Basic_Character_Class>().tile.gameObject))){
            return true;
        }
        //If the target can't be found return false
        return false;
    }


    //Swaps two different tiles on the map
    //Takes in the previous tile and the new tile to switch to, as well as the integer value of the TileType in the tileTypes array
    public ClickableTile swapTiles (ClickableTile previousTile, int tileNumber, bool transferEffects)
    {
        //transfers the values of the previous tile to the new tile
        GameObject newTilePrefab = Instantiate(tileTypes[tileNumber].tileVisualPrefab);
        newTilePrefab.name = tileTypes[tileNumber].tileVisualPrefab.name;
        ClickableTile newTile = newTilePrefab.GetComponent<ClickableTile>();
        newTilePrefab.transform.position = previousTile.gameObject.transform.position;
        newTile.characterOnTile = previousTile.characterOnTile;
        newTile.map = this;
        newTile.TileX = previousTile.TileX;
        newTile.TileY = previousTile.TileY;
        newTile.tileWas = tiles[previousTile.TileX,previousTile.TileY];
        //Sets the tile arrays in the map to contain the new tile
        tiles[previousTile.TileX, previousTile.TileY] = tileNumber;
        clickableTiles[previousTile.TileX, previousTile.TileY] = newTile;
        if (transferEffects){
            for (int i = 0; i < previousTile.effectsOnTile.Count; i++){
                newTile.addEffectToTile(previousTile.effectsOnTile[i]);
            }
        }
        else {
            while (previousTile.effectsOnTile.Count != 0){
                previousTile.removeEffectFromTile(previousTile.effectsOnTile[0]);
            }
        }
        //Checks if there was a character on the previous tile
        if (previousTile.characterOnTile != null)
        {
            //If so, also updates the character on the tile, adding the new effect and linking the character to the new tile
            Basic_Character_Class unitAffected = previousTile.characterOnTile.GetComponent<Basic_Character_Class>();
            unitAffected.tile = newTile;
            //unitAffected.removeStatus(unitAffected.tileEffect, true);
            StatusEffect newEffect = new StatusEffect();
            newEffect.initializeTileEffect(newTile.statsToEffect, newTile.name, newTile.effectAmounts, unitAffected.gameObject, newTile.name + "Effect");
            unitAffected.tileType = newTile.map.tileTypes[tileNumber];
            newTile.isWalkable = false;
        }
        //Destroys the old tile
        Destroy(previousTile.gameObject);
        return newTile;
    }

    //Adds a status effect to a unit as they enter a tile, takes in the x, y location of the tile and the unit the effect will be applied to
    public void addTileEffect(int x, int y, GameObject unitToAffect){
        //Creates the status effect and then applies it to the character
        StatusEffect newEffect = new StatusEffect();
        newEffect.initializeTileEffect(clickableTiles[x, y].statsToEffect, tileTypes[tiles[x, y]].name, clickableTiles[x, y].effectAmounts, unitToAffect, tileTypes[tiles[x, y]].name + "Effect");
    }

    //Sets phase to match that of the gamecontroller
    public void setPhase(int p = 0) {
        //Resets all units having already walked
        //if (phase != p) {  
        //}

        phase = p;
    }

    public void setMoveButtonPressed(bool b) {
        moveButtonPressed = b;
    }





    public string checkDirection(Vector3 startPosition, Vector3 targetPosition, ClickableTile originalTile){
        Vector3 normalizedVector = (startPosition - targetPosition).normalized;
        UnityEngine.Debug.Log(normalizedVector);
        if (normalizedVector.x > 0.71f && -0.71f < normalizedVector.z && normalizedVector.z < 0.71f){
            if (clickableTiles[originalTile.TileX + 1, originalTile.TileY].gameObject.tag != "Wall"){
                return "Right";
            }
            else if (normalizedVector.z > 0.0f){
                return "Top";
            }
            else {
                return "Bottom";
            }
        }
        else if(-0.71f < normalizedVector.x && normalizedVector.x < 0.71f && normalizedVector.z > 0.71f){
            if (clickableTiles[originalTile.TileX, originalTile.TileY + 1].gameObject.tag != "Wall"){
                return "Top";
            }
            else if (normalizedVector.x > 0.0f){
                return "Right";
            }
            else {
                return "Left";
            }
        }
        else if (normalizedVector.x < -0.71f && -0.71f < normalizedVector.z && normalizedVector.z < 0.71f){
            if (clickableTiles[originalTile.TileX - 1, originalTile.TileY].gameObject.tag != "Wall"){
                return "Left";
            }
            else if (normalizedVector.z > 0.0f){
                return "Top";
            }
            else{
                return "Bottom";
            }
        }
        else if (-0.71f < normalizedVector.x && normalizedVector.x < 0.71f && normalizedVector.z < -0.71f){
            if (clickableTiles[originalTile.TileX, originalTile.TileY - 1].gameObject.tag != "Wall"){
                return "Bottom";
            }
            else if (normalizedVector.x > 0.0f){
                return "Right";
            }
            else {
                return "Left";
            }
        }
        else if(Mathf.Abs(normalizedVector.x - 0.71f) < 1f && Mathf.Abs(normalizedVector.z - 0.71f) < 1f){
            return "TopRight";
        }
        else if(Mathf.Abs(normalizedVector.x - -0.71f) < 1f && Mathf.Abs(normalizedVector.z - 0.71f) < 1f){
            return "TopLeft";
        }
        else if(Mathf.Abs(normalizedVector.x - -0.71f) < 1f && Mathf.Abs(normalizedVector.z - -0.71f) < 1f){
            return "BottomLeft";
        }
        else if (Mathf.Abs(normalizedVector.x - 0.71f) < 1f && Mathf.Abs(normalizedVector.z - -0.71f) < 1f){
            return "BottomRight";
        }
        return "Failed To Find Direction";
    }

    public ClickableTile shuntOver(ClickableTile originalTile, ClickableTile casterTile){
        switch (checkDirection(casterTile.gameObject.transform.position, originalTile.gameObject.transform.position, originalTile)){
            case "Right":
                return clickableTiles[originalTile.TileX + 1, originalTile.TileY];

            case "Left":
                return clickableTiles[originalTile.TileX - 1, originalTile.TileY];

            case "Top":
                return clickableTiles[originalTile.TileX, originalTile.TileY + 1];
            
            case "Bottom":
                return clickableTiles[originalTile.TileX, originalTile.TileY - 1];
            
            case "TopLeft":
                return clickableTiles[originalTile.TileX - 1, originalTile.TileY + 1];

            case "TopRight":
                return clickableTiles[originalTile.TileX + 1, originalTile.TileY + 1];

            case "BottomRight":
                return clickableTiles[originalTile.TileX + 1, originalTile.TileY - 1];

            case "BottomLeft":
                return clickableTiles[originalTile.TileX - 1, originalTile.TileY - 1];
        }
        return originalTile;
    }

    public bool inRange(int X, int Y) {
        if (X < 0 || Y < 0 || X > clickableTiles.GetLength(0)-1 || Y > clickableTiles.GetLength(1)-1) {
            return false;
        }
        return true;
    }

    public void displayRange(){
        for (int i = 0; i < allTargets.Count; i++){
            ClickableTile tileToCheck = allTargets[i].GetComponent<ClickableTile>();
            if (!tileExists(tileToCheck.TileX + 1, tileToCheck.TileY) || (inRange(tileToCheck.TileX + 1, tileToCheck.TileY) && !allTargets.Contains(clickableTiles[tileToCheck.TileX + 1, tileToCheck.TileY].gameObject))){
                tileToCheck.gameObject.transform.Find("OutlineR").gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            if (!tileExists(tileToCheck.TileX - 1, tileToCheck.TileY) || (inRange(tileToCheck.TileX - 1, tileToCheck.TileY) && !allTargets.Contains(clickableTiles[tileToCheck.TileX - 1, tileToCheck.TileY].gameObject))){
                tileToCheck.gameObject.transform.Find("OutlineL").gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            if (!tileExists(tileToCheck.TileX, tileToCheck.TileY + 1) || (inRange(tileToCheck.TileX, tileToCheck.TileY + 1) && !allTargets.Contains(clickableTiles[tileToCheck.TileX, tileToCheck.TileY + 1].gameObject))){
                tileToCheck.gameObject.transform.Find("OutlineT").gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            if (!tileExists(tileToCheck.TileX, tileToCheck.TileY - 1) || (inRange(tileToCheck.TileX, tileToCheck.TileY - 1) && !allTargets.Contains(clickableTiles[tileToCheck.TileX, tileToCheck.TileY - 1].gameObject))){
                tileToCheck.gameObject.transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void pushCharacter(Basic_Character_Class characterToMove, int startX, int startY, string direction, int strength){
        switch (direction){
            case "Left":
                for (int i = 1; i < strength + 1; i++){
                    if (clickableTiles[startX - i, startY].isWalkable){
                        moveCharacterToTile(characterToMove, clickableTiles[startX, startY], clickableTiles[startX - i, startY]);
                        startX -= i;
                    }
                    else {
                        break;
                    }
                }
                break;
            
            case "Right":
                for (int i = 1; i < strength + 1; i++){
                    if (clickableTiles[startX + i, startY].isWalkable){
                        moveCharacterToTile(characterToMove, clickableTiles[startX, startY], clickableTiles[startX + i, startY]);
                        startX += i;
                    }
                    else {
                        break;
                    }
                }
                break;
            
            case "Up":
                for (int i = 1; i < strength + 1; i++){
                    if (clickableTiles[startX, startY + i].isWalkable){
                        moveCharacterToTile(characterToMove, clickableTiles[startX, startY + i], clickableTiles[startX, startY + i]);
                        startY += i;
                    }
                    else {
                        break;
                    }
                }
                break;
            
            case "Down":
                for (int i = 1; i < strength + 1; i++){
                    if (clickableTiles[startX, startY - i].isWalkable){
                        moveCharacterToTile(characterToMove, clickableTiles[startX, startY - i], clickableTiles[startX, startY - i]);
                        startY -= i;
                    }
                    else {
                        break;
                    }
                }
                break;
            
            case "LeftUp":
                for (int i = 1; i < strength + 1; i++){
                    if (clickableTiles[startX - i, startY + i].isWalkable){
                        moveCharacterToTile(characterToMove, clickableTiles[startX - i, startY + i], clickableTiles[startX - i, startY + i]);
                        startX -= i;
                        startY += i;
                    }
                    else {
                        break;
                    }
                }
                break;
            
            case "LeftDown":
                for (int i = 1; i < strength + 1; i++){
                    if (clickableTiles[startX - i, startY - i].isWalkable){
                        moveCharacterToTile(characterToMove, clickableTiles[startX - i, startY - i], clickableTiles[startX - i, startY - i]);
                        startX -= i;
                        startY -= i;
                    }
                    else {
                        break;
                    }
                }
                break;

            case "RightUp":
                for (int i = 1; i < strength + 1; i++){
                    if (clickableTiles[startX + i, startY + i].isWalkable){
                        moveCharacterToTile(characterToMove, clickableTiles[startX + i, startY + i], clickableTiles[startX + i, startY + i]);
                        startX += i;
                        startY += i;
                    }
                    else {
                        break;
                    }
                }
                break;
            
            case "RightDown":
                for (int i = 1; i < strength + 1; i++){
                    if (clickableTiles[startX + i, startY - i].isWalkable){
                        moveCharacterToTile(characterToMove, clickableTiles[startX + i, startY - i], clickableTiles[startX + i, startY - i]);
                        startX += i;
                        startY -= i;
                    }
                    else {
                        break;
                    }
                }
                break;
        }
    }

    public void moveCharacterToTile(Basic_Character_Class characterToMove, ClickableTile startTile, ClickableTile endTile){
        startTile.characterOnTile = null;
        startTile.isWalkable = true;

        characterToMove.tileX = endTile.TileX;
        characterToMove.tileY = endTile.TileY;
        //Used to apply buff/debuff to the player based on tile type stepped on
        characterToMove.tileType = tileTypes[tiles[endTile.TileX, endTile.TileY]];
        characterToMove.tile = clickableTiles[endTile.TileX, endTile.TileY];
        endTile.characterOnTile = characterToMove.gameObject;

        //Makes the tile impassable when a character stands on it
        endTile.isWalkable = false;

        addTileEffect(endTile.TileX, endTile.TileY, characterToMove.gameObject);
        //Removes the node the unit just travelled to from the path
        if (endTile.effectsOnTile.Count > 0){
            for(int i = 0; i < endTile.effectsOnTile.Count; i++){
                endTile.effectsOnTile[i].tileEffectPrefab.GetComponent<tileEffectActions>().performStepOnEffect(endTile);
                UnityEngine.Debug.Log(characterToMove);
            }
        }
        characterToMove.updateCharStats();
        characterToMove.gameObject.transform.position = new Vector3(endTile.transform.position.x, 1.0f, endTile.transform.position.z);
    }

/*
    public int finiteDirection(Vector3 startPosition, Vector3 targetPosition){
        Vector3 normalizedVector = (startPosition - targetPosition).normalized;
        float x = normalizedVector.x;
        float z = normalizedVector.z;
        if (x > 0.0f && x < 0.71f && z < 1f && z > 0.71f){
            return 1;
        }
        else if (Mathf.Abs(x - 0.71f) < 0.05f && Mathf.Abs(z - 0.71f) < 0.05f){
            return 2;
        }
        else if (x > 0.71f && x < 1f && z > 0f && z < 0.71f){
            return 3;
        }
        else if (Mathf.Abs(x - 1f) < 0.05f && Mathf.Abs(z) < 0.05f){
            return 4;
        }
        else if (x > 0.71f &&  x < 1f && z < 0.0f && z > -0.71f){
            return 5;
        }
        else if (Mathf.Abs(x - 0.71f) < 0.05f && Mathf.Abs(z + 0.71f) < 0.05f){
            return 6;
        }
        else if(x < 0.71f && x > 0.0f && z < -0.71f && z > -1f){
            return 7;
        }
        else if(Mathf.Abs(x) < 0.05f && Mathf.Abs(z + 1f) < 0.05f){
            return 8;
        }
        else if (x < 0.0f && x > -0.71f && z < -0.71f && z > -1f){
            return 9;
        }
        else if(Mathf.Abs(x + 0.71f) < 0.05f && Mathf.Abs(z + 0.71f) < 0.05f){
            return 10;
        }
        else if (x < -0.71f && x > -1f && z > -0.71f && z < 0.0f){
            return 11;
        }
        else if (Mathf.Abs(x + 1) < 0.05f && Mathf.Abs(z) < 0.05f){
            return 12;
        }
        else if(x > -1f && x < -0.71f && z > 0.0f && z < 0.71f){
            return 13;
        }
        else if (Mathf.Abs(x + 0.71f) < 0.05f && Mathf.Abs(z - 0.71f) < 0.05f){
            return 14;
        }
        else if(x > -0.71f && x < 0.0f && z > 0.71f && z < 1f){
            return 15;
        }
        else if (Mathf.Abs(x) < 0.05f && Mathf.Abs(z - 1f) < 0.05f){
            return 16;
        }
        return 0;
    }
*/


    public void displayMovementLimit(int x, int y){
        if (displayMovementDisplay){
            List<GameObject> tiles = new List<GameObject>();
            bool finish = false;
            List<Node> newList = null;
            bool hit = false;
            int j = 1;
            while(finish == false){
                hit = false;
                UnityEngine.Debug.Log("Here");
                for (int i = 0; i < j; i++){
                    if (checkIndex(x + i, y + j) && tileExists(x + i, y + j)){
                        newList = generatePathTo(x + i, y + j, fromDisplay: true);
                        if (newList != null && newList.Count != 0){
                            tiles.Add(clickableTiles[x + i, y + j].gameObject);
                            hit = true;
                        }
                        newList = null;
                    }
                }
                if (checkIndex(x + j, y + j) && tileExists(x + j, y + j)){
                        newList = generatePathTo(x + j, y + j, fromDisplay: true);
                        if (newList != null && newList.Count != 0){
                            tiles.Add(clickableTiles[x + j, y + j].gameObject);
                            hit = true;
                        }
                        newList = null;
                }
                for (int i = 0; i < j * 2; i++){
                    if (checkIndex(x + j, y + j - i) && tileExists(x + j, y + j - i)){
                        newList = generatePathTo(x + j, y + j - i, fromDisplay: true);
                        if (newList != null && newList.Count != 0){
                            tiles.Add(clickableTiles[x + j, y + j - i].gameObject);
                            hit = true;
                        }
                        newList = null;
                    }
                }
                if (checkIndex(x + j, y - j) && tileExists(x + j, y - j)){
                        newList = generatePathTo(x + j, y - j, fromDisplay: true);
                        if (newList != null && newList.Count != 0){
                            tiles.Add(clickableTiles[x + j, y - j].gameObject);
                            hit = true;
                        }
                        newList = null;
                }
                for (int i = 0; i < j * 2; i++){
                    if (checkIndex(x + j - i, y - j) && tileExists(x + j - i, y - j)){
                        newList = generatePathTo(x + j - i, y - j, fromDisplay: true);
                        if (newList != null && newList.Count != 0){
                            tiles.Add(clickableTiles[x + j - i, y - j].gameObject);
                            hit = true;
                        }
                        newList = null;
                    }
                }
                if (checkIndex(x - j, y - j) && tileExists(x - j, y - j)){
                        newList = generatePathTo(x - j, y - j, fromDisplay: true);
                        if (newList != null && newList.Count != 0){
                            tiles.Add(clickableTiles[x - j, y - j].gameObject);
                            hit = true;
                        }
                        newList = null;
                }
                for (int i = 0; i < j * 2; i++){
                    if (checkIndex(x - j, y - j + i) && tileExists(x - j, y - j + i)){
                        newList = generatePathTo(x - j, y - j + i, fromDisplay: true);
                        if (newList != null && newList.Count != 0){
                            tiles.Add(clickableTiles[x - j, y - j + i].gameObject);
                            hit = true;
                        }
                        newList = null;
                    }
                }
                if (checkIndex(x - j, y + j) && tileExists(x - j, y + j)){
                        newList = generatePathTo(x - j, y + j, fromDisplay: true);
                        if (newList != null && newList.Count != 0){
                            tiles.Add(clickableTiles[x - j, y + j].gameObject);
                            hit = true;
                        }
                        newList = null;
                }
                for (int i = 0; i < j - 1; i++){
                    if (checkIndex(x - j + 1 + i, y + j) && tileExists(x - j + 1 + i, y + j)){
                        newList = generatePathTo(x - j + 1 + i, y + j, fromDisplay: true);
                        if (newList != null && newList.Count != 0){
                            tiles.Add(clickableTiles[x - j + 1 + i, y + j].gameObject);
                            hit = true;
                        }
                        newList = null;
                    }
                }
                if (hit){
                    UnityEngine.Debug.Log(j);
                    j++;
                }
                else{
                    finish = true;
                }
            }
            UnityEngine.Debug.Log(tiles.Count);
            tiles.Add(clickableTiles[x, y].gameObject);
            for (int i = 0; i < tiles.Count; i++){
                ClickableTile tileToCheck = tiles[i].GetComponent<ClickableTile>();
                if (!tileExists(tileToCheck.TileX + 1, tileToCheck.TileY) || !tiles.Contains(clickableTiles[tileToCheck.TileX + 1, tileToCheck.TileY].gameObject)){
                    tileToCheck.gameObject.transform.Find("OutlineR").gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                if (!tileExists(tileToCheck.TileX - 1, tileToCheck.TileY) || !tiles.Contains(clickableTiles[tileToCheck.TileX - 1, tileToCheck.TileY].gameObject)){
                    tileToCheck.gameObject.transform.Find("OutlineL").gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                if (!tileExists(tileToCheck.TileX, tileToCheck.TileY + 1) || !tiles.Contains(clickableTiles[tileToCheck.TileX, tileToCheck.TileY + 1].gameObject)){
                    tileToCheck.gameObject.transform.Find("OutlineT").gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                if (!tileExists(tileToCheck.TileX, tileToCheck.TileY - 1) || !tiles.Contains(clickableTiles[tileToCheck.TileX, tileToCheck.TileY - 1].gameObject)){
                    tileToCheck.gameObject.transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            currentPath = null;
            movementDisplayTiles = tiles;
        }
    }

    public void removeMovementLimit(){
        if (movementDisplayTiles != null && movementDisplayTiles.Count > 0 && displayMovementDisplay){
            for (int i = 0; i < movementDisplayTiles.Count; i++){
                movementDisplayTiles[i].gameObject.transform.Find("OutlineL").gameObject.GetComponent<MeshRenderer>().enabled = false;
                movementDisplayTiles[i].gameObject.transform.Find("OutlineR").gameObject.GetComponent<MeshRenderer>().enabled = false;
                movementDisplayTiles[i].gameObject.transform.Find("OutlineT").gameObject.GetComponent<MeshRenderer>().enabled = false;
                movementDisplayTiles[i].gameObject.transform.Find("OutlineB").gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            movementDisplayTiles = new List<GameObject>();
        }
    }
}


