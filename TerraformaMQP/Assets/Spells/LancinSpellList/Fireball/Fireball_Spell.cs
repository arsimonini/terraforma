using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Spell : MonoBehaviour
{

    public void castSpell(GameObject target)
    {
        target.GetComponent<Basic_Character_Class>().takeMagicDamage(5, "Fire");
    }
}
