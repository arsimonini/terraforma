   using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // Start is called before the first frame update
    [SerializeField]
    //public GameObject textBox;
    //public TextMeshProUGUI descrip;
    //public Hero_Character_Class hcc;
    //public int spellNum;

    public GameObject abilityDescription = null;
    //public TextMeshProUGUI spellDescText = null;
    //public GameObject spell = null;
    //public Transform currPos;

    public bool displayingDescription = false;


    void Start()
    {
    
    }

    public void Update() 
    {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            abilityDescription.SetActive(false);
        }
    }


    public void OnPointerEnter(PointerEventData data) 
    {
    abilityDescription.SetActive(true);
            
    }

    public void OnPointerExit(PointerEventData data) {
        abilityDescription.SetActive(false);
    }

    public void buttonClicked() {
        //Destroy(spell);
        abilityDescription.SetActive(false);
    }

}

