using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class BurningTileEffect : tileEffectActions
{
    public override void performEndOfTurnEffect(ClickableTile tile){
        UnityEngine.Debug.Log("Proccing Burning");
        if (tile.GetComponent<ClickableTile>().characterOnTile != null){
            tile.GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>().takeMagicDamage(2, "Fire");
        }
    }
}
