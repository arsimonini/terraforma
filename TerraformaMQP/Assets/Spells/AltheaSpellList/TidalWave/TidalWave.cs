using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalWave : MonoBehaviour, Cast_Spell
{
    public void castSpell(List<GameObject> targets, GameObject caster){
        Basic_Character_Class basicCaster = caster.GetComponent<Basic_Character_Class>();
        int targetx = 0; int targety = 0;
        ClickableTile ct = targets[0].GetComponent<ClickableTile>();
        TileMap map;
        int x = basicCaster.tile.TileX; int y = basicCaster.tile.TileY;
        if (ct != null) {
            targetx = ct.TileX;
            targety = ct.TileY;
            map = ct.map;
        } else {
            Basic_Character_Class basicTarget = targets[0].GetComponent<Basic_Character_Class>();
            targetx = basicTarget.tileX; 
            targety = basicTarget.tileY;
            map = basicTarget.tile.map;
        }
        
        int dir = findAttackDirection(basicCaster.tileX,basicCaster.tileY,targetx,targety);
        switch(dir){
            case 0:
                hitPos(x + 3, y, map, basicCaster, "Right");
                hitPos(x + 3, y + 1, map, basicCaster, "Right");
                hitPos(x + 3, y - 1, map, basicCaster, "Right");
                hitPos(x + 2, y, map, basicCaster, "Right");
                hitPos(x + 2, y + 1, map, basicCaster, "Right");
                hitPos(x + 2, y - 1, map, basicCaster, "Right");
                hitPos(x + 1, y, map, basicCaster, "Right");
                hitPos(x + 1, y + 1, map, basicCaster, "Right");
                hitPos(x + 1, y - 1, map, basicCaster, "Right");
                break;
            
            case 4:
                hitPos(x - 3, y, map, basicCaster, "Left");
                hitPos(x - 3, y + 1, map, basicCaster, "Left");
                hitPos(x - 3, y - 1, map, basicCaster, "Left");
                hitPos(x - 2, y, map, basicCaster, "Left");
                hitPos(x - 2, y + 1, map, basicCaster, "Left");
                hitPos(x - 2, y - 1, map, basicCaster, "Left");
                hitPos(x - 1, y, map, basicCaster, "Left");
                hitPos(x - 1, y + 1, map, basicCaster, "Left");
                hitPos(x - 1, y - 1, map, basicCaster, "Left");
                break;

            case 6:
                hitPos(x, y + 3, map, basicCaster, "Up");
                hitPos(x + 1, y + 3, map, basicCaster, "Up");
                hitPos(x - 1, y + 3, map, basicCaster, "Up");
                hitPos(x, y + 2, map, basicCaster, "Up");
                hitPos(x + 1, y + 2, map, basicCaster, "Up");
                hitPos(x - 1, y + 2, map, basicCaster, "Up");
                hitPos(x, y + 1, map, basicCaster, "Up");
                hitPos(x + 1, y + 1, map, basicCaster, "Up");
                hitPos(x - 1, y + 1, map, basicCaster, "Up");
                break;

            case 2:
                hitPos(x, y - 3, map, basicCaster, "Down");
                hitPos(x + 1, y - 3, map, basicCaster, "Down");
                hitPos(x - 1, y - 3, map, basicCaster, "Down");
                hitPos(x, y - 2, map, basicCaster, "Down");
                hitPos(x + 1, y - 2, map, basicCaster, "Down");
                hitPos(x - 1, y - 2, map, basicCaster, "Down");
                hitPos(x, y - 1, map, basicCaster, "Down");
                hitPos(x + 1, y - 1, map, basicCaster, "Down");
                hitPos(x - 1, y - 1, map, basicCaster, "Down");
                break;

            case 7:
                hitPos(x + 3, y + 3, map, basicCaster, "RightUp");
                hitPos(x + 3, y + 2, map, basicCaster, "Right");
                hitPos(x + 2, y + 3, map, basicCaster, "Up");
                hitPos(x + 2, y + 2, map, basicCaster, "RightUp");
                hitPos(x + 2, y + 1, map, basicCaster, "Right");
                hitPos(x + 1, y + 2, map, basicCaster, "Up");
                hitPos(x + 1, y + 1, map, basicCaster, "RightUp");
                hitPos(x + 1, y, map, basicCaster, "Right");
                hitPos(x, y + 1, map, basicCaster, "Up");
                break;
            
            case 1:
                hitPos(x + 3, y - 3, map, basicCaster, "RightDown");
                hitPos(x + 3, y - 2, map, basicCaster, "Right");
                hitPos(x + 2, y - 3, map, basicCaster, "Down");
                hitPos(x + 2, y - 2, map, basicCaster, "RightDown");
                hitPos(x + 2, y - 1, map, basicCaster, "Right");
                hitPos(x + 1, y - 2, map, basicCaster, "Down");
                hitPos(x + 1, y - 1, map, basicCaster, "RightDown");
                hitPos(x + 1, y, map, basicCaster, "Right");
                hitPos(x, y - 1, map, basicCaster, "Down"); 
                break;
            
            case 5:
                hitPos(x - 3, y + 3, map, basicCaster, "LeftUp");
                hitPos(x - 3, y + 2, map, basicCaster, "Left");
                hitPos(x - 2, y + 3, map, basicCaster, "Up");
                hitPos(x - 2, y + 2, map, basicCaster, "LeftUp");
                hitPos(x - 2, y + 1, map, basicCaster, "Left");
                hitPos(x - 1, y + 2, map, basicCaster, "Up");
                hitPos(x - 1, y + 1, map, basicCaster, "LeftUp");
                hitPos(x - 1, y, map, basicCaster, "Left");
                hitPos(x, y + 1, map, basicCaster, "Up");
                break;
            
            case 3:
                hitPos(x - 3, y - 3, map, basicCaster, "LeftDown");
                hitPos(x - 3, y - 2, map, basicCaster, "Left");
                hitPos(x - 2, y - 3, map, basicCaster, "Down");
                hitPos(x - 2, y - 2, map, basicCaster, "LeftDown");
                hitPos(x - 2, y - 1, map, basicCaster, "Left");
                hitPos(x - 1, y - 2, map, basicCaster, "Down");
                hitPos(x - 1, y - 1, map, basicCaster, "LeftDown");
                hitPos(x - 1, y, map, basicCaster, "Left");
                hitPos(x, y - 1, map, basicCaster, "Down");
                break; 

        }
    }

    public void hitPos(int x, int y, TileMap map, Basic_Character_Class caster, string pushDir){
        ClickableTile tile = map.clickableTiles[x, y];
        map.gameObject.GetComponent<ReactionController>().checkReaction(tile, caster.gameObject.GetComponent<Hero_Character_Class>().selectedSpell.elementType, caster.gameObject.GetComponent<Hero_Character_Class>().selectedSpell.spellName, true);
        if (tile.characterOnTile != null){
            Basic_Character_Class character = tile.characterOnTile.GetComponent<Basic_Character_Class>();
            map.pushCharacter(character, tile.TileX, tile.TileY, pushDir, 1);
            character.takeMagicDamage(caster.gameObject.GetComponent<Hero_Character_Class>().magic.moddedValue, caster.gameObject.GetComponent<Hero_Character_Class>().selectedSpell.elementType);
        }
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

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        List<GameObject> list = new List<GameObject>();
        //centerTile.canHit();   
        TileMap map = centerTile.map;

        int x = targetersTile.TileX;
        int y = targetersTile.TileY;
        
        int dir = findAttackDirection(targetersTile.TileX, targetersTile.TileY, centerTile.TileX, centerTile.TileY);

        switch (dir){
            case 0:
                hitTile(map, x + 1, y, list);
                hitTile(map, x + 1, y + 1, list);
                hitTile(map, x + 1, y - 1, list);
                hitTile(map, x + 2, y, list);
                hitTile(map, x + 2, y + 1, list);
                hitTile(map, x + 2, y - 1, list);                
                hitTile(map, x + 3, y, list);
                hitTile(map, x + 3, y + 1, list);
                hitTile(map, x + 3, y - 1, list);
                break;
            
            case 4:
                hitTile(map, x - 1, y, list);
                hitTile(map, x - 1, y + 1, list);
                hitTile(map, x - 1, y - 1, list);
                hitTile(map, x - 2, y, list);
                hitTile(map, x - 2, y + 1, list);
                hitTile(map, x - 2, y - 1, list);                
                hitTile(map, x - 3, y, list);
                hitTile(map, x - 3, y + 1, list);
                hitTile(map, x - 3, y - 1, list); 
                break;
            
            case 6:
                hitTile(map, x, y + 1, list);
                hitTile(map, x + 1, y + 1, list);
                hitTile(map, x - 1, y + 1, list);
                hitTile(map, x, y + 2, list);
                hitTile(map, x + 1, y + 2, list);
                hitTile(map, x - 1, y + 2, list);
                hitTile(map, x, y + 3, list);
                hitTile(map, x + 1, y + 3, list);
                hitTile(map, x - 1, y + 3, list);
                break;
            
            case 2:
                hitTile(map, x, y - 1, list);
                hitTile(map, x + 1, y - 1, list);
                hitTile(map, x - 1, y - 1, list);
                hitTile(map, x, y - 2, list);
                hitTile(map, x + 1, y - 2, list);
                hitTile(map, x - 1, y - 2, list);
                hitTile(map, x, y - 3, list);
                hitTile(map, x + 1, y - 3, list);
                hitTile(map, x - 1, y - 3, list);
                break;

            case 7:
                hitTile(map, x + 1, y, list);
                hitTile(map, x + 1, y + 1, list);
                hitTile(map, x, y + 1, list);
                hitTile(map, x + 2, y + 1, list);
                hitTile(map, x + 2, y + 2, list);
                hitTile(map, x + 1, y + 2, list);
                hitTile(map, x + 3, y + 2, list);
                hitTile(map, x + 2, y + 3, list);
                hitTile(map, x + 3, y + 3, list);
                break;

            case 1:
                hitTile(map, x + 1, y, list);
                hitTile(map, x + 1, y - 1, list);
                hitTile(map, x, y - 1, list);
                hitTile(map, x + 2, y - 1, list);
                hitTile(map, x + 2, y - 2, list);
                hitTile(map, x + 1, y - 2, list);
                hitTile(map, x + 3, y - 2, list);
                hitTile(map, x + 2, y - 3, list);
                hitTile(map, x + 3, y - 3, list);
                break;
            
            case 5: 
                hitTile(map, x - 1, y, list);
                hitTile(map, x, y + 1, list);
                hitTile(map, x - 1, y + 1, list);
                hitTile(map, x - 2, y + 1, list);
                hitTile(map, x - 1, y + 2, list);
                hitTile(map, x - 2, y + 2, list);
                hitTile(map, x - 3, y + 2, list);
                hitTile(map, x - 2, y + 3, list);
                hitTile(map, x - 3, y + 3, list);
                break;

            case 3:
                hitTile(map, x - 1, y, list);
                hitTile(map, x, y - 1, list);
                hitTile(map, x - 1, y - 1, list);
                hitTile(map, x - 2, y - 1, list);
                hitTile(map, x - 1, y - 2, list);
                hitTile(map, x - 2, y - 2, list);
                hitTile(map, x - 3, y - 2, list);
                hitTile(map, x - 2, y - 3, list);
                hitTile(map, x - 3, y - 3, list);
                break;
        }
        return list;
    }

    private void hitTile(TileMap map, int x, int y, List<GameObject> list){
        if (map.tileExists(x, y)){
            map.clickableTiles[x, y].canHit();
            list.Add(map.clickableTiles[x, y].gameObject);
        }
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        for (int i = 0; i < tiles.Count; i++){
            tiles[i].GetComponent<ClickableTile>().removeHighlight();
        }
    }
}
