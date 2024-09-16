using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{

    public StatusEffectController statusEffectController;
    public List<Basic_Character_Class> enemyTeamList;
    public List<Basic_Character_Class> playerTeamList;
    public int round = 0;
    public int phase = 0;  //0 for player phase, 1 for enemy effects, 2 for enemy phase, 3 for player effects
    int timer = 0;

    void Update()
    {
        switch (phase)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    //End Turn Stuff
                    UnityEngine.Debug.Log("Switching to Phase 1");
                    phase++;
                }
                break;

            case 1:
                statusEffectController.playerTeamEffectsAdvance();
                UnityEngine.Debug.Log("Switching to Phase 2");
                phase++;
                break;

            case 2:
                if (timer != 200)
                {
                    timer++;
                }
                else
                {
                    phase++;
                    timer = 0;
                    UnityEngine.Debug.Log("Switching to Phase 3");
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
