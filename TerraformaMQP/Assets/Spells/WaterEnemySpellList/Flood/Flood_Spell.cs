using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flood_Spell : MonoBehaviour, Cast_Spell
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void castSpell(List<GameObject> targets, GameObject caster){
        foreach (GameObject target in targets) {
            caster.GetComponent<Basic_Character_Class>().map.gameObject.GetComponent<ReactionController>().checkReaction(target.GetComponent<ClickableTile>(), "Water", "Random Rain", false);
            if (target.GetComponent<ClickableTile>().characterOnTile != null) {
                target.GetComponent<ClickableTile>().characterOnTile.GetComponent<Basic_Character_Class>().takeMagicDamage(5, "Water");

                //movement stuff here
            }
        }
    }
}
