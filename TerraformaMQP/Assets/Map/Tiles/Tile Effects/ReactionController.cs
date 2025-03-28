using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionController : MonoBehaviour
{

    public List<TileEffect> tileEffects;
    public List<GameObject> tilePrefabs;
    public BuffClass joltedPrefab;


    IEnumerator Start(){
        while(this.gameObject.GetComponent<TileMap>().mapCreated == false){
            yield return null;
        }

        for (int i = 0; i < this.gameObject.GetComponent<TileMap>().tileTypes.Length; i++){
            tilePrefabs.Add(this.gameObject.GetComponent<TileMap>().tileTypes[i].tileVisualPrefab);
        }
    }

    public void checkReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        //UnityEngine.Debug.Log("Here");
        bool checkTile = true;
        if (tile.effectsOnTile != null){
            for (int i = 0; i < tile.effectsOnTile.Count; i++){
                switch (tile.effectsOnTile[i].name){
                    case "Burning":
                        //UnityEngine.Debug.Log("Here");
                        checkTile = checkBurningReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "Soaked":
                        checkTile = checkSoakedReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "Foggy":
                        checkTile = checkFoggyReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;
                    
                    case "Electrified":
                        //checkTile = checkElectrifiedReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;
                    
                    case "Frozen":
                        //checkTile = checkFrozenReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "Rocky":
                        //checkTile = checkRockyReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "Sandstorm":
                        //checkTile = checkSandstormReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "Snowstorm":
                        //checkTile = checkSnowstormReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "Overgrown":
                        //checkTile = checkOvergrownReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "Tailwind":
                        //checkTile = checkTailwindReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "Trapped":
                        //checkTile = checkTrappedReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "Heated":
                        //checkTile = checkHeatedReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                        break;

                    case "HearthFire":
                        //UnityEngine.Debug.Log("Here");
                        checkTile = checkHearthfireReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
                    break;
                }
            }
        }
        if (checkTile){
            if (tile.gameObject.name.Contains("tileGrass")){
                checkGrassReaction(tile, damageType, source, playerTeam);
            }
            else if(tile.gameObject.name.Contains("tileDirt")){
                checkDirtReaction(tile, damageType, source, playerTeam);
            }
            else if(tile.gameObject.name.Contains("tileMud")){
                checkMudReaction(tile, damageType, source, playerTeam);
            }
            else if(tile.gameObject.name.Contains("tileIce")){

            }
            else if(tile.gameObject.name.Contains("tileStone")){

            }
            else if(tile.gameObject.name.Contains("tileWoodPlank")){
                checkWoodPlankReaction(tile, damageType, source, playerTeam);
            }
            else if(tile.gameObject.name.Contains("tileDenseForest")){
                checkDenseForestReaction(tile, damageType, source, playerTeam);

            }
            else if(tile.gameObject.name.Contains("tileLightForest")){
                checkLightForestReaction(tile, damageType, source, playerTeam);
            }
            else if(tile.gameObject.name.Contains("tileShallowWater")){
                checkShallowWaterReaction(tile, damageType, source, playerTeam);
            }
            else if(tile.gameObject.name.Contains("tileDeepWater")){
                checkDeepWaterReaction(tile, damageType, source, playerTeam);
            }
            else if(tile.gameObject.name.Contains("tileSand")){

            }
            else if(tile.gameObject.name.Contains("tileGlass")){

            }
            else if(tile.gameObject.name.Contains("tileMetal")){

            }
            else if(tile.gameObject.name.Contains("tileAshen")){

            }
            else if(tile.gameObject.name.Contains("tileMountain")){

            }
            else if(tile.gameObject.name.Contains("tileHill")){

            }
            else if(tile.gameObject.name.Contains("tileWall")){

            }
            else if(tile.gameObject.name.Contains("tileWhiteVoid")){
                
            } 

        }
    }

    /*
    Example template for a check for reaction function:
    private bool check<NameOfEffect>Reaction(ClickableTile tile, string damageType, string Source, TileEffect effectOnTile){
        switch(damageType){
            case "Fire":
                What to do during a reaction with a fire spell
                return true;

            case "Water":
                What to do during a reaction with a water spell
                return true;

            case "Earth":
                What to do during a reaction with an earth spell
                return true;

            case "Air":
                What to do during a reaction with an air spell
                return true;

            case "Lightning":
                What to do during a reaction with a lightning spell
                return true;

            case "Ice":
                What to do during a reaction with an ice spell
                return true;

            case "Plant":
                What to do during a reaction with a plant spell
                return true;
        }
        return true;
        }
    }
    The return statements are given to let the main function know if it should check for a reaction with the tile itself after dealing with the effects
    */


//-------------------------------------------------------------------REACTIONS WITH OTHER EFFECTS BELOW-------------------------------------------------------------
    private bool checkBurningReaction(ClickableTile tile, string damageType, string source, TileEffect effectOnTile, bool playerTeam){
        TileEffect newEffect;
        switch (damageType){
            case "Fire":
                effectOnTile.duration += 2;
                return false;

            case "Water":
                tile.removeEffectFromTile(effectOnTile, blockChange: true);
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                return false;

            case "Earth":
                return true;

            case "Air":
                return true;

            case "Lightning":
                return true;

            case "Ice":
                return true;

            case "Plant":
                return true;
        }
        return true;
    }

    private bool checkSoakedReaction(ClickableTile tile, string damageType, string source, TileEffect effectOnTile, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                /*
                tile.removeEffectFromTile(effectOnTile);
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true);
                */
                tile.removeEffectFromTile(effectOnTile, blockChange: true);
                if (tile.characterOnTile != null){
                    Debug.Log("removeEffectFromTile called at time: " + Time.time);
                }
                
                newEffect = Instantiate(tileEffects[5]);

                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                if (tile.characterOnTile != null){
                    Debug.Log("createTileEffect called at time: " + Time.time);
                }

                return false;

            case "Water":
                effectOnTile.duration += 2;
                return false;

            case "Earth":
                return true;

            case "Air":
                return true;

            case "Lightning":
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Electrified"){
                        tile.effectsOnTile[i].duration += 1;
                        return true;
                    }
                }
                newEffect = Instantiate(tileEffects[4]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
                joltCharacter(tile);
                return true;

            case "Ice":
                return true;

            case "Plant":
                return true;

            case "LightningStart":
                UnityEngine.Debug.Log("Electrify");
                TileMap map = tile.map;
                for (int i = -2; i <= 2; i++){
                    for (int j = -2; j <= 2; j++){
                        UnityEngine.Debug.Log("Checking Tile: (" + i + " , " + j + ")");
                        if (map.tileExists(tile.TileX + i, tile.TileY + j) && checkLightningSpread(tile.TileX + i, tile.TileY + j, tile.map) && !(i == 0 && j == 0)){
                            map.gameObject.GetComponent<ReactionController>().checkReaction(map.clickableTiles[tile.TileX + i, tile.TileY + j], "Lightning", source, playerTeam);
                        }
                    }
                }
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Electrified"){
                        tile.effectsOnTile[i].duration += 1;
                        return true;
                    }
                }
                newEffect = Instantiate(tileEffects[4]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
                joltCharacter(tile);
                return true;
        }
        return true;
    }

    private bool checkHearthfireReaction(ClickableTile tile, string damageType, string source, TileEffect effectOnTile, bool playerTeam){
        switch (damageType){
            case "Fire":
                effectOnTile.duration += 2;
                return false;

            case "Water":
                tile.removeEffectFromTile(effectOnTile, blockChange: true);
                return false;

            case "Earth":
                return true;

            case "Air":
                return true;

            case "Lightning":
                return true;

            case "Ice":
                return true;

            case "Plant":
                return true;
        }
        return true;
    }

private bool checkFoggyReaction(ClickableTile tile, string damageType, string source, TileEffect effectOnTile, bool playerTeam){
        TileEffect newEffect;
        switch (damageType){
            case "Fire":
                return true;

            case "Water":
                return true;

            case "Earth":
                return true;

            case "Air":
                return true;

            case "Lightning":
                return true;

            case "Ice":
                return true;

            case "Plant":
                return true;
        }
        return true;
    }


//------------------------------------------------------------------TILE REACTIONS BELOW-----------------------------------------------------------------------
    private void checkGrassReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                newEffect = Instantiate(tileEffects[0]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Water":
                newEffect = Instantiate(tileEffects[1]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Earth":
                newEffect = Instantiate(tileEffects[2]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration:4, fromReact: true, blockRemoval: true);
                break;

            case "HearthFire": //Need to do this last bit
                newEffect = Instantiate(tileEffects[3]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Air":
                break;

            case "LightningStart":
                break;

            case "Ice":
                break;

            case "Plant":
                break;

            case "Lightning":
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Electrified"){
                        tile.effectsOnTile[i].duration += 1;
                        break;
                    }
                }
                newEffect = Instantiate(tileEffects[4]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 1, fromReact: true, blockRemoval: true);
                joltCharacter(tile);
                break;

            case "Foggy":
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
        }
    }

    private void checkDirtReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                newEffect = Instantiate(tileEffects[0]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 1, fromReact: true, blockRemoval: true);
                break;

            case "Water":
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Rocky"){
                        tile.removeEffectFromTile(tile.effectsOnTile[i], blockChange: true);
                    }
                }
                ClickableTile newTile = tile.map.swapTiles(tile, 2, true);
                newEffect = Instantiate(tileEffects[1]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Earth":
                newEffect = Instantiate(tileEffects[2]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, fromReact: true, blockRemoval: true);
                break;
            
            case "HearthFire": //Need to do this last bit
                newEffect = Instantiate(tileEffects[3]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Air":
                break;

            case "Lightning":
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Electrified"){
                        tile.effectsOnTile[i].duration += 1;
                        break;
                    }
                }
                newEffect = Instantiate(tileEffects[4]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 1, fromReact: true, blockRemoval: true);
                joltCharacter(tile);
                break;

            case "Ice":
                break;

            case "Plant":
                break;        
            
            case "Foggy":
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
        }
    }

    private void checkMudReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                this.gameObject.GetComponent<TileMap>().swapTiles(tile, 1, true);
                break;

            case "Water":
                newEffect = Instantiate(tileEffects[1]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Earth":
                newEffect = Instantiate(tileEffects[2]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, fromReact: true, blockRemoval: true);
                break;

            case "HearthFire": //Need to do this last bit
                this.gameObject.GetComponent<TileMap>().swapTiles(tile, 1, true);
                break;

            case "Air":
                break;

            case "Lightning":
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Electrified"){
                        tile.effectsOnTile[i].duration += 1;
                        break;
                    }
                }
                newEffect = Instantiate(tileEffects[4]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
                joltCharacter(tile);
                break;

            case "Ice":
                break;

            case "Plant":
                break;

            case "LightningStart":
                UnityEngine.Debug.Log("Electrify");
                TileMap map = tile.map;
                for (int i = -1; i <= 1; i++){
                    for (int j = -1; j <= 1; j++){
                        UnityEngine.Debug.Log("Checking Tile: (" + i + " , " + j + ")");
                        if (map.tileExists(tile.TileX + i, tile.TileY + j) && checkLightningSpread(tile.TileX + i, tile.TileY + j, tile.map) && !(i == 0 && j == 0)){
                            map.gameObject.GetComponent<ReactionController>().checkReaction(map.clickableTiles[tile.TileX + i, tile.TileY + j], "Lightning", source, playerTeam);
                        }
                    }
                }
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Electrified"){
                        tile.effectsOnTile[i].duration += 1;
                        break;
                    }
                }
                newEffect = Instantiate(tileEffects[4]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
                joltCharacter(tile);
                break;

            case "Foggy":
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
        }
    }

    private void checkLightForestReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                newEffect = Instantiate(tileEffects[0]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Water":
                newEffect = Instantiate(tileEffects[1]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Earth":
                newEffect = Instantiate(tileEffects[2]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, fromReact: true, blockRemoval: true);
                break;

            case "HearthFire": //Need to do this last bit
                newEffect = Instantiate(tileEffects[3]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Air":
                break;

            case "Lightning":
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Electrified"){
                        tile.effectsOnTile[i].duration += 1;
                        break;
                    }
                }
                newEffect = Instantiate(tileEffects[4]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
                joltCharacter(tile);
                break;

            case "Ice":
                break;

            case "Plant":
                break;

            case "Foggy":
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
        }
    }

    private void checkDenseForestReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                newEffect = Instantiate(tileEffects[0]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3);
                break;

            case "Water":
                newEffect = Instantiate(tileEffects[1]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Earth":
                newEffect = Instantiate(tileEffects[2]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source);
                break;

            case "HearthFire": //Need to do this last bit
                newEffect = Instantiate(tileEffects[3]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true);
                break;

            case "Air":
                break;

            case "Lightning":
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Electrified"){
                        tile.effectsOnTile[i].duration += 1;
                        break;
                    }
                }
                newEffect = Instantiate(tileEffects[4]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
                joltCharacter(tile);
                break;

            case "Ice":
                break;

            case "Plant":
                break;

            case "Foggy":
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
        }
    }

    private void checkStoneReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                break;

            case "Water":
                newEffect = Instantiate(tileEffects[1]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Earth":
                newEffect = Instantiate(tileEffects[2]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, fromReact: true, blockRemoval: true);
                break;

            case "HearthFire": //Need to do this last bit
                newEffect = Instantiate(tileEffects[3]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Air":
                break;

            case "Lightning":
                break;

            case "Ice":
                break;

            case "Plant":
                break;

            case "Foggy":
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
        }
    }

    private void checkWoodPlankReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                newEffect = Instantiate(tileEffects[0]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Water":
                newEffect = Instantiate(tileEffects[1]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;

            case "Earth":
                break;

            case "HearthFire": //Need to do this last bit
                newEffect = Instantiate(tileEffects[3]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
                
            case "Air":
                break;

            case "Lightning":
                break;

            case "Ice":
                break;

            case "Plant":
                break;

            case "Foggy":
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
        }
    }

    private void checkDeepWaterReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
    TileEffect newEffect;
    switch(damageType){
        case "Fire":
            newEffect = Instantiate(tileEffects[5]);
            newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
            break;

        case "Water":
            break;

        case "Earth":
            break;

        case "HearthFire": //Need to do this last bit
            break;

        case "Air":
            break;

        case "Lightning":
            for (int i = 0; i < tile.effectsOnTile.Count; i++){
                if (tile.effectsOnTile[i].name == "Electrified"){
                    tile.effectsOnTile[i].duration += 1;
                    break;
                }
            }
            newEffect = Instantiate(tileEffects[4]);
            newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
            joltCharacter(tile);
            break;

        case "Ice":
            break;

        case "Plant":
            break;

        case "LightningStart":
            UnityEngine.Debug.Log("Electrify");
            TileMap map = tile.map;
            for (int i = -2; i <= 2; i++){
                for (int j = -2; j <= 2; j++){
                    UnityEngine.Debug.Log("Checking Tile: (" + i + " , " + j + ")");
                    if (map.tileExists(tile.TileX + i, tile.TileY + j) && checkLightningSpread(tile.TileX + i, tile.TileY + j, tile.map) && !(i == 0 && j == 0)){
                        map.gameObject.GetComponent<ReactionController>().checkReaction(map.clickableTiles[tile.TileX + i, tile.TileY + j], "Lightning", source, playerTeam);
                    }
                }
            }
            for (int i = 0; i < tile.effectsOnTile.Count; i++){
                if (tile.effectsOnTile[i].name == "Electrified"){
                    tile.effectsOnTile[i].duration += 1;
                    break;
                }
            }
            newEffect = Instantiate(tileEffects[4]);
            newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
            joltCharacter(tile);
            break;
        
            case "Foggy":
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
        }
    }

    private void checkShallowWaterReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
    TileEffect newEffect;
    switch(damageType){
        case "Fire":
            newEffect = Instantiate(tileEffects[5]);
            newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
            break;

        case "Water":
            break;

        case "Earth":
            break;

        case "HearthFire": //Need to do this last bit
            break;

        case "Air":
            break;

        case "Lightning":
            for (int i = 0; i < tile.effectsOnTile.Count; i++){
                if (tile.effectsOnTile[i].name == "Electrified"){
                    tile.effectsOnTile[i].duration += 1;
                    break;
                }
            }
            newEffect = Instantiate(tileEffects[4]);
            newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
            joltCharacter(tile);
            break;

        case "Ice":
            break;

        case "Plant":
            break; 

        case "LightningStart":
            UnityEngine.Debug.Log("Electrify");
            TileMap map = tile.map;
            for (int i = -2; i <= 2; i++){
                for (int j = -2; j <= 2; j++){
                    UnityEngine.Debug.Log("Checking Tile: (" + i + " , " + j + ")");
                    if (map.tileExists(tile.TileX + i, tile.TileY + j) && checkLightningSpread(tile.TileX + i, tile.TileY + j, tile.map) && !(i == 0 && j == 0)){
                        map.gameObject.GetComponent<ReactionController>().checkReaction(map.clickableTiles[tile.TileX + i, tile.TileY + j], "Lightning", source, playerTeam);
                    }
                }
            }
            for (int i = 0; i < tile.effectsOnTile.Count; i++){
                if (tile.effectsOnTile[i].name == "Electrified"){
                    tile.effectsOnTile[i].duration += 1;
                    break;
                }
            }
            newEffect = Instantiate(tileEffects[4]);
            newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 2, fromReact: true, blockRemoval: true);
            joltCharacter(tile);
            break;

        case "Foggy":
                newEffect = Instantiate(tileEffects[5]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3, fromReact: true, blockRemoval: true);
                break;
    }
}



    private bool checkLightningSpread(int x, int y, TileMap map){
        ClickableTile tileToCheck = map.clickableTiles[x, y];
        if (tileToCheck.gameObject.name.Contains("tileDeepWater") || tileToCheck.gameObject.name.Contains("tileShallowWater") || tileToCheck.gameObject.name.Contains("tileMud")){
            return true;
        }
        else if (tileToCheck.effectsOnTile != null){
            for (int i = 0; i < tileToCheck.effectsOnTile.Count; i++){
                if (tileToCheck.effectsOnTile[i].name == "Soaked"){
                    return true;
                }
            }
        }
        return false;
    }
    
    private void joltCharacter(ClickableTile tile){
        if (tile.characterOnTile != null){
            Basic_Character_Class character = tile.GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>();
            for (int i = 0; i < character.buffs.Count; i++){
                if (character.buffs[i].name == "Jolted"){
                    character.buffs[i].duration += 1;
                    return;
                }
            }
            BuffClass newBuff = Instantiate(joltedPrefab);
            newBuff.createBuff(true, character, 1, "ElectrifiedTile");
        }
    }
}
