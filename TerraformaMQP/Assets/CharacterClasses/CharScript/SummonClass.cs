using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonClass : MonoBehaviour
{
    public List<Basic_Spell_Class> abilityList;
    public Basic_Spell_Class selectedAbility;
    public List<int> cooldowns;

    void Start(){
        for (int i = 0; i < abilityList.Count; i++){
            if (abilityList[i].spellPrefab.GetComponent<ActiveAbility>().cooldown != 99){
                cooldowns.Add(0);
            }
        }
    }

    public void useAbility(List<GameObject> targets){
        Basic_Spell_Class abilityInstance = Instantiate(selectedAbility);
        abilityInstance.spellPrefab.GetComponent<Cast_Spell>().castSpell(targets, this.gameObject);
        playAnimation();
        cooldowns[abilityList.IndexOf(selectedAbility)] = selectedAbility.spellPrefab.GetComponent<ActiveAbility>().cooldown;
        Destroy(this);
    }

    public bool offCooldown(int i){
        if (cooldowns[i] == 0){
            return true;
        }
        else {
            return false;
        }
    }

    public void reduceCooldowns(){
        for (int i = 0; i < cooldowns.Count; i++){
            if (cooldowns[i] != 0 && cooldowns[i] != 99){
                cooldowns[i]--;
            }
        }
    }

    public void playAnimation(){

    }

}
