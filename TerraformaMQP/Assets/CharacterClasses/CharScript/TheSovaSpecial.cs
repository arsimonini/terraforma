using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSovaSpecial : MonoBehaviour
{
    public int stacks = 0;
    public BuffClass asleepPrefab;

    public void smother(ClickableTile tile){
        tile.map.gameObject.GetComponent<ReactionController>().checkReaction(tile, "Smother", "Sova", true);
        stacks++;
        if (stacks > 5){
            stacks = 5;
        }
    }

    public void sleep(){
        BuffClass newBuff = Instantiate(asleepPrefab);
        newBuff.createBuff(false, gameObject.GetComponent<Basic_Character_Class>(), newDuration: 2, newSource: "Indomitable Rest");
        stacks = 0;
    }
}
