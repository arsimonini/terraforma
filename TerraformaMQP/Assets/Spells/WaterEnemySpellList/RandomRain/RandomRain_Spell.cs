using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRain_Spell : MonoBehaviour, Cast_Spell
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void castSpell(List<GameObject> targets, GameObject caster){
        int targetX = targets[0].GetComponent<ClickableTile>().TileX;
        int targetY = targets[0].GetComponent<ClickableTile>().TileY;
        int[] rainX = new int[8];
        int[] rainY = new int[8];

        int[] mapsize = caster.GetComponent<Basic_Character_Class>().map.getMapSize();
        int mapX = mapsize[0];
        int mapY = mapsize[1];

        for (int i = 0; i < 5; i++) {
            rainX[i] = Random.Range(targetX-2, targetX+3);
            rainY[i] = Random.Range(targetY-2, targetY+3);

            if (rainX[i] >= 0 && rainY[i] >=0 && rainX[i] < mapX && rainY[i] < mapY){

                ClickableTile target = caster.GetComponent<Basic_Character_Class>().map.clickableTiles[rainX[i], rainY[i]];

                if (target != null) {
                    caster.GetComponent<Basic_Character_Class>().map.gameObject.GetComponent<ReactionController>().checkReaction(target, "Water", "Random Rain", false);
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
