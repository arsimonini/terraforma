using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class Basic_Spell_Class : ScriptableObject
{

    public int manaCost;
    public string spellName;
    public string elementType;
    public GameObject spellPrefab;

}
