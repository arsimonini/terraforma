using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrify : MonoBehaviour, Cast_Spell
{
    public void castSpell(List<GameObject> targets, GameObject caster){
        ClickableTile target = targets[0].GetComponent<ClickableTile>();
        target.map.gameObject.GetComponent<ReactionController>().checkReaction(target, caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, caster.GetComponent<Hero_Character_Class>().selectedSpell.spellName, true);
    }
}
