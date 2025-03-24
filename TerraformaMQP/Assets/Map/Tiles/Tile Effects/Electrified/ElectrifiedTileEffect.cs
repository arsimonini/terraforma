using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class ElectrifiedTileEffect : tileEffectActions
{

    public BuffClass joltedPrefab;

    public override void performEndOfTurnEffect(ClickableTile tile){
        //GameObject character = tile.GetComponent<ClickableTile>().characterOnTile;
    }

    public override void performStartOfTurnEffect(ClickableTile tile){
        /*
        Basic_Character_Class character = tile.GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>();
        for (int i = 0; i < character.buffs.Count; i++){
            if (character.buffs[i].name == "Jolted"){
                character.buffs[i].duration += 1;
                return;
            }
        }
        BuffClass newBuff = Instantiate(joltedPrefab);
        newBuff.createBuff(true, character, 1, "ElectrifiedTile");
        */
    }

    /*
    public override void react(List<TileEffect> effectsOnTile, ClickableTile tile, TileEffect thisEffect){
        UnityEngine.Debug.Log("We Overriding with this one");
        if (effectsOnTile != null){
            for (int i = 0; i < effectsOnTile.Count; i++){
                switch (effectsOnTile[i].name){
                    case "Soaked":
                        tile.removeEffectFromTile(effectsOnTile[i]);
                        tile.removeEffectFromTile(thisEffect);
                        break;
                }
            }
        }
        if (tile.effectsOnTile.Contains(thisEffect)){
            switch(tile.gameObject.name){
                case "tileMud":
                    UnityEngine.Debug.Log("Hit deep water");
                    break;
            }
        }
    }
    */

    public override void endOfDurationEffect(ClickableTile tile){
        /*
        if (tile.GetComponent<ClickableTile>().characterOnTile != null){
            Basic_Character_Class character = tile.GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>();
            for (int i = 0; i < character.buffs.Count; i++){
                if (character.buffs[i].name == "Jolted"){
                    character.buffs[i].duration += 1;
                    return;
                }
            }
            BuffClass newBuff = Instantiate(joltedPrefab);
            newBuff.createBuff(true, character, 1, "ElectrifiedTile");
        }
        */
        
    }

    public override void performStepOnEffect(ClickableTile tile){
        if (tile.GetComponent<ClickableTile>().characterOnTile != null){
            Basic_Character_Class character = tile.GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>();
            for (int i = 0; i < character.buffs.Count; i++){
                if (character.buffs[i].name == "Jolted"){
                    return;
                }
            }
            BuffClass newBuff = Instantiate(joltedPrefab);
            newBuff.createBuff(true, character, 1, "ElectrifiedTile");
        }
    }

}
