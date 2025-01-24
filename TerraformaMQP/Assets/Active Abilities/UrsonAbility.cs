using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrsonAbility : ActiveAbility, Cast_Spell
{
    public void castSpell(List<GameObject> targets, GameObject caster){
        targets[0].GetComponent<Basic_Character_Class>().takeMagicDamage(10, "Fire");
    }
}
