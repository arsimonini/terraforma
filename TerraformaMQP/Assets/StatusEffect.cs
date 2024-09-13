using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : ScriptableObject
{
    public int duration;
    public List<string> statToEffect;
    public string source;
    public List<int> amount;
    public GameObject selectedObject;

    public void initializeStatusEffect(int newDuration, List<string> newStatToEffect, string newSource, List<int> newAmount, GameObject newSelectedObject)
    {
        duration = newDuration;
        statToEffect = newStatToEffect;
        source = newSource;
        amount = newAmount;
        selectedObject = newSelectedObject;
        selectedObject.GetComponent<Basic_Character_Class>().addStatus(this, true);

    }

    public void initializeTileEffect(List<string> newStatToEffect, string newSource, List<int> newAmount, GameObject newSelectedObject)
    {
        statToEffect = newStatToEffect;
        source = newSource;
        amount = newAmount;
        selectedObject = newSelectedObject;
        selectedObject.GetComponent<Basic_Character_Class>().addStatus(this, true);
    }

}
