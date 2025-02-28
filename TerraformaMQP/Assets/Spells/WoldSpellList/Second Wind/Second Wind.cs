using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Second_Wind : MonoBehaviour, Cast_Spell
{
    [SerializeField]
    //public BuffClass FleetFootworkBuff;
    public int healAmount = 5;

    public void castSpell(List<GameObject> targets, GameObject caster){
        //
        Basic_Character_Class bcc = caster.GetComponent<Basic_Character_Class>();
        Hero_Character_Class hcc = caster.GetComponent<Hero_Character_Class>();

        //bcc.health = Mathf.
        bcc.health = Mathf.Min(bcc.health + healAmount,bcc.maxHealth.moddedValue);
    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        return null;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        return;
    }
}
