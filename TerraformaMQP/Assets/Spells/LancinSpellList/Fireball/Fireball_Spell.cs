using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class Fireball_Spell : MonoBehaviour, Cast_Spell
{
    public int spread;
    public TileEffect BurningTileEffect;

    private LayerMask hittableTilesMask;
    private LayerMask wallsMask;

    public GameObject magicAnimation;

    void Start(){
        hittableTilesMask = LayerMask.GetMask("Default");
        wallsMask = LayerMask.GetMask("Block Visibility");
    }




    public void castSpell(List<GameObject> targets, GameObject caster)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            /*
            ClickableTile previousTile = targets[i].GetComponent<Basic_Character_Class>().tile;
            GameObject newTile = (GameObject)Instantiate(tileFireGrass, new Vector3(previousTile.gameObject.transform.position.x, 0, previousTile.gameObject.transform.position.z), Quaternion.identity);
            previousTile.map.swapTiles(previousTile, newTile.GetComponent<ClickableTile>(), 18);
            */
            //Used to test for casting AOE on tiles
            /*
            ClickableTile targetTile = targets[i].GetComponent<ClickableTile>();
            TileEffect newEffect = Instantiate(BurningTileEffect);
            newEffect.createTileEffect(true, targetTile, newSource: "Fireball", newDuration: 3);
            */
            /* Wall of Fire
            ClickableTile targetTile = targets[i].GetComponent<ClickableTile>();
            RaycastHit[] hits;
            hits = Physics.RaycastAll(caster.GetComponent<Basic_Character_Class>().tile.gameObject.transform.position, targetTile.gameObject.transform.position - caster.GetComponent<Basic_Character_Class>().tile.gameObject.transform.position, Vector3.Distance(targetTile.gameObject.transform.position, caster.GetComponent<Basic_Character_Class>().tile.gameObject.transform.position));
            for (int j = 0; j < hits.Length; j++){
                targetTile.map.swapTiles(hits[j].collider.gameObject.GetComponent<ClickableTile>(), 1);
            }
            */
            ClickableTile targetTile;
            if (targets[0].GetComponent<ClickableTile>()){
                targetTile = targets[0].GetComponent<ClickableTile>();
            }
            else{
                targetTile = targets[0].GetComponent<Basic_Character_Class>().tile;
            }

            if (targetTile.gameObject.tag != "Wall"){
                Collider[] hits = Physics.OverlapSphere(targetTile.gameObject.transform.position, 0.8f);

                foreach (var hitCollider in hits){
                    if (hitCollider.gameObject.name.StartsWith("tile")){
                        if (hitCollider.gameObject.tag != "Wall"){
                            if (hitCollider.gameObject.GetComponent<ClickableTile>().characterOnTile != null){
                                hitCollider.gameObject.GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>().takeMagicDamage(caster.GetComponent<Hero_Character_Class>().magic.moddedValue, "Fire");
                            }
                            hitCollider.gameObject.GetComponent<ClickableTile>().map.gameObject.GetComponent<ReactionController>().checkReaction(hitCollider.gameObject.GetComponent<ClickableTile>(), caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Fireball", true);
                            //GameObject animation = Instantiate(magicAnimation);
                            //animation.transform.position = new Vector3(hitCollider.gameObject.transform.position.x, hitCollider.gameObject.transform.position.y, hitCollider.gameObject.transform.position.z + 0.75f);
                        }
                    }
                }
            }
            else {
                targetTile = targetTile.map.shuntOver(targetTile, caster.GetComponent<Basic_Character_Class>().tile.GetComponent<ClickableTile>());
                Collider[] hits = Physics.OverlapSphere(targetTile.gameObject.transform.position, 0.8f);

                foreach (var hitCollider in hits){
                    if (hitCollider.gameObject.name.StartsWith("tile")){
                        if (hitCollider.gameObject.tag != "Wall"){
                            if (hitCollider.gameObject.GetComponent<ClickableTile>().characterOnTile != null){
                                hitCollider.gameObject.GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>().takeMagicDamage(caster.GetComponent<Hero_Character_Class>().magic.moddedValue, "Fire");
                            }
                            hitCollider.gameObject.GetComponent<ClickableTile>().map.gameObject.GetComponent<ReactionController>().checkReaction(hitCollider.gameObject.GetComponent<ClickableTile>(), caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Fireball", true);
                            //GameObject animation = Instantiate(magicAnimation);
                            //animation.transform.position = new Vector3(hitCollider.gameObject.transform.position.x, hitCollider.gameObject.transform.position.y, hitCollider.gameObject.transform.position.z + 0.75f);
                        }
                    }
                }
            }
            GameObject animation = Instantiate(magicAnimation);
            animation.transform.position = new Vector3(targetTile.gameObject.transform.position.x, targetTile.gameObject.transform.position.y + 0.75f, targetTile.gameObject.transform.position.z);
            animation.transform.localScale = animation.transform.localScale * 4;

            GameObject animation2 = Instantiate(magicAnimation);
            animation2.transform.position = new Vector3(targetTile.gameObject.transform.position.x, targetTile.gameObject.transform.position.y + 0.75f, targetTile.gameObject.transform.position.z);
            animation2.transform.localScale = animation2.transform.localScale * 4;
            animation2.transform.Rotate(0.0f, 90.0f, 0.0f);


            /*
             ClickableTile targetTile;
            //Normal Casting
            if (targets[i].GetComponent<Basic_Character_Class>()){
                targetTile = targets[i].GetComponent<Basic_Character_Class>().tile;
                targets[i].GetComponent<Basic_Character_Class>().takeMagicDamage(caster.GetComponent<Hero_Character_Class>().magic.moddedValue, "Fire");
            }
            else {
                targetTile = targets[i].GetComponent<ClickableTile>();
            }

            //TileEffect newEffect = Instantiate(BurningTileEffect);
            //newEffect.createTileEffect(true, targetTile, newSource: "Fireball", newDuration: 3);
            targetTile.map.gameObject.GetComponent<ReactionController>().checkReaction(targetTile, caster.GetComponent<Hero_Character_Class>().selectedSpell.elementType, "Fireball", true);
            //UnityEngine.Debug.Log(newEffect.duration);
            */
        }
    }
}
