using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UnitModel : MonoBehaviour
{
    public GameObject Unit;

    //Recolors when mouse is hovering over a unit
    private void OnMouseEnter()
    {
        Unit.GetComponent<Unit>().mouseEnter();
    }

    //Resets when mouse has stopped hovering over a unit
    private void OnMouseExit()
    {
        Unit.GetComponent<Unit>().mouseExit();
    }

    private void OnMouseDown()
    {
        Unit.GetComponent<Hero_Character_Class>().mouseDown();
    }
}
