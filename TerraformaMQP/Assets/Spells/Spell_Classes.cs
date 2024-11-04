using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class Basic_Spell_Class : ScriptableObject
{

    public int manaCost; //The mana cost of the spell
    public string spellName; //The name of the spell
    public string elementType; //The type of elemental damage the spell will deal
    public GameObject spellPrefab; //Reference to the spells prefab
    public string description; //A description of what the spell does
    public bool targeted; //If the spell needs to be targeted, true if it does, false if not
    public int range; //How far the spell can reach
    public bool targetTiles; //If the spell can target tiles, true if it can, false if not
    public bool targetAllies; //If the spell can target allies, true if it can, false if not
    public GameObject target;
    public int amountOfTargets; //The amount of targets the spell can hit
    public bool requireDifferentTargets; //If the spell needs to be directed towards different targets or can hit the same target multiple times, true if it needs different targets, false if not or the spell only needs one target
    public bool targetEnemies; //If the spell can target enemies, true if it can, false if not
    public bool hitOwnTile; //If the spell should hit the caster's tile if AOE, true if it should, false if not
    public bool hitSelf; //If the spell should be able to hith the caster, true if it should, false if not




}


//Interface that individual spells inherit to allow the castSpell function to be called
public interface Cast_Spell
{
    //Function that will be overridden by individual spells, takes in a list of GameObjects as the target and a GameObject as the caster
    public void castSpell(List<GameObject> targets, GameObject caster)
    {
        UnityEngine.Debug.Log("Cast Spell");
    }
}
