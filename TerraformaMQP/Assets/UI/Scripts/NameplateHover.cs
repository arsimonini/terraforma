using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameplateHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // Start is called before the first frame update
    [SerializeField]
    public Nameplate nameplate;
    public bool extraEffects;
    public GameObject smallDescriptor;
    public TextMeshProUGUI smallDescriptorText;
    public GameObject largeDescriptor;
    public TextMeshProUGUI largeDescriptorText;
    public string key;
    public int effectIndex; //-1 means that it doesn't need an index
    public string specText;


    public void turnOffDesc() {
        smallDescriptor.SetActive(false);
        largeDescriptor.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData data) 
    {
        extraEffects = exEffChecker();

        if(extraEffects == true) {
            smallDescriptor.SetActive(true);
            smallDescriptorText.text = getSpecializedText(key);
        }
        else if(extraEffects == false) {
            largeDescriptor.SetActive(true);
            largeDescriptorText.text = getSpecializedText(key);
        }
    }

    public void OnPointerExit(PointerEventData data) {
        largeDescriptor.SetActive(false);
        smallDescriptor.SetActive(false);
    }

    public string getSpecializedText(string str) {
        switch (str)
        {
        case "Health":
            specText = "Amount of Damage You Can Endure Before Defeated";
            break;
        case "Mana":
            specText = "Amount of Magic You Can Use";
            break;
        case "Mag":
            specText = "Potency of Spell Based Attacks";
            break;
        case "Atk":
            specText = "Potency of Phyisical Attack";
            break;
        case "Def":
            specText = "Endurance Against Physical Attacks";
            break;
        case "Res":
            specText = "Endurance Against Spell Based Attacks";
            break;
        case "Spd":
            specText = "Chance to Completely Dodge an Attack";
            break;
        case "Crit":
            specText = "Chance to Deal a Stronger Physical Attack";
            break;
        case "Acc":
            specText = "Chance to Hit with a Physical Attack";
            break;
            
        case "Effect":

            //Need to check which space it is

            //Need to get the info
            for(int i = 0; i < nameplate.effects.Count; i++) {
                if(nameplate.effects[i].key == nameplate.effectKeyNames[effectIndex]) {
                    specText = nameplate.effects[i].name + " - " + nameplate.effects[i].description;
                }
            }

            //specText = nameplate.effectKeyNames[0];
            break;
        // case "Effect2":
        //     specText = nameplate.effectKeyNames[1];
        //     break;
        // case "Effect3":
        //     specText = nameplate.effectKeyNames[2];
        //     break;
        // case "Effect4":
        //     specText = nameplate.effectKeyNames[3];
        //     break;
        // case "Effect5":
        //     specText = nameplate.effectKeyNames[4];
        //     break;
        // case "Effect6":
        //     specText = nameplate.effectKeyNames[5];
        //     break;
        // case "Effect7":
        //     specText = nameplate.effectKeyNames[6];
        //     break;
        // case "Effect8":
        //     specText = nameplate.effectKeyNames[7];
        //     break;
        // case "Effect9":
        //     specText = nameplate.effectKeyNames[8];
        //     break;
        // case "Effect10":
        //     specText = nameplate.effectKeyNames[9];
        //     break;
        // case "Effect11":
        //     specText = nameplate.effectKeyNames[10];
        //     break;
        // case "Effect12":
        //     specText = nameplate.effectKeyNames[11];
        //     break;
        // case "Effect13":
        //     specText = nameplate.effectKeyNames[12];
        //     break;
        // case "Effect14":
        //     specText = nameplate.effectKeyNames[13];
        //     break;
        // case "Effect15":
        //     specText = nameplate.effectKeyNames[14];
        //     break;
        // case "Effect16":
        //     specText = nameplate.effectKeyNames[15];
        //     break;
        // case "Effect17":
        //     specText = nameplate.effectKeyNames[16];
        //     break;
        // case "Effect18":
        //     specText = nameplate.effectKeyNames[17];
        //     break;
        // case "Effect19":
        //     specText = nameplate.effectKeyNames[18];
        //     break;
        // case "Effect20":
        //     specText = nameplate.effectKeyNames[19];
        //     break;
        // case "Effect21":
        //     specText = nameplate.effectKeyNames[20];
        //     break;
        default:
            specText = "Shouldn't See This";
            break;
        }

        return specText;
    }

    public bool exEffChecker() {
        return nameplate.exEffectOn;
    }
}