using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeismicStrike : ActiveAbility, Cast_Spell
{
    public void castSpell(List<GameObject> targets, GameObject caster){
        for (int i = 0; i < targets.Count; i++){
            targets[i].GetComponent<ClickableTile>().map.gameObject.GetComponent<ReactionController>().checkReaction(targets[i].GetComponent<ClickableTile>(), caster.GetComponent<SummonClass>().selectedAbility.elementType, caster.GetComponent<SummonClass>().selectedAbility.name, true);
            if (targets[i].GetComponent<ClickableTile>().characterOnTile != null && targets[i].GetComponent<ClickableTile>().characterOnTile.tag == "EnemyTeam"){
                targets[i].GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>().takeMagicDamage(caster.GetComponent<Basic_Character_Class>().attack.moddedValue, caster.GetComponent<SummonClass>().selectedAbility.elementType);
            }
        }
    }
}
