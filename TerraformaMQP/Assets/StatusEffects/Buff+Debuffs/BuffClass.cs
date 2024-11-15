using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class BuffClass : ScriptableObject
{
    public List<string> statsToEffect;
    public List<int> amountsToEffect;
    public int duration;
    public string source;
    public string name;
    public bool playerTeam;
    public Basic_Character_Class character;
    public GameObject buffAsset;

    public bool reduceDuration(){
        if (duration > 1){
            duration--;
            buffAsset.GetComponent<BuffActions>().tickEffect(character);
            return true;
        }
        else{
            duration--;
            buffAsset.GetComponent<BuffActions>().endOfDurationEffect(character);
            return false;
        }
    }

    public void createBuff(bool newPlayerTeam, Basic_Character_Class newCharacter, int newDuration = 0, string newSource = null, string newName = null){
        if (newDuration != 0){
            duration = newDuration;
        }
        if (newSource != null){
            source = newSource;
        }
        if (newName != null){
            name = newName;
        }
        playerTeam = newPlayerTeam;
        character = newCharacter;
        UnityEngine.Debug.Log(character.name);
        if (playerTeam){
            character.tile.GetComponent<ClickableTile>().map.gameObject.GetComponent<BuffController>().addPlayerBuff(this);
        }
        else {
            character.tile.GetComponent<ClickableTile>().map.gameObject.GetComponent<BuffController>().addEnemyBuff(this);
        }
    }
}

public class BuffActions : MonoBehaviour
{
    public virtual void startOfDurationEffect(Basic_Character_Class character){
        UnityEngine.Debug.Log("Performing Start of Duration Effect");
    }

    public virtual void endOfDurationEffect(Basic_Character_Class character){
        UnityEngine.Debug.Log("Performing End of Duration Effect");
    }

    public virtual void tickEffect(Basic_Character_Class character){
        UnityEngine.Debug.Log("Performing Tick Effect");
    }

    public virtual void takeDamageEffect(Basic_Character_Class character){
        UnityEngine.Debug.Log("Performing Take Damage Effect");
    }
}
