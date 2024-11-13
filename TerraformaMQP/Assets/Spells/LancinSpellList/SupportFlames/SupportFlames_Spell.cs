using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportFlames_Spell : MonoBehaviour, Cast_Spell
{

    public void castSpell(List<GameObject> targets, GameObject caster){
        List<string> statsToEffect = new List<string>();
        statsToEffect.Add("attack");
        statsToEffect.Add("magic");
        statsToEffect.Add("speed");
        List<int> amountsToEffect = new List<int>();
        amountsToEffect.Add(2);
        amountsToEffect.Add(2);
        amountsToEffect.Add(2);
        for (int i = 0; i < targets.Count; i++){
            if (targets[i].GetComponent<ClickableTile>().characterOnTile != null){
                StatusEffect newEffect = new StatusEffect();
                newEffect.initializeStatusEffect(3, statsToEffect, "Support Flames", amountsToEffect, targets[i].GetComponent<ClickableTile>().characterOnTile, "Support Flames", true, targets[i].GetComponent<ClickableTile>().map.gameObject);
            }
        }
    }

}
