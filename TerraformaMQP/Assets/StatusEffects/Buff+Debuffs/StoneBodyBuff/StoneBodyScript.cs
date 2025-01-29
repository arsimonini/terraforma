using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBodyScript : BuffActions
{
    public GameObject magicAnimation;

    public override void endOfDurationEffect(Basic_Character_Class character){
        //character.takeMagicDamage(4, "Fire");

        //burst, hitting enemies to left, right, up, and down
        int revenge = (int) Mathf.Max(0,((float) character.startHP-character.health) / 1.2f);

        strikeAtPosition(character,1,0,revenge);
        strikeAtPosition(character,-1,0,revenge);
        strikeAtPosition(character,0,1,revenge);
        strikeAtPosition(character,0,-1,revenge);

        showAnimation(magicAnimation,character.transform.position.x,character.transform.position.y,character.transform.position.z);

        UnityEngine.Debug.Log("Stone Body Ended, attack strength" + revenge);
    }

    public void strikeAtPosition(Basic_Character_Class caster = null, int xOffset = 0, int yOffset = 0, int damage = 0) {
        if (caster == null) return;

        TileMap map = caster.map;
        
        
        //Find the tile at the right position and see if it's available. If not, return early.
        int trueX = caster.tileX + xOffset;
        int trueY = caster.tileY + yOffset;


        UnityEngine.Debug.Log(trueX+ ", "+ trueY);

        if ((trueX < 0) || (trueY < 0) || (trueX > map.mapSizeX) || (trueY > map.mapSizeY)) {
            return;
        } else {
            ClickableTile tile = map.clickableTiles[trueX,trueY];

            //check for a player sitting there
            if (tile.characterOnTile != null) {
                //UnityEngine.Debug.Log("Enemy Here!");
                Basic_Character_Class characterGettingHit = tile.characterOnTile.GetComponent<Basic_Character_Class>();
                if (characterGettingHit != null) {
                    characterGettingHit.takeMagicDamage(damage,"Earth");
                }
            } else {
                //UnityEngine.Debug.Log("No enemy here");
            }

            //affect the tiles with rocky terrain
            //placeTileEffect(tile,"Earth","Rock Spikes");
            
        }
    }

    public void showAnimation(GameObject anim, float x, float y, float z, float scale = 4) {
            GameObject animation = Instantiate(anim);
            animation.transform.position = new Vector3(x, y, z);
            animation.transform.localScale = animation.transform.localScale * scale;

            GameObject animation2 = Instantiate(anim);
            animation2.transform.position = new Vector3(x,y,z);
            animation2.transform.localScale = animation2.transform.localScale * scale;
            animation2.transform.Rotate(0.0f, 90.0f, 0.0f);
    }

}
