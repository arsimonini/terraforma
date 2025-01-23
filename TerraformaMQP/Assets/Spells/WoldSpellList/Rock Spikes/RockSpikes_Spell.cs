using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class RockSpikes_Spell : MonoBehaviour, Cast_Spell
{
    public int spread;
    public TileEffect RockyTileEffect;

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
            ClickableTile ct = targets[0].GetComponent<ClickableTile>();

            //Decide attack direction (1 of 8)
            int dir = findAttackDirection(basicCaster.tileX,basicCaster.tileY,ct.TileX,ct.TileY);
            int dmg = 3;
            float acc = 1;
            string elm = "Earth";

            switch (dir) {
                case 0: 
                    strikeAtPosition(basicCaster,1,0,dmg,acc,elm);
                    strikeAtPosition(basicCaster,2,0,dmg,acc,elm);
                    strikeAtPosition(basicCaster,3,0,dmg,acc,elm);
                    strikeAtPosition(basicCaster,4,0,dmg,acc,elm);
                    strikeAtPosition(basicCaster,2,1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,2,-1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,3,1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,3,-1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,4,1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,4,-1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,4,2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,4,-2,dmg,acc,elm);
                break;

                case 1: 
                    strikeAtPosition(basicCaster,1,-1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,2,-2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,3,-3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,2,-3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,3,-2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,4,-3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,3,-4,dmg,acc,elm);
                break;
                
                case 2: 
                    strikeAtPosition(basicCaster,0,1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,0,2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,0,3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,0,4,dmg,acc,elm);
                    strikeAtPosition(basicCaster,1,2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-1,2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,1,3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-1,3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,1,4,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-1,4,dmg,acc,elm);
                    strikeAtPosition(basicCaster,2,4,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-2,4,dmg,acc,elm);
                break;
                
                case 3: 
                    strikeAtPosition(basicCaster,-1,-1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-2,-2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-3,-3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-2,-3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-3,-2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-4,-3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-3,-4,dmg,acc,elm);
                break;
                
                case 4: 
                    strikeAtPosition(basicCaster,-1,0,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-2,0,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-3,0,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-4,0,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-2,1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-2,-1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-3,1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-3,-1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-4,1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-4,-1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-4,2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-4,-2,dmg,acc,elm);
                break;
                
                case 5: 
                    strikeAtPosition(basicCaster,-1,1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-2,2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-3,3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-2,3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-3,2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-4,3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-3,4,dmg,acc,elm);
                break;
                
                case 6: 
                    strikeAtPosition(basicCaster,0,-1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,0,-2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,0,-3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,0,-4,dmg,acc,elm);
                    strikeAtPosition(basicCaster,1,-2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-1,-2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,1,-3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-1,-3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,1,-4,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-1,-4,dmg,acc,elm);
                    strikeAtPosition(basicCaster,2,-4,dmg,acc,elm);
                    strikeAtPosition(basicCaster,-2,-4,dmg,acc,elm);
                break;
                
                case 7:
                    strikeAtPosition(basicCaster,1,1,dmg,acc,elm);
                    strikeAtPosition(basicCaster,2,2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,3,3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,2,3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,3,2,dmg,acc,elm);
                    strikeAtPosition(basicCaster,4,3,dmg,acc,elm);
                    strikeAtPosition(basicCaster,3,4,dmg,acc,elm);
                break;
            }
        }
    }


    //Strikes at a position (x,y) relative to the caster. Deals d damage with accuracy a and leaves effect e.
    public void strikeAtPosition(Basic_Character_Class caster = null, int xOffset = 0, int yOffset = 0, int damage = 0, float accuracy = 0, string e = "none") {
        if (caster == null) return;

        TileMap map = caster.map;
        
        
        //Find the tile at the right position and see if it's available. If not, return early.
        int trueX = caster.tileX + xOffset;
        int trueY = caster.tileY + yOffset;

        if ((trueX < 0) || (trueY < 0) || (trueX > map.mapSizeX) || (trueY > map.mapSizeY)) {
            return;
        } else {
            ClickableTile tile = map.clickableTiles[trueX,trueY];

            //check for a player sitting there
            if (tile.characterOnTile != null) {
                UnityEngine.Debug.Log("Enemy Here!");
                Basic_Character_Class characterGettingHit = tile.characterOnTile.GetComponent<Basic_Character_Class>();
                if (characterGettingHit != null) {
                    characterGettingHit.takeMagicDamage(damage,"Earth");
                }
            } else {
                UnityEngine.Debug.Log("No enemy here");
            }

            //affect the tiles with rocky terrain

            
        }
    }

    //Finds the direction of the attack to be done. Has 8 directions.
    public int findAttackDirection(float x1, float x2, float y1, float y2) {
        //
        float xDelta = x2 - x1;
        float yDelta = y2 - y1;

        //As a simplification, any time that x or y is twice as great as another means that its just that axis. otherwise, it's a 
        if (xDelta > Mathf.Abs(2*yDelta)) { //East
            return 0;
        } else if (xDelta < -Mathf.Abs(2*yDelta)) {//West 
            return 4;
        } else if (yDelta > Mathf.Abs(2*xDelta)) { //North
            return 6;
        } else if (yDelta < -Mathf.Abs(2*xDelta)) { //South
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
        centerTile.canHit();
        list.Add(centerTile.gameObject);
        centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].canHit();
        list.Add(centerTile.map.clickableTiles[centerTile.TileX + 1, centerTile.TileY].gameObject);
        return list;
    }

    public void removeAOEDisplay (List<GameObject> tiles){
        for (int i = 0; i < tiles.Count; i++){
            tiles[i].GetComponent<ClickableTile>().removeHighlight();
        }
    }
}

  //                          hitCollider.gameObject.GetComponent<ClickableTile>().map.gameObject.GetComponent<ReactionController>().checkReaction(hitCollider.gameObject.GetComponent<ClickableTile>(), caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Fireball", true);