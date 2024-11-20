using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flood_Spell : MonoBehaviour, Cast_Spell
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void castSpell(List<GameObject> targets, GameObject caster){
        //UnityEngine.Debug.Log("ENEMY CAST FLOOD - HERE");
        foreach (GameObject target in targets) {
            caster.GetComponent<Basic_Character_Class>().map.gameObject.GetComponent<ReactionController>().checkReaction(target.GetComponent<ClickableTile>(), "Water", "Flood", false);
            if (target.GetComponent<ClickableTile>().characterOnTile != null) {
                //UnityEngine.Debug.Log("FLOOD HITS " + target.GetComponent<ClickableTile>().characterOnTile.name);
                target.GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>().takeMagicDamage(4, "Water");

                //movement stuff here
                int targetX = target.GetComponent<ClickableTile>().TileX;
                int casterX = caster.GetComponent<Basic_Character_Class>().tileX;
                int targetY = target.GetComponent<ClickableTile>().TileY;
                int casterY = caster.GetComponent<Basic_Character_Class>().tileY;
                if (!(targetX == casterX && targetY == casterY)) {
                    //List<Node> path = new List<Node>();
                    //caster.GetComponent<Basic_Character_Class>().map.updateSelectedCharacter(target);
                    GameObject targetChar = target.GetComponent<ClickableTile>().characterOnTile;
                    //UnityEngine.Debug.Log("FLOOD PUSHES " + targetChar.name);
                    if (targetX > casterX) {
                        caster.GetComponent<Basic_Character_Class>().map.moveCharTo(targetChar, targetX+1, targetY);
                        //path = caster.GetComponent<Basic_Character_Class>().map.generatePathTo(targetX+1, targetY, startY: targetY);
                    }
                    else if (targetX < casterX) {
                        caster.GetComponent<Basic_Character_Class>().map.moveCharTo(targetChar, targetX-1, targetY);
                        //path = caster.GetComponent<Basic_Character_Class>().map.generatePathTo(targetX-1, targetY, startY: targetY);
                    }
                    else if (targetY > casterY) {
                        caster.GetComponent<Basic_Character_Class>().map.moveCharTo(targetChar, targetX, targetY+1);
                        //path = caster.GetComponent<Basic_Character_Class>().map.generatePathTo(targetX, targetY+1, startY: targetY);
                    }
                    else {
                        caster.GetComponent<Basic_Character_Class>().map.moveCharTo(targetChar, targetX, targetY-1);
                        //path = caster.GetComponent<Basic_Character_Class>().map.generatePathTo(targetX, targetY-1, startY: targetY);
                    }
                }
            }
        }
    }
}
