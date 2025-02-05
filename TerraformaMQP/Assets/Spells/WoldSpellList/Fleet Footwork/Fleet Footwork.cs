using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleet_Footwork : MonoBehaviour, Cast_Spell
{
    [SerializeField]
    public BuffClass FleetFootworkBuff;
    //public int startHp;

    public void castSpell(List<GameObject> targets, GameObject caster){
        //Basic_Character_Class wold = caster.GetComponent<Basic_Character_Class>();
        //if (wold == null) {return null};
        if ((targets.Count) > 0 && (targets[0].GetComponent<Basic_Character_Class>() != null)) {
            Basic_Character_Class ally = targets[0].GetComponent<Basic_Character_Class>();

            for (int i = 0; i < ally.GetComponent<Basic_Character_Class>().buffs.Count; i++){
                if (ally.buffs[i].name == "Fleet Footwork"){
                    ally.buffs[i].duration += 1;
                    return;
                }
            }
        
            BuffClass newBuff = Instantiate(FleetFootworkBuff);
            newBuff.createBuff(true, ally);
        }



    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        return null;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        return;
    }
}
