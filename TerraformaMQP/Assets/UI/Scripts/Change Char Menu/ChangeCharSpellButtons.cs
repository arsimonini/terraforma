using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharSpellButtons : MonoBehaviour
{

    public string buttonClicked;
    public TeamInfo ti;
    public GameObject go;
    public GameObject thisGO;

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spellButtonClicked(string s) {
        buttonClicked = s;
        swapSpell(go.GetComponent<ChangeCharMenu>().currChanging, buttonClicked);
        
        
        Destroy(thisGO);
    }



    public void swapSpell(string place, string what) {
        
        switch (place) {
            case "Hero1_Spell1":
            ti.spell1_1 = what;
            break;

            case "Hero1_Spell2":
            ti.spell1_2 = what;
            break;

            case "Hero1_Spell3":
            ti.spell1_3 = what;
            break;

            case "Hero1_Spell4":
            ti.spell1_4 = what;
            break;

            case "Hero1_Spell5":
            ti.spell1_5 = what;
            break;

            case "Hero1_Spell6":
            ti.spell1_6 = what;
            break;
            
            case "Hero2_Spell1":
            ti.spell2_1 = what;
            break;

            case "Hero2_Spell2":
            ti.spell2_2 = what;
            break;

            case "Hero2_Spell3":
            ti.spell2_3 = what;
            break;

            case "Hero2_Spell4":
            ti.spell2_4 = what;
            break;

            case "Hero2_Spell5":
            ti.spell2_5 = what;
            break;

            case "Hero2_Spell6":
            ti.spell2_6 = what;
            break;

            case "Hero3_Spell1":
            ti.spell3_1 = what;
            break;

            case "Hero3_Spell2":
            ti.spell3_2 = what;
            break;

            case "Hero3_Spell3":
            ti.spell3_3 = what;
            break;

            case "Hero3_Spell4":
            ti.spell3_4 = what;
            break;

            case "Hero3_Spell5":
            ti.spell3_5 = what;
            break;

            case "Hero3_Spell6":
            ti.spell3_6 = what;
            break;
        }
    }

}
