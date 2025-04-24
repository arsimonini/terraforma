using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBall : MonoBehaviour, Cast_Spell
{
    [SerializeField]
    public BuffClass SolarBallBuff;

    public void castSpell(List<GameObject> targets, GameObject caster){
        //If we already got the buff, cut down on it immediately.
        Basic_Character_Class bCaster = caster.GetComponent<Basic_Character_Class>();

        if (bCaster.buffs.Count > 0){
            for (int i = 0; i < bCaster.buffs.Count; i++){
                if (bCaster.buffs[i].name == "Solar Ball"){
                    bCaster.buffs[i].duration = 1;
                    return;
                }
            }
        }

        BuffClass newBuff = Instantiate(SolarBallBuff);
        newBuff.createBuff(true, bCaster);

        Lancin lancin = caster.GetComponent<Lancin>();
        if (lancin != null) {
            if (targets.Count > 0) {
                ClickableTile ct = targets[0].GetComponent<ClickableTile>();
                if (ct != null) {
                    lancin.SolarBallX = ct.TileX;
                    lancin.SolarBallY = ct.TileY;
                } else {
                    Basic_Character_Class basicTarget = targets[0].GetComponent<Basic_Character_Class>();
                    lancin.SolarBallX = basicTarget.tileX; 
                    lancin.SolarBallY = basicTarget.tileY;
                }
            }
        }
    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        return null;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        return;
    }
}
