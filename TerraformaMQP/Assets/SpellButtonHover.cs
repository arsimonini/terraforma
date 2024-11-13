using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellButtonHover : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    //public GameObject textBox;
    //public TextMeshProUGUI descrip;
    //public Hero_Character_Class hcc;
    //public int spellNum;

    public GameObject UISpellDesc;


    void Start()
    {
        //textBox.SetActive(false);
    }


    void OnMouseEnter() 
    {
        //textBox.SetActive(true);
        //descrip.text = hcc.spellList[spellNum].description;

        GameObject spell = Instantiate(UISpellDesc, new Vector3(0, 0, 0), Quaternion.identity);
        UnityEngine.Debug.Log("Do I get Here");
        //
    }

    void OnMouseExit()
    {
        //textBox.SetActive(false);
    }

}
