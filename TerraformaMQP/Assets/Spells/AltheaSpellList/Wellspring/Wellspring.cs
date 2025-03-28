using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wellspring : MonoBehaviour, Cast_Spell
{
    [SerializeField]
    //public BuffClass FleetFootworkBuff;
    public int healAmount = 3;

    public void castSpell(List<GameObject> targets, GameObject caster){
        //
        Basic_Character_Class bcc = caster.GetComponent<Basic_Character_Class>();
        Hero_Character_Class hcc = caster.GetComponent<Hero_Character_Class>();

        Basic_Character_Class tgt = targets[0].GetComponent<Basic_Character_Class>();
        //bcc.health = Mathf.
        tgt.health = Mathf.Min(tgt.health + healAmount,tgt.maxHealth.moddedValue);
    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        return null;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        return;
    }
}
