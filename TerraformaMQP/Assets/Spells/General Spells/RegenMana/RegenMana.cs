using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenMana : MonoBehaviour, Cast_Spell
{
    public void castSpell(List<GameObject> targets, GameObject caster){
        caster.GetComponent<Hero_Character_Class>().regenMana(4);
    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        List<GameObject> list = new List<GameObject>();
        return list;
    }

    public void removeAOEDisplay (List<GameObject> tiles){

    }
}
