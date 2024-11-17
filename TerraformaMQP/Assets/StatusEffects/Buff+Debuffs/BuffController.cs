using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    [SerializeField]
    public List<BuffClass> playerTeamBuffs;
    public List<BuffClass> enemyTeamBuffs;

    public void playerTeamBuffsAdvance(){
        if (playerTeamBuffs != null){
            List<BuffClass> buffsToRemove = new List<BuffClass>();
            for (int i = 0; i < playerTeamBuffs.Count; i++){
                if (!playerTeamBuffs[i].reduceDuration()){
                    buffsToRemove.Add(playerTeamBuffs[i]);
                }
            }
            if (buffsToRemove != null){
                for (int i = 0; i < buffsToRemove.Count; i++){
                    playerTeamBuffs[i].character.removeBuff(playerTeamBuffs[i]);
                    playerTeamBuffs.Remove(buffsToRemove[i]);
                }
            }
        }
    }

    public void enemyTeamBuffsAdvance(){
        if (enemyTeamBuffs != null){
            List<BuffClass> buffsToRemove = new List<BuffClass>();
            for (int i = 0; i < enemyTeamBuffs.Count; i++){
                if (!enemyTeamBuffs[i].reduceDuration()){
                    buffsToRemove.Add(enemyTeamBuffs[i]);
                }
            }
            if (buffsToRemove != null){
                for (int i = 0; i < buffsToRemove.Count; i++){
                    enemyTeamBuffs[i].character.removeBuff(enemyTeamBuffs[i]);
                    enemyTeamBuffs.Remove(buffsToRemove[i]);
                }
            }
        }  
    }

    public void addPlayerBuff(BuffClass buff){
        playerTeamBuffs.Add(buff);
        buff.character.addBuff(buff);
        buff.buffAsset.GetComponent<BuffActions>().startOfDurationEffect(buff.character);
    }

    public void addEnemyBuff(BuffClass buff){
        enemyTeamBuffs.Add(buff);
        buff.character.addBuff(buff);
        buff.buffAsset.GetComponent<BuffActions>().startOfDurationEffect(buff.character);
    }
}
