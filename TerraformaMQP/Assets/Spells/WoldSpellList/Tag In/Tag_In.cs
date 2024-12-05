using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class Tag_In : MonoBehaviour, Cast_Spell
{
    public int spread;
    public TileEffect BurningTileEffect;

    private LayerMask hittableTilesMask;
    private LayerMask wallsMask;

    void Start(){
        hittableTilesMask = LayerMask.GetMask("Default");
        wallsMask = LayerMask.GetMask("Block Visibility");
    }




    public void castSpell(List<GameObject> targets, GameObject caster)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            Basic_Character_Class targetAlly = targets[0].GetComponent<Basic_Character_Class>();
            if ((targets[0].GetComponent<Enemy_Character_Class>() == null) && (targetAlly != null)) { //They are an ally (are not an enemy && are a player character)
                //Swaps locations
                Vector3 allyPos = targets[0].transform.position;
                int allyX = targetAlly.tileX;
                int allyY = targetAlly.tileY;
                ClickableTile allyTile = targetAlly.tile;

                Basic_Character_Class me = caster.GetComponent<Basic_Character_Class>();
                if (me != null) {
                    Vector3 myPos = caster.transform.position;
                    int myX = me.tileX;
                    int myY = me.tileY;
                    ClickableTile myTile = me.tile;

                    //Does the flip
                    caster.transform.position = allyPos;
                    me.tileX = allyX;
                    me.tileY = allyY;
                    me.tile = allyTile;
                    allyTile.characterOnTile = caster;

                    targetAlly.transform.position = myPos;
                    targetAlly.tileX = myX;
                    targetAlly.tileY = myY;
                    targetAlly.tile = myTile;
                    myTile.characterOnTile = targets[0];

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
