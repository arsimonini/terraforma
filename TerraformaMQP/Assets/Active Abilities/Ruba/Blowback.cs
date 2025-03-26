using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowback : ActiveAbility, Cast_Spell
{
    public void castSpell(List<GameObject> targets, GameObject caster){
        Basic_Character_Class target = targets[0].GetComponent<Basic_Character_Class>();
        Basic_Character_Class casterB = caster.GetComponent<Basic_Character_Class>();
        TileMap map = target.tile.map;
        int dir = findAttackDirection(casterB.tileX, casterB.tileY, target.tileX, target.tileY);
        switch (dir){
            case 0:
                map.pushCharacter(target, target.tileX, target.tileY, "Right", 2);
                map.pushCharacter(casterB, casterB.tileX, casterB.tileY, "Left", 2);
                break;
            
            case 4:
                map.pushCharacter(target, target.tileX, target.tileY, "Left", 2);
                map.pushCharacter(casterB, casterB.tileX, casterB.tileY, "Right", 2);
                break;
            
            case 6:
                map.pushCharacter(target, target.tileX, target.tileY, "Up", 2);
                map.pushCharacter(casterB, casterB.tileX, casterB.tileY, "Down", 2);
                break;
            
            case 2:
                map.pushCharacter(target, target.tileX, target.tileY, "Down", 2);
                map.pushCharacter(casterB, casterB.tileX, casterB.tileY, "Up", 2);
                break;
            
            case 7:
                map.pushCharacter(target, target.tileX, target.tileY, "RightUp", 2);
                map.pushCharacter(casterB, casterB.tileX, casterB.tileY, "LeftDown", 2);
                break;
            
            case 1:
                map.pushCharacter(target, target.tileX, target.tileY, "RightDown", 2);
                map.pushCharacter(casterB, casterB.tileX, casterB.tileY, "LeftUp", 2);
                break;
            
            case 5:
                map.pushCharacter(target, target.tileX, target.tileY, "LeftUp", 2);
                map.pushCharacter(casterB, casterB.tileX, casterB.tileY, "RightDown", 2);
                break;
            
            case 3:
                map.pushCharacter(target, target.tileX, target.tileY, "LeftDown", 2);
                map.pushCharacter(casterB, casterB.tileX, casterB.tileY, "RightUp", 2);
                break;  
        }
        target.takePhysicalDamage(casterB.attack.moddedValue);
    }

    public int findAttackDirection(float x1, float y1, float x2, float y2) {
        float xDelta = x2 - x1;
        float yDelta = y2 - y1;

        UnityEngine.Debug.Log("Deltas:" + xDelta+ ", "+ yDelta);
        //As a simplification, any time that x or y is twice as great as another means that its just that axis. otherwise, it's a 
        if (xDelta >= Mathf.Abs(2*yDelta)) { //East
            return 0;
        } else if (xDelta <= -Mathf.Abs(2*yDelta)) {//West 
            return 4;
        } else if (yDelta >= Mathf.Abs(2*xDelta)) { //North
            return 6;
        } else if (yDelta <= -Mathf.Abs(2*xDelta)) { //South
            return 2;
        } else {
            //NE
            if ((xDelta > 0) && (yDelta > 0)) {
                return 7;
            }
            //SE
            else if ((xDelta > 0) && (yDelta < 0)) {
                return 1;
            }
            //NW
            else if ((xDelta < 0) && (yDelta > 0)) {
                return 5;
            }
            //SW
            else if ((xDelta < 0) && (yDelta < 0)) {
                return 3;
            }
        }

        return 0; //defaults to east
    }
}
