using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndomitableRest : ActiveAbility, Cast_Spell
{
    public void castSpell(List<GameObject> targets, GameObject caster){
        Basic_Character_Class sova = caster.GetComponent<Basic_Character_Class>();
        TheSovaSpecial sovaSpecial = caster.GetComponent<TheSovaSpecial>();
        sova.increaseHealth(2 + (sovaSpecial.stacks * 2));
        sovaSpecial.sleep();
    }
}
