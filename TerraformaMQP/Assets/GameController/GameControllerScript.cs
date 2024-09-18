using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    private int enemyCount = 0;
    public bool lockPlayer = false;
    public GameObject selectedCharacter;

    void Update()
    {

        if (map.selectedUnit == null)
        {
            selectedCharacter = null;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.gameObject.GetComponent<UnitModel>() != null && phase == 0)
                {
                    selectedCharacter = hit.collider.gameObject;
                    selectedCharacter.GetComponent<UnitModel>().Unit.GetComponent<Basic_Character_Class>().displayStats();
                    selectedCharacter.GetComponent<UnitModel>().Unit.GetComponent<Unit>().charSelected = true;
                    selectedCharacter.GetComponent<UnitModel>().Unit.GetComponent<Unit>().renderer.material.color = Color.red;
                    map.selectedUnit = selectedCharacter.GetComponent<UnitModel>().Unit;
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && selectedCharacter != null && phase == 0)
        {
            selectedCharacter.GetComponent<UnitModel>().Unit.GetComponent<Unit>().charSelected = false;
            selectedCharacter.GetComponent<UnitModel>().Unit.GetComponent<Unit>().renderer.material.color = Color.white;
            map.selectedUnit = null;
            selectedCharacter = null;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && selectedCharacter != null && phase == 0)
        {
            selectedCharacter.GetComponent<UnitModel>().Unit.GetComponent<Unit>().charSelected = false;
            selectedCharacter.GetComponent<UnitModel>().Unit.GetComponent<Unit>().renderer.material.color = Color.white;
            map.selectedUnit = null;
            selectedCharacter = null;
        }


        switch (phase)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    //End Turn Stuff
                    UnityEngine.Debug.Log("Switching to Phase 1");
                    phase++;
                    map.selectedUnit = null;
                    selectedCharacter = null;
                }
                break;

            case 1:
                statusEffectController.playerTeamEffectsAdvance();
                UnityEngine.Debug.Log("Switching to Phase 2");
                phase++;
                enemyCount = enemyTeamList.Count;
                break;

            case 2:
                if (enemyCount > 0)
                {
                    if (movingEnemy == false)
                    {
                        map.selectedUnit = enemyTeamList[enemyCount - 1];
                        enemyTeamList[enemyCount - 1].GetComponent<Enemy_Character_Class>().takeTurn();
                        movingEnemy = true;
                    }
                    else if (map.movingEnemy == false && movingEnemy == true)
                    {
                        movingEnemy = false;
                        enemyCount--;
                    }
                }
                else
                {
                    phase++;
                }
                break;

            case 3:
                UnityEngine.Debug.Log("End of round " + round + ". Switching to phase 0");
                statusEffectController.enemyTeamEffectsAdvance();
                round++;
                phase = 0;
                break;
        }
    }

}
