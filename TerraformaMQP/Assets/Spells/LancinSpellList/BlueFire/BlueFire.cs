using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bluefire : MonoBehaviour, Cast_Spell
{
    [SerializeField]

    public void castSpell(List<GameObject> targets, GameObject caster){
        Lancin lancin = caster.GetComponent<Lancin>();
        if (lancin != null) {
            lancin.BlueFire = true;
        }
    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        return null;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        return;
    }
}
