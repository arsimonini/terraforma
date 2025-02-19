using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class Hearthfire_Spell : MonoBehaviour, Cast_Spell
{
    public int spread;
    public TileEffect FireTileEffect;
    private LayerMask hittableTilesMask;
    private LayerMask wallsMask;

    public GameObject magicAnimation;

    void Start(){
        hittableTilesMask = LayerMask.GetMask("Default");
        wallsMask = LayerMask.GetMask("Block Visibility");
    }

    public void castSpell(List<GameObject> targets, GameObject caster)
    {
        Basic_Character_Class basicCaster = caster.GetComponent<Basic_Character_Class>();

        if (targets.Count > 0) {
            int endX = 0;
            int endY = 0;
            ClickableTile ct = targets[0].GetComponent<ClickableTile>();
            if (ct != null) { endX = ct.TileX; endY = ct.TileY;                
            } else { //It's likely a character
                Basic_Character_Class bct = targets[0].GetComponent<Basic_Character_Class>();
                if (bct != null) {endX = bct.tileX; endY = bct.tileY;}
            }

            TileMap map = basicCaster.map;
            
            ClickableTile a = map.getTile(endX,endY); if (a !=null) {placeTileEffect(a,"Hearthfire","Hearthfire");};
            ClickableTile b = map.getTile(endX,endY-1); if (b !=null) {placeTileEffect(b,"Hearthfire","Hearthfire");};
            ClickableTile c = map.getTile(endX,endY+1); if (c !=null) {placeTileEffect(c,"Hearthfire","Hearthfire");};
            ClickableTile d = map.getTile(endX-1,endY); if (d !=null) {placeTileEffect(d,"Hearthfire","Hearthfire");};
            ClickableTile e = map.getTile(endX+1,endY); if (e !=null) {placeTileEffect(e,"Hearthfire","Hearthfire");};
            //placeTileEffect(warpath[i],"Hearthfire","Hearthfire");

        }

    }

    

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        List<GameObject> list = new List<GameObject>();

        TileMap map = centerTile.map;

        int endX = centerTile.TileX;
        int endY = centerTile.TileY;
        
        if (map.inRange(endX,endY)) { lightTile(list, map, endX,endY); }
        if (map.inRange(endX,endY+1)) { lightTile(list, map, endX,endY+1); }
        if (map.inRange(endX,endY-1)) { lightTile(list, map, endX,endY-1); }
        if (map.inRange(endX+1,endY)) { lightTile(list, map, endX+1,endY); }
        if (map.inRange(endX-1,endY)) { lightTile(list, map, endX-1,endY); }

        
        return list;
    }

    public bool lightTile(List<GameObject> l, TileMap map, int xPos, int yPos) {
        if (xPos < 0 || yPos < 0 || xPos > map.mapSizeX || yPos > map.mapSizeY) {
            return false;
        }

        ClickableTile ct = map.clickableTiles[xPos,yPos];
        ct.canHit();
        //UnityEngine.Debug.Log("location: (" + (ct.TileX) + ", "+ (ct.TileY) + ")");
        l.Add(ct.gameObject);
        return true;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        for (int i = 0; i < tiles.Count; i++){
            tiles[i].GetComponent<ClickableTile>().removeHighlight();
        }
    }

    public bool placeTileEffect (ClickableTile ct, string element = "Earth", string spellName = "Rock Spikes", bool heroTeam = true) {
        if (ct == null) {
            return false;
        }
        TileMap map = ct.map; if (map == null) { return false;}
        ReactionController rc = map.GetComponent<ReactionController>(); if (rc == null) { return false;}
        rc.checkReaction(ct,element,spellName, heroTeam);

        return true;
    }
}