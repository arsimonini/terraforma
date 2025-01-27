using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class NameplateEffect : ScriptableObject
{
    public string key;
    public string name;
    public string description;
    public int timeRemaining;
    public Sprite effectVis;
    public string type;
}
