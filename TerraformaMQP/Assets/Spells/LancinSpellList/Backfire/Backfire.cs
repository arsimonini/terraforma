using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backfire : MonoBehaviour, Cast_Spell
{
    [SerializeField]
    public BuffClass BackfireBuff;

    public void castSpell(List<GameObject> targets, GameObject caster){
        if (targets[0].GetComponent<Basic_Character_Class>().buffs.Count > 0){
            for (int i = 0; i < targets[0].GetComponent<Basic_Character_Class>().buffs.Count; i++){
                if (targets[0].GetComponent<Basic_Character_Class>().buffs[i].name == "Backfire"){
                    targets[0].GetComponent<Basic_Character_Class>().buffs[i].duration += 1;
                    return;
                }
            }
        }
        BuffClass newBuff = Instantiate(BackfireBuff);
        newBuff.createBuff(true, targets[0].GetComponent<Basic_Character_Class>());
    }
}
