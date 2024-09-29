using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class Fireball_Spell : MonoBehaviour, Cast_Spell
{
    public int spread;
    public GameObject tileFireGrass;

    public void castSpell(List<GameObject> targets, GameObject caster)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            ClickableTile previousTile = targets[i].GetComponent<Basic_Character_Class>().tile;
            GameObject newTile = (GameObject)Instantiate(tileFireGrass, new Vector3(previousTile.gameObject.transform.position.x, 0, previousTile.gameObject.transform.position.z), Quaternion.identity);
            newTile.GetComponent<ClickableTile>().characterOnTile = previousTile.characterOnTile;
            newTile.GetComponent<ClickableTile>().map = previousTile.map;
            newTile.GetComponent<ClickableTile>().TileX = previousTile.TileX;
            newTile.GetComponent<ClickableTile>().TileY = previousTile.TileY;
            previousTile.map.tiles[previousTile.TileX, previousTile.TileY] = 18;
            previousTile.map.clickableTiles[previousTile.TileX, previousTile.TileY] = tileFireGrass.GetComponent<ClickableTile>();
            
            
            Destroy(previousTile.gameObject);
            targets[i].GetComponent<Basic_Character_Class>().tile = newTile.GetComponent<ClickableTile>();
            targets[i].GetComponent<Basic_Character_Class>().removeStatus(targets[i].GetComponent<Basic_Character_Class>().tileEffect, true);
            StatusEffect newEffect = new StatusEffect();
            newEffect.initializeTileEffect(newTile.GetComponent<ClickableTile>().statsToEffect, newTile.GetComponent<ClickableTile>().name, newTile.GetComponent<ClickableTile>().effectAmounts, targets[i], newTile.GetComponent<ClickableTile>().name + "Effect");
            targets[i].GetComponent<Basic_Character_Class>().tileType = newTile.GetComponent<ClickableTile>().map.tileTypes[18];
            targets[i].GetComponent<Basic_Character_Class>().takeMagicDamage(caster.GetComponent<Hero_Character_Class>().magic.moddedValue, "Fire");

        }
    }
}
