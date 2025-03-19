using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jolt : MonoBehaviour, Cast_Spell
{
    public BuffClass joltedPrefab;

    public void castSpell(List<GameObject> targets, GameObject caster){
        Basic_Character_Class target = targets[0].GetComponent<Basic_Character_Class>();
        target.takeMagicDamage(caster.GetComponent<Hero_Character_Class>().magic.moddedValue, "Lightning");
        for (int i = 0; i < target.buffs.Count; i++){
            if (target.buffs[i].name == "Jolted"){
                target.buffs[i].duration += 1;
                return;
            }
        }
        BuffClass newBuff = Instantiate(joltedPrefab);
        newBuff.createBuff(true, target, 1, "Jolt");
    }
}
