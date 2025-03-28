using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class Mist_Spell : MonoBehaviour, Cast_Spell
{
    public int spread;
    public TileEffect BurningTileEffect;

    private LayerMask hittableTilesMask;
    private LayerMask wallsMask;

    public GameObject magicAnimation;

    public int damageScale = 0;

    void Start(){
        hittableTilesMask = LayerMask.GetMask("Default");
        wallsMask = LayerMask.GetMask("Block Visibility");
    }




    public void castSpell(List<GameObject> targets, GameObject caster)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            ClickableTile targetTile;
            if (targets[0].GetComponent<ClickableTile>()){
                targetTile = targets[0].GetComponent<ClickableTile>();
            }
            else{
                targetTile = targets[0].GetComponent<Basic_Character_Class>().tile;
            }

            int x = targetTile.TileX; int y = targetTile.TileY;
            TileMap map = targetTile.map;
            //Strike in these 21 spots
            placeTileEffect(targetTile);
            placeTileEffect(map.getTile(x-1,y));
            placeTileEffect(map.getTile(x+1,y));
            placeTileEffect(map.getTile(x,y-1));
            placeTileEffect(map.getTile(x,y+1));
            placeTileEffect(map.getTile(x-2,y));
            placeTileEffect(map.getTile(x+2,y));
            placeTileEffect(map.getTile(x,y-2));
            placeTileEffect(map.getTile(x,y+2));
            placeTileEffect(map.getTile(x-1,y-1));
            placeTileEffect(map.getTile(x+1,y+1));
            placeTileEffect(map.getTile(x+1,y-1));
            placeTileEffect(map.getTile(x-1,y+1));

            GameObject animation = Instantiate(magicAnimation);
            animation.transform.position = new Vector3(targetTile.gameObject.transform.position.x, targetTile.gameObject.transform.position.y + 0.75f, targetTile.gameObject.transform.position.z);
            animation.transform.localScale = animation.transform.localScale * 4;

            GameObject animation2 = Instantiate(magicAnimation);
            animation2.transform.position = new Vector3(targetTile.gameObject.transform.position.x, targetTile.gameObject.transform.position.y + 0.75f, targetTile.gameObject.transform.position.z);
            animation2.transform.localScale = animation2.transform.localScale * 4;
            animation2.transform.Rotate(0.0f, 90.0f, 0.0f);
        }
    }

    public List<GameObject> displaySpecificAOE(string attackType, ClickableTile centerTile, int size = 0, bool square = false, ClickableTile targetersTile = null){
        TileMap map = centerTile.map;
        List<GameObject> tiles = new List<GameObject>();
        int cX = centerTile.TileX;
        int cY = centerTile.TileY;

        lightTile(tiles,map.getTile(cX,cY));
        lightTile(tiles,map.getTile(cX-1,cY));
        lightTile(tiles,map.getTile(cX+1,cY));
        lightTile(tiles,map.getTile(cX,cY+1));
        lightTile(tiles,map.getTile(cX,cY-1));
        lightTile(tiles,map.getTile(cX-1,cY-1));
        lightTile(tiles,map.getTile(cX-1,cY+1));
        lightTile(tiles,map.getTile(cX+1,cY-1));
        lightTile(tiles,map.getTile(cX+1,cY+1));
        lightTile(tiles,map.getTile(cX-2,cY));
        lightTile(tiles,map.getTile(cX+2,cY));
        lightTile(tiles,map.getTile(cX,cY-2));
        lightTile(tiles,map.getTile(cX,cY+2));

        //lightTile(tiles,map.getTile(cX-2,cY-1));
        //lightTile(tiles,map.getTile(cX-2,cY+1));
        //lightTile(tiles,map.getTile(cX+2,cY-1));
        //lightTile(tiles,map.getTile(cX+2,cY+1));
        //lightTile(tiles,map.getTile(cX-1,cY+2));
        //lightTile(tiles,map.getTile(cX+1,cY+2));
        //lightTile(tiles,map.getTile(cX-1,cY-2));
        //lightTile(tiles,map.getTile(cX+1,cY-2));

        return tiles;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        for (int i = 0; i < tiles.Count; i++){
            tiles[i].GetComponent<ClickableTile>().removeHighlight();
        }
    }

    public bool placeTileEffect (ClickableTile ct, string element = "Foggy", string spellName = "Mist", bool heroTeam = true) {
        if (ct == null) {
            return false;
        }
        TileMap map = ct.map; if (map == null) { return false;}
        ReactionController rc = map.GetComponent<ReactionController>(); if (rc == null) { return false;}
        rc.checkReaction(ct,element,spellName, heroTeam);

        return true;
    }

    public void lightTile(List<GameObject> l, ClickableTile ct) {
        if (ct == null) return;

        l.Add(ct.gameObject);
        ct.canHit();
    }
}
