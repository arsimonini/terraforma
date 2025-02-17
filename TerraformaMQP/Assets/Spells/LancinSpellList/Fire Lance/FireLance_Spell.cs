using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class FireLance_Spell : MonoBehaviour, Cast_Spell
{
    public int spread;
    public TileEffect FireTileEffect;
    public List<ClickableTile> warpath= new List<ClickableTile>();

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


            warpath = new List<ClickableTile>();
            TileMap map = basicCaster.map;
            
            int startX = basicCaster.tileX;
            int startY = basicCaster.tileY;
            for (int i = 0; i < 6; i++) {
                int a = (i+1);
                int b = (6-i);
                int tweenX = (int) ((float) a*startX+ (float)b*endX)/7; //abbbbbb, aabbbbb, aaabbbb, aaaabbb, aaaaabb, aaaaaab
                int tweenY = (int) ((float) a*startY+ (float)b*endY)/7;
                warpath.Add(map.clickableTiles[tweenX,tweenY]);
            }
            warpath.Add(map.clickableTiles[endX,endY]);


            //Cull any duplicate values; will do this later

            for (int i = 0; i < warpath.Count; i++) {
                //UnityEngine.Debug.Log("FLAME SPEAR");
                placeTileEffect(warpath[i],"Fire","Fire Lance");
            }

            //Hit anyone standing on the last spot.
            UnityEngine.Debug.Log("Fire Lance End Point:" + warpath[warpath.Count - 1].TileX + ", " + warpath[warpath.Count - 1].TileY);
            
            if (warpath[warpath.Count-1].characterOnTile != null) {
                //Translate them to a basic_character_class
                Basic_Character_Class characterGettingHit = warpath[warpath.Count - 1].characterOnTile.GetComponent<Basic_Character_Class>();
                
                if (characterGettingHit != null) {
//                    UnityEngine.Debug.Log("HIT!");
                    characterGettingHit.takeMagicDamage(7,"Fire");
                }

            }
        }

    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        List<GameObject> list = new List<GameObject>();
        warpath = new List<ClickableTile>();

        TileMap map = centerTile.map;

        int endX = centerTile.TileX;
        int endY = centerTile.TileY;
        
        //Get Lancin's position
        int startX = targetersTile.TileX;
        int startY = targetersTile.TileY;

        //Find the maximum six points between Lancin and the target
        for (int i = 0; i < 6; i++) {
            int a = (i+1);
            int b = (6-i);

            int tweenX = (a*startX+b*endX)/7; //abbbbbb, aabbbbb, aaabbbb, aaaabbb, aaaaabb, aaaaaab
            int tweenY = (a*startY+b*endY)/7;

            //Add the tile to the posiiton
            warpath.Add(map.clickableTiles[tweenX,tweenY]);

            lightTile(list, map, tweenX,tweenY); 
            //centerTile tweenTile = map.clickableTiles[tweenX,tweenY];
            
            //list.Add(tweenTile.gameObject);
        }

        lightTile(list, map, endX,endY); 
        warpath.Add(map.clickableTiles[endX,endY]);
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