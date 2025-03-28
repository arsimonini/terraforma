using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constrict : ActiveAbility, Cast_Spell
{
    public BuffClass stunnedPrefab;
    public void castSpell(List<GameObject> targets, GameObject caster){
        Basic_Character_Class target = targets[0].GetComponent<Basic_Character_Class>();
        target.takePhysicalDamage(caster.GetComponent<Basic_Character_Class>().attack.moddedValue);
        for (int i = 0; i < target.buffs.Count; i++){
            if (target.buffs[i].name == "Stunned"){
                target.buffs[i].duration += 1;
                return;
            }
        }
        BuffClass newBuff = Instantiate(stunnedPrefab);
        newBuff.createBuff(true, target, 1, "Stunned");
    }
}
