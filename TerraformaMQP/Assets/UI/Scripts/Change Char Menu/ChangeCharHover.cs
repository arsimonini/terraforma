using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeCharHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // Start is called before the first frame update
    [SerializeField]

    public string key;
    public GameObject[] chars;
    public Basic_Spell_Class[] spells;
    public GameObject onPage;
    public Transform parent;
    public string spellKey;


    public void OnPointerEnter(PointerEventData data) 
    {
        checkHoverLocation(key);

    }

    public void OnPointerExit(PointerEventData data) {
        Destroy(onPage);
    }

    public string checkHoverLocation(string str) {
        switch (str)
        {
        case "Hero1":
            checkHoverValue(TeamInfoManager.instance.hero1);
            break;
        case "Hero2":
            checkHoverValue(TeamInfoManager.instance.hero2);
            break;
        case "Hero3":
            checkHoverValue(TeamInfoManager.instance.hero3);
            break;
        case "Summon1_1":
            checkHoverValue(TeamInfoManager.instance.summon1_1);
            break;
        case "Summon1_2":
            checkHoverValue(TeamInfoManager.instance.summon1_2);
            break;
        case "Summon2_1":
            checkHoverValue(TeamInfoManager.instance.summon2_1);
            break;
        case "Summon2_2":
            checkHoverValue(TeamInfoManager.instance.summon2_2);
            break;
        case "Summon3_1":
            checkHoverValue(TeamInfoManager.instance.summon3_1);
            break;
        case "Summon3_2":
            checkHoverValue(TeamInfoManager.instance.summon3_2);
            break;
        case "Spell1_1":
            spellKey = TeamInfoManager.instance.spell1_1;
            checkHoverValue("Spell");
            break;
        case "Spell1_2":
            spellKey = TeamInfoManager.instance.spell1_2;
            checkHoverValue("Spell");
            break;
        case "Spell1_3":
            spellKey = TeamInfoManager.instance.spell1_3;
            checkHoverValue("Spell");
            break;
        case "Spell1_4":
            spellKey = TeamInfoManager.instance.spell1_4;
            checkHoverValue("Spell");
            break;
        case "Spell1_5":
            spellKey = TeamInfoManager.instance.spell1_5;
            checkHoverValue("Spell");
            break;
        case "Spell1_6":
            spellKey = TeamInfoManager.instance.spell1_6;
            checkHoverValue("Spell");
            break;
        case "Spell2_1":
            spellKey = TeamInfoManager.instance.spell2_1;
            checkHoverValue("Spell");
            break;
        case "Spell2_2":
            spellKey = TeamInfoManager.instance.spell2_2;
            checkHoverValue("Spell");
            break;
        case "Spell2_3":
            spellKey = TeamInfoManager.instance.spell2_3;
            checkHoverValue("Spell");
            break;
        case "Spell2_4":
            spellKey = TeamInfoManager.instance.spell2_4;
            checkHoverValue("Spell");
            break;
        case "Spell2_5":
            spellKey = TeamInfoManager.instance.spell2_5;
            checkHoverValue("Spell");
            break;
        case "Spell2_6":
            spellKey = TeamInfoManager.instance.spell2_6;
            checkHoverValue("Spell");
            break;
        case "Spell3_1":
            spellKey = TeamInfoManager.instance.spell3_1;
            checkHoverValue("Spell");
            break;
        case "Spell3_2":
            spellKey = TeamInfoManager.instance.spell3_2;
            checkHoverValue("Spell");
            break;
        case "Spell3_3":
            spellKey = TeamInfoManager.instance.spell3_3;
            checkHoverValue("Spell");
            break;
        case "Spell3_4":
            spellKey = TeamInfoManager.instance.spell3_4;
            checkHoverValue("Spell");
            break;
        case "Spell3_5":
            spellKey = TeamInfoManager.instance.spell3_5;
            checkHoverValue("Spell");
            break;
        case "Spell3_6":
            spellKey = TeamInfoManager.instance.spell3_6;
            checkHoverValue("Spell");
            break;
        default:
            //specText = "Shouldn't See This";
            break;
        }

        return "test";
        //return specText;
            
    }

    public string checkHoverValue(string str) {
        switch (str)
        {
        case "Lancin Bermane":
            onPage = Instantiate(chars[0], new Vector3(750.0f, 800.0f, 0.0f), Quaternion.identity, parent);
            break;
        case "Wold Wold":
            onPage = Instantiate(chars[1], new Vector3(750.0f, 800.0f, 0.0f), Quaternion.identity, parent);
            break;
        case "Althea Petrik":
            onPage = Instantiate(chars[2], new Vector3(750.0f, 800.0f, 0.0f), Quaternion.identity, parent);
            break;
        case "Urson":
            onPage = Instantiate(chars[3], new Vector3(750.0f, 800.0f, 0.0f), Quaternion.identity, parent);
            break;
        case "Sova":
            onPage = Instantiate(chars[4], new Vector3(750.0f, 800.0f, 0.0f), Quaternion.identity, parent);
            break;
        case "Rold":
            onPage = Instantiate(chars[5], new Vector3(750.0f, 800.0f, 0.0f), Quaternion.identity, parent);
            break;
        case "Hold":
            onPage = Instantiate(chars[6], new Vector3(750.0f, 800.0f, 0.0f), Quaternion.identity, parent);
            break;
        case "Zuli":
            onPage = Instantiate(chars[7], new Vector3(750.0f, 800.0f, 0.0f), Quaternion.identity, parent);
            break;
        case "Ruba":
            onPage = Instantiate(chars[8], new Vector3(750.0f, 800.0f, 0.0f), Quaternion.identity, parent);
            break;
            
        case "Spell":

            onPage = Instantiate(chars[9], new Vector3(750.0f, 600.0f, 0.0f), Quaternion.identity, parent);

            //Need to get the info
            for(int i = 0; i < spells.Length; i++) {
                if(spells[i].spellName == spellKey) {
                    onPage.transform.GetChild(0).Find("Description").GetComponent<TextMeshProUGUI>().text = spells[i].description;
                }
            }
            break;
        default:
            //specText = "Shouldn't See This";
            break;
        }
        return "Test";
        //return specText;
    }
}