using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Character_Class : MonoBehaviour
{

    public void takeTurn()
    {
        System.Random rand = new System.Random();
        UnityEngine.Debug.Log("Taking turn");
        this.gameObject.GetComponent<Unit>().map.MoveSelectedUnitTo(rand.Next(1, 9), rand.Next(1, 9));

    }

}
