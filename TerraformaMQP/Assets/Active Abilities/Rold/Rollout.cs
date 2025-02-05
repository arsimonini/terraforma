using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rollout : ActiveAbility, Cast_Spell
{
    public BuffClass rolloutBuff;

    public void castSpell(List<GameObject> targets, GameObject caster){
        if (caster.GetComponent<Rold>().defenseStance){
            caster.GetComponent<Rold>().defenseStance = false;
            BuffClass newBuff = Instantiate(rolloutBuff);
            newBuff.createBuff(true, caster.GetComponent<Basic_Character_Class>());
        }
        else{
            caster.GetComponent<Rold>().defenseStance = true;
            caster.GetComponent<Basic_Character_Class>().removeBuff(caster.GetComponent<Basic_Character_Class>().buffs[caster.GetComponent<Basic_Character_Class>().buffs.FindIndex(a => a.name == "Rollout")]);
        }
    }
}
