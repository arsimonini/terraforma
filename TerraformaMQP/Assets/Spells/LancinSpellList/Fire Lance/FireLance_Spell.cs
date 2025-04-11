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


        float damage = .4375f;
        Hero_Character_Class heroCaster = caster.GetComponent<Hero_Character_Class>();
        if (heroCaster != null) {
            //UnityEngine.Debug.Log("Multiply!");
            damage = (damage* (heroCaster.magic.moddedValue));
        }

        if (targets.Count > 0) { 
            int endX = 0;
            int endY = 0;
            ClickableTile targetTile = targets[0].GetComponent<ClickableTile>();
            if (targetTile != null) { endX = targetTile.TileX; endY = targetTile.TileY;                
            } else { //It's likely a character
                Basic_Character_Class bct = targets[0].GetComponent<Basic_Character_Class>();
                if (bct != null) {endX = bct.tileX; endY = bct.tileY;
                targetTile = bct.tile;
                }
            }


            warpath = new List<ClickableTile>();
            TileMap map = basicCaster.map;
            float startX = basicCaster.transform.position.x;
            float startZ = basicCaster.transform.position.z;
            
            //UnityEngine.Debug.Log("Fire Lance Start Position" + startX.ToString() + ", " + startZ.ToString());
            float tileTx = targetTile.transform.position.x;
            float tileTy = targetTile.transform.position.y;
            float tileTz = targetTile.transform.position.z;

            Vector3 startPosition = new Vector3(startX, tileTy, startZ);
            Vector3 endPosition = new Vector3(tileTx, tileTy, tileTz);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(startPosition, (endPosition-startPosition).normalized, Vector3.Distance(endPosition,startPosition));
            UnityEngine.Debug.Log(hits);
            for (int i = 0; i < hits.Length; i++){
                GameObject res = hits[i].collider.gameObject;
                ClickableTile newCt = res.GetComponent<ClickableTile>();

                if (newCt != null) {
                    placeTileEffect(newCt,"Fire","Fire Lance");
                }
            }

            //Damage the foe at the target space
            if (map.clickableTiles[endX,endY].characterOnTile != null) {
                GameObject characterToStrike = map.clickableTiles[endX,endY].characterOnTile;

                Basic_Character_Class cts = characterToStrike.GetComponent<Basic_Character_Class>();
                if (cts != null) {
                    cts.takeMagicDamage((int) (heroCaster.magic.moddedValue/1.2), "Fire");
                }
            }

            
            
 //           warpath.Add(map.clickableTiles[endX,endY]);


            //Cull any duplicate values; will do this later

   //         for (int i = 0; i < warpath.Count; i++) {
                //UnityEngine.Debug.Log("FLAME SPEAR");
     //           placeTileEffect(warpath[i],"Fire","Fire Lance");
       //     }

            //Hit anyone standing on the last spot.
            //UnityEngine.Debug.Log("Fire Lance End Point:" + warpath[warpath.Count - 1].TileX + ", " + warpath[warpath.Count - 1].TileY);
            
            /*if (warpath[warpath.Count-1].characterOnTile != null) {
                //Translate them to a basic_character_class
                Basic_Character_Class characterGettingHit = warpath[warpath.Count - 1].characterOnTile.GetComponent<Basic_Character_Class>();
                
                if (characterGettingHit != null) {
//                    UnityEngine.Debug.Log("HIT!");
                    characterGettingHit.takeMagicDamage((int) damage,"Fire");
                }

            }*/
        }

    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        int endX = centerTile.TileX;
        int endY = centerTile.TileY;
        List<GameObject> list = new List<GameObject>();
        warpath = new List<ClickableTile>();

        TileMap map = centerTile.map;

        //Get Lancin's tile's position
        if (targetersTile == null) {return list;}
        Vector3 startPos = targetersTile.transform.position;
        Vector3 endPos = centerTile.transform.position;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(startPos, (endPos-startPos).normalized, Vector3.Distance(endPos,startPos));
        UnityEngine.Debug.Log(hits);
        
        for (int i = 0; i < hits.Length; i++){
            GameObject res = hits[i].collider.gameObject;
            ClickableTile newCt = res.GetComponent<ClickableTile>();

            if (newCt != null) {
                //placeTileEffect(newCt,"Fire","Fire Lance");
                list.Add(newCt.gameObject);
                lightTile(list,map,newCt.TileX,newCt.TileY);
            }
        }

       // lightTile(list,map,endX,endY);

        //int endX = centerTile.TileX;
        //int endY = centerTile.TileY;
        
        //Get Lancin's position
        //int startX = targetersTile.TileX;
        //int startY = targetersTile.TileY;

        //Find the maximum six points between Lancin and the target
        
        
        /*for (int i = 0; i < 6; i++) {
            int a = (i+1);
            int b = (6-i);

            int tweenX = (int) Mathf.Round((a*startX+b*endX)/7); //abbbbbb, aabbbbb, aaabbbb, aaaabbb, aaaaabb, aaaaaab
            int tweenY = (int) Mathf.Round((a*startY+b*endY)/7);

            //Add the tile to the position
            warpath.Add(map.clickableTiles[tweenX,tweenY]);

            lightTile(list, map, tweenX,tweenY); 
            //centerTile tweenTile = map.clickableTiles[tweenX,tweenY];
            
            //list.Add(tweenTile.gameObject);
        }*/

        //lightTile(list, map, endX,endY); 
        //warpath.Add(map.clickableTiles[endX,endY]);
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