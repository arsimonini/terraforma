using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{

    public StatusEffectController statusEffectController;
    public List<GameObject> enemyTeamList;
    public List<GameObject> playerTeamList;
    public Camera camera;
    public int round = 0;
    public int phase = 0;  //0 for player phase, 1 for enemy effects, 2 for enemy phase, 3 for player effects
    public TileMap map;
    public bool movingEnemy = false;
    public int enemyCount = 0;
    public int enemiesToMove = 0;
    public bool lockPlayer = false;
    public bool targeting = false;
    public GameObject selectedCharacter;
    public Basic_Character_Class characterScript;
    public List<GameObject> targets = new List<GameObject>();

    void Update()
    {

        enemyTeamList.RemoveAll(x => !x);
        playerTeamList.RemoveAll(x => !x);

        if (selectedCharacter != null)
        {
            if (characterScript.targeting == true)
            {
                targeting = true;
            }
        }
        /*
        if (map.selectedUnit == null)
        {
            updateSelectedObject(null);
        }
        */
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (targeting == false)
                {
                    if (selectedCharacter == null)
                    {
                        if (hit.collider.gameObject.GetComponent<Basic_Character_Class>() != null && phase == 0 && hit.collider.gameObject.tag == "PlayerTeam")
                        {
                            updateSelectedObject(hit.collider.gameObject);
                            characterScript.selectCharacter();
                            if (characterScript.turnEnded == false)
                            {
                                map.updateSelectedCharacter(selectedCharacter);
                            }
                        }
                        else if (hit.collider.gameObject.GetComponent<Basic_Character_Class>() != null && hit.collider.gameObject.tag == "EnemyTeam" && phase == 0)
                        {
                            updateSelectedObject(hit.collider.gameObject);
                            characterScript.selectCharacter();
                        }
                    }
                    else if (selectedCharacter != null && hit.collider.gameObject.GetComponent<Basic_Character_Class>() != null && phase == 0)
                    {
                        characterScript.deselectCharacter();
                        map.updateSelectedCharacter(null);
                        map.currentPath = null;
                        updateSelectedObject(null);
                    } 
                    else if (selectedCharacter != null && (selectedCharacter.gameObject.tag == "EnemyTeam" || characterScript.turnEnded == true))
                    {
                        characterScript.deselectCharacter();
                        map.updateSelectedCharacter(null);
                        map.currentPath = null;
                        updateSelectedObject(null);
                    }
                    else if(selectedCharacter != null && hit.collider.gameObject.GetComponent<ClickableTile>() != null){
                        map.MoveSelectedUnitTo(hit.collider.gameObject.GetComponent<ClickableTile>().TileX, hit.collider.gameObject.GetComponent<ClickableTile>().TileY);
                    }
                }
                else if (targeting == true && hit.collider.gameObject.tag == "EnemyTeam" && characterScript.withinReach(hit.collider.gameObject) == true)
                {
                    if (characterScript.attackType == "Attack")
                    {
                        if (characterScript.attackCharacter(hit.collider.gameObject, characterScript.attack.moddedValue))
                        {
                            updateSelectedObject(null);
                            map.updateSelectedCharacter(null);
                        }
                    }
                    else if (characterScript.attackType == "Spell")
                    {
                        if (targets == null || selectedCharacter.GetComponent<Hero_Character_Class>().selectedSpell.amountOfTargets > targets.Count){
                            if (targets == null){
                                targets = new List<GameObject>();
                            }
                            if (!targets.Contains(hit.collider.gameObject) || selectedCharacter.GetComponent<Hero_Character_Class>().selectedSpell.requireDifferentTargets == false){
                                targets.Add(hit.collider.gameObject);
                            }
                            if (selectedCharacter.GetComponent<Hero_Character_Class>().selectedSpell.amountOfTargets == targets.Count){
                                if (characterScript.castSpell(targets))
                                {
                                    updateSelectedObject(null);
                                    map.updateSelectedCharacter(null);
                                    stopTargeting();
                                }
                            }
                        }

                    }
                    targeting = false;

                }
                else if (targeting == true)
                {
                    stopTargeting();
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && selectedCharacter != null && phase == 0)
        {
            characterScript.deselectCharacter();
            map.updateSelectedCharacter(null);
            map.currentPath = null;
            updateSelectedObject(null);
            stopTargeting();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && selectedCharacter != null && phase == 0 && targeting == true)
        {
            stopTargeting();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && selectedCharacter != null && phase == 0)
        {
            stopTargeting();
            characterScript.deselectCharacter();
            map.updateSelectedCharacter(null);
            map.currentPath = null;
            updateSelectedObject(null);
        }


        switch (phase)
        {
            case 0:
                enemyCount = enemyTeamList.Count;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    //End Turn Stuff
                    UnityEngine.Debug.Log("Switching to Phase 1");
                    phase++;
                    if (selectedCharacter != null)
                    {
                        UnityEngine.Debug.Log("Made it here");
                        characterScript.deselectCharacter();
                    }
                    stopTargeting();
                    map.updateSelectedCharacter(null);
                    updateSelectedObject(null);
                    endAllPlayerTurns();
                }
                break;

            case 1:
                playerTeamEndOfTurnTileEffects();
                statusEffectController.enemyTeamEffectsAdvance();
                UnityEngine.Debug.Log("Switching to Phase 2");
                enemyTeamStartOfTurnTileEffects();
                enemyTeamList.RemoveAll(x => !x);
                playerTeamList.RemoveAll(x => !x);
                phase++;

                break;
            
            case 2:
                enemyCount = enemyTeamList.Count;
                enemiesToMove = enemyCount;
                phase++;
                break;

            case 3:
                if (enemiesToMove > 0)
                {
                    if (movingEnemy == false)
                    {
                        map.updateSelectedCharacter(enemyTeamList[enemiesToMove - 1]);
                        enemyTeamList[enemiesToMove - 1].GetComponent<Enemy_Character_Class>().takeTurn();
                        movingEnemy = true;
                    }
                    else if (map.movingEnemy == false && movingEnemy == true)
                    {
                        movingEnemy = false;
                        enemiesToMove--;
                    }
                }
                else
                {
                    phase++;
                }
                break;

            case 4:
                enemyTeamEndOfTurnTileEffects();
                statusEffectController.playerTeamEffectsAdvance();
                map.updateSelectedCharacter(null);
                playerTeamStartOfTurnTileEffects();
                phase++;
                break;

            case 5:
                UnityEngine.Debug.Log("End of round " + round + ". Switching to phase 0");
                round++;
                phase = 0;
                resetPlayerTeamTurns();
                break;
        }
    }

    private void updateSelectedObject(GameObject newObject)
    {
        if (newObject != null)
        {
            selectedCharacter = newObject;
            characterScript = newObject.GetComponent<Basic_Character_Class>();
        }
        else
        {
            selectedCharacter = null;
            characterScript = null;
        }

    }

    private void resetPlayerTeamTurns()
    {
        for (int i = 0; i < playerTeamList.Count; i++)
        {
            playerTeamList[i].GetComponent<Basic_Character_Class>().resetTurn();
        }
    }

    private void endAllPlayerTurns()
    {
        for (int i = 0; i < playerTeamList.Count; i++)
        {
            playerTeamList[i].GetComponent<Basic_Character_Class>().endTurn();
        }
    }

    private void stopTargeting(){
        targeting = false;
        targets = null;
        if (selectedCharacter != null){
            characterScript.stopTargeting();
        }
    }

    private void playerTeamStartOfTurnTileEffects(){
        for (int i = 0; i < playerTeamList.Count; i++){
            if (playerTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile != null && playerTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile.Count != 0){
                for (int j = 0; j < playerTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile.Count; j++){
                    playerTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile[j].tileEffectPrefab.GetComponent<tileEffectActions>().performStartOfTurnEffect(playerTeamList[i].GetComponent<Basic_Character_Class>().tile);
                }
            }
        }
    }

    private void playerTeamEndOfTurnTileEffects(){
        for (int i = 0; i < playerTeamList.Count; i++){
            if (playerTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile != null && playerTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile.Count != 0){
                for (int j = 0; j < playerTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile.Count; j++){
                    playerTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile[j].tileEffectPrefab.GetComponent<tileEffectActions>().performEndOfTurnEffect(playerTeamList[i].GetComponent<Basic_Character_Class>().tile);
                }
            }
        }
    }

    private void enemyTeamStartOfTurnTileEffects(){
        for (int i = 0; i < enemyTeamList.Count; i++){
            if (enemyTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile != null && enemyTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile.Count != 0){
                for (int j = 0; j < enemyTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile.Count; j++){
                    enemyTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile[j].tileEffectPrefab.GetComponent<tileEffectActions>().performStartOfTurnEffect(enemyTeamList[i].GetComponent<Basic_Character_Class>().tile);
                }
            }
        }
    }

    private void enemyTeamEndOfTurnTileEffects(){
        for (int i = 0; i < enemyTeamList.Count; i++){
            if (enemyTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile != null && enemyTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile.Count != 0){
                for (int j = 0; j < enemyTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile.Count; j++){
                    enemyTeamList[i].GetComponent<Basic_Character_Class>().tile.effectsOnTile[j].tileEffectPrefab.GetComponent<tileEffectActions>().performEndOfTurnEffect(enemyTeamList[i].GetComponent<Basic_Character_Class>().tile);
                }
            }
        }
    }

}
