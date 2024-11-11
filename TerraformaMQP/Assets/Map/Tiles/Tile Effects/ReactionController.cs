using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionController : MonoBehaviour
{

    public List<TileEffect> tileEffects;
    public List<GameObject> tilePrefabs;


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
                        //checkTile = checkFoggyReaction(tile, damageType, source, tile.effectsOnTile[i], playerTeam);
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
                }
            }
        }
        if (checkTile){
            switch (tile.gameObject.name){
                case "tileGrass":
                    checkGrassReaction(tile, damageType, source, playerTeam);
                    break;
                
                case "tileDirt":
                    checkDirtReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileMud":
                    checkMudReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileIce":
                    //checkIceReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileStone":
                    //checkStoneReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileWoodPlank":
                    //checkWoodPlankReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileDenseForest":
                    //checkDenseForestReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileLightForest":
                    //checkLightForestReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileShallowWater":
                    //checkShallowWaterReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileDeepWater":
                    //checkDeepWaterReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileSand":
                    //checkSandReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileGlass":
                    //checkGlassReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileMetal":
                    //checkMetalReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileAshen":
                    //checkAshenReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileMountain":
                    //checkMountainReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileHill":
                    //checkHillReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileWall":
                    //checkWallReaction(tile, damageType, source, playerTeam);
                    break;

                case "tileWhiteVoid":
                    //checkWhiteVoidReaction(tile, damageType, source, playerTeam);
                    break;
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
        switch (damageType){
            case "Fire":
                effectOnTile.duration += 2;
                return false;

            case "Water":
                tile.removeEffectFromTile(effectOnTile);
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
        switch(damageType){
            case "Fire":
                tile.removeEffectFromTile(effectOnTile);
                return false;

            case "Water":
                effectOnTile.duration += 2;
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



//------------------------------------------------------------------TILE REACTIONS BELOW-----------------------------------------------------------------------
    private void checkGrassReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                newEffect = Instantiate(tileEffects[0]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3);
                break;

            case "Water":
                newEffect = Instantiate(tileEffects[1]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3);
                break;

            case "Earth":
                newEffect = Instantiate(tileEffects[2]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source);
                break;

            case "Air":
                break;

            case "Lightning":
                break;

            case "Ice":
                break;

            case "Plant":
                break;
        }
    }

    private void checkDirtReaction(ClickableTile tile, string damageType, string source, bool playerTeam){
        TileEffect newEffect;
        switch(damageType){
            case "Fire":
                newEffect = Instantiate(tileEffects[0]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 1);
                break;

            case "Water":
                for (int i = 0; i < tile.effectsOnTile.Count; i++){
                    if (tile.effectsOnTile[i].name == "Rocky"){
                        tile.removeEffectFromTile(tile.effectsOnTile[i]);
                    }
                }
                ClickableTile newTile = tile.map.swapTiles(tile, 2, true);
                newEffect = Instantiate(tileEffects[1]);
                newEffect.createTileEffect(playerTeam, newTile, newSource: source, newDuration: 3);
                break;

            case "Earth":
                newEffect = Instantiate(tileEffects[2]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source);
                break;
                
            case "Air":
                break;

            case "Lightning":
                break;

            case "Ice":
                break;

            case "Plant":
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
                newEffect.createTileEffect(playerTeam, tile, newSource: source, newDuration: 3);
                break;

            case "Earth":
                newEffect = Instantiate(tileEffects[2]);
                newEffect.createTileEffect(playerTeam, tile, newSource: source);
                break;

            case "Air":
                break;

            case "Lightning":
                break;

            case "Ice":
                break;

            case "Plant":
                break;
        }
    }

}
