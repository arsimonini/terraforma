using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBody : MonoBehaviour, Cast_Spell
{
    [SerializeField]
    public BuffClass StoneBodyBuff;

    public void castSpell(List<GameObject> targets, GameObject caster){
        //Basic_Character_Class wold = caster.GetComponent<Basic_Character_Class>();
        //if (wold == null) {return null};
        

        for (int i = 0; i < caster.GetComponent<Basic_Character_Class>().buffs.Count; i++){
            if (caster.GetComponent<Basic_Character_Class>().buffs[i].name == "Stone Body"){
                caster.GetComponent<Basic_Character_Class>().buffs[i].duration += 1;
                    return;
                }
            }
        
        BuffClass newBuff = Instantiate(StoneBodyBuff);
        newBuff.createBuff(true, caster.GetComponent<Basic_Character_Class>());
    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        return null;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        return;
    }
}
