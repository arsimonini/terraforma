using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void swapSpellSprite(int index, string spell) {
        Debug.Log(index);
        Debug.Log(spell);
        for(int i = 0; i < go.GetComponent<ChangeCharMenu>().spellSpriteKeys.Length; i++) {
            if(spell == go.GetComponent<ChangeCharMenu>().spellSpriteKeys[i]) {
                Debug.Log(spell);
                Debug.Log(go.GetComponent<ChangeCharMenu>().spellSpriteKeys[i]);
                //go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
                go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
            }
        }

    }



    public void swapSpell(string place, string what) {
        if(what == "Locked") {
            return;
        }
        switch (place) {
            case "Hero1_Spell1":
            ti.spell1_1 = what;
            swapSpellSprite(0, what);
            break;

            case "Hero1_Spell2":
            ti.spell1_2 = what;
            swapSpellSprite(1, what);
            break;

            case "Hero1_Spell3":
            ti.spell1_3 = what;
            swapSpellSprite(2, what);
            break;

            case "Hero1_Spell4":
            ti.spell1_4 = what;
            swapSpellSprite(3, what);
            break;

            case "Hero1_Spell5":
            ti.spell1_5 = what;
            swapSpellSprite(4, what);
            break;

            case "Hero1_Spell6":
            ti.spell1_6 = what;
            swapSpellSprite(5, what);
            break;
            
            case "Hero2_Spell1":
            ti.spell2_1 = what;
            swapSpellSprite(6, what);
            break;

            case "Hero2_Spell2":
            ti.spell2_2 = what;
            swapSpellSprite(7, what);
            break;

            case "Hero2_Spell3":
            ti.spell2_3 = what;
            swapSpellSprite(8, what);
            break;

            case "Hero2_Spell4":
            ti.spell2_4 = what;
            swapSpellSprite(9, what);
            break;

            case "Hero2_Spell5":
            ti.spell2_5 = what;
            swapSpellSprite(10, what);
            break;

            case "Hero2_Spell6":
            ti.spell2_6 = what;
            swapSpellSprite(11, what);
            break;

            case "Hero3_Spell1":
            ti.spell3_1 = what;
            swapSpellSprite(12, what);
            break;

            case "Hero3_Spell2":
            ti.spell3_2 = what;
            swapSpellSprite(13, what);
            break;

            case "Hero3_Spell3":
            ti.spell3_3 = what;
            swapSpellSprite(14, what);
            break;

            case "Hero3_Spell4":
            ti.spell3_4 = what;
            swapSpellSprite(15, what);
            break;

            case "Hero3_Spell5":
            ti.spell3_5 = what;
            swapSpellSprite(16, what);
            break;

            case "Hero3_Spell6":
            ti.spell3_6 = what;
            swapSpellSprite(17, what);
            break;
        }
    }

}
