using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // Start is called before the first frame update
    [SerializeField]
    //public GameObject textBox;
    //public TextMeshProUGUI descrip;
    //public Hero_Character_Class hcc;
    //public int spellNum;

    public GameObject UISpellDesc = null;
    public GameObject spell = null;
    public Transform currPos;
    public int spellNum;

    public TextMeshProUGUI SN = null;
    public TextMeshProUGUI MC = null;
    public TextMeshProUGUI SD = null;

    public bool displayingDescription = false;

    public bool hasSpell;
    public float spellOffset;


    void Start()
    {
        //UISpellDesc.GetComponent<Canvas>().worldCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        UISpellDesc.GetComponent<Canvas>().GetComponent<Billboard>().cam = GameObject.Find("UI Camera").GetComponent<Camera>().transform;
        SN.text = GetComponentInParent<Hero_Character_Class>().spellList[spellNum].spellName;
        MC.text = GetComponentInParent<Hero_Character_Class>().spellList[spellNum].manaCost.ToString();
        SD.text = GetComponentInParent<Hero_Character_Class>().spellList[spellNum].description;
    }

    public void Update() 
    {
        if(GetComponentInParent<Basic_Character_Class>().spellListIsUp == true && Input.GetKeyUp(KeyCode.I)) {
            if(displayingDescription == true) {
                SN.enabled = true;
                MC.enabled = true;
                SD.enabled = false;
                displayingDescription = false;
            }
            else if(displayingDescription == false) {
                SN.enabled = false;
                MC.enabled = false;
                SD.enabled = true;
                displayingDescription = true;
            }
        }
    }


    public void OnPointerEnter(PointerEventData data) 
    {
        if(hasSpell == true) {
            spell = Instantiate(UISpellDesc, new Vector3(currPos.position.x - spellOffset, currPos.position.y , currPos.position.z), currPos.rotation);
            spell.GetComponentInChildren<TextMeshProUGUI>().text = GetComponentInParent<Hero_Character_Class>().spellList[spellNum].description;
        }
    }

    public void OnPointerExit(PointerEventData data) {
        if(hasSpell == true) {
            Destroy(spell);   
        }
    }

}