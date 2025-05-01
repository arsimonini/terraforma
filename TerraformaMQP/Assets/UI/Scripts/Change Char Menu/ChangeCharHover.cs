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

    public GameObject target;
    public Vector3 targetPosition;

    public void Start() {
        targetPosition = target.transform.position;
    }

    public void OnPointerEnter(PointerEventData data) 
    {
        checkHoverLocation(key);

    }

    public void OnPointerExit(PointerEventData data) {
        onPage.SetActive(false);
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
            onPage = chars[0];
            chars[0].SetActive(true);
            break;
        case "Wold Wold":
            onPage = chars[1];
            chars[1].SetActive(true);
            break;
        case "Althea Petrik":
            onPage = chars[2];
            chars[2].SetActive(true);
            break;
        case "Urson":
            onPage = chars[3];
            chars[3].SetActive(true);
            break;
        case "Sova":
            onPage = chars[4];
            chars[4].SetActive(true);
            break;
        case "Rold":
            onPage = chars[5];
            chars[5].SetActive(true);
            break;
        case "Hold":
            onPage = chars[6];
            chars[6].SetActive(true);
            break;
        case "Zuli":
            onPage = chars[7];
            chars[7].SetActive(true);
            break;
        case "Ruba":
            onPage = chars[8];
            chars[8].SetActive(true);
            break;
            
        case "Spell":
            onPage = chars[9];
            chars[9].SetActive(true);

            Transform nameObj = onPage.transform.Find("Name");
            Transform manaObj = onPage.transform.Find("ManaCost");
            Transform descObj = onPage.transform.Find("Description");
            Transform durationObj = onPage.transform.Find("Duration");
            Transform rangeObj = onPage.transform.Find("Range");

            //Need to get the info
            for(int i = 0; i < spells.Length; i++) {
                
                if(spells[i].spellName == spellKey) {
                    // UnityEngine.Debug.Log(spells[i].spellName);
                    // UnityEngine.Debug.Log(nameObj.GetComponent<TextMeshProUGUI>().text);
                    // UnityEngine.Debug.Log(manaObj.GetComponent<TextMeshProUGUI>().text);
                    // UnityEngine.Debug.Log(descObj.GetComponent<TextMeshProUGUI>().text);
                    //UnityEngine.Debug.Log(dura.transform.GetComponentInChildren<TextMeshProUGUI>().text);
                    nameObj.GetComponent<TextMeshProUGUI>().text = spells[i].spellName;
                    manaObj.GetComponent<TextMeshProUGUI>().text = spells[i].manaCost + " Mana";
                    descObj.GetComponent<TextMeshProUGUI>().text = spells[i].description;
                    rangeObj.GetComponent<TextMeshProUGUI>().text = "Range:" + spells[i].range + " Tiles";

                    if(spells[i].spellName == "Pirate's Eye" || spells[i].spellName == "Smooth Sailing" || spells[i].spellName == "Blue Fire" || spells[i].spellName == "Backfire" || spells[i].spellName == "Support Flames" || spells[i].spellName == "Fleet Footwork" || spells[i].spellName == "Stone Body") {
                        durationObj.GetComponent<TextMeshProUGUI>().text = "Duration: 3 Turns";
                    }
                    else {
                        durationObj.GetComponent<TextMeshProUGUI>().text = "Duration: Instant";
                    }

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