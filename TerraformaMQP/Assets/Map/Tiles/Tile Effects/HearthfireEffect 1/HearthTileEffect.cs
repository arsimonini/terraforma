using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class HearthTileEffect : tileEffectActions
{
    public List<string> burnableTiles;
    public List<string> reactableTiles;

    public int healCount= 2;
    public override void performEndOfTurnEffect(ClickableTile tile){
        GameObject character = tile.GetComponent<ClickableTile>().characterOnTile;
        if (character != null) {
            heal(character.GetComponent<Basic_Character_Class>(),healCount);
        }
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
        if (tile.gameObject.name.Contains("tileGrass")){
            tile.map.swapTiles(tile, 1, true);
        }
        else if (tile.gameObject.name.Contains("Forest")){
            tile.map.swapTiles(tile, 1, true);
        }
    }

    public override void performStepOnEffect(ClickableTile tile){
        GameObject character = tile.GetComponent<ClickableTile>().characterOnTile;
        if (character != null){
            heal(character.GetComponent<Basic_Character_Class>(),healCount);
        }
    }

    public void heal(Basic_Character_Class bcc, int heal) {
        bcc.health = Mathf.Min(bcc.health + heal,bcc.maxHealth.moddedValue);
    }

}
