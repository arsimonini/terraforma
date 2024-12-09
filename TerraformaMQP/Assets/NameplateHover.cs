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
            specText = "The characters health";
            break;
        case "Mana":
            specText = "The characters mana";
            break;
        case "Mag":
            specText = "The characters magic";
            break;
        case "Atk":
            specText = "The characters attack";
            break;
        case "Def":
            specText = "The characters defense";
            break;
        case "Res":
            specText = "The characters resistance";
            break;
        case "Spd":
            specText = "The characters speed";
            break;
        case "Crit":
            specText = "The characters critical chance";
            break;
        case "Acc":
            specText = "The characters accuracy";
            break;
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