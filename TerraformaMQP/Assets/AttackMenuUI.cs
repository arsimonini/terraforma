using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMenuUI : MonoBehaviour
{

    private Color usedButtonCol;
    [SerializeField]
    public GameObject AttackButton;
    public GameObject MoveButton;
    public GameObject MagicButton = null;
    public GameObject WaitButton;

    // Start is called before the first frame update
    void Start()
    {
        //set the color for grayed out buttons
        ColorUtility.TryParseHtmlString("#BFB6B6", out usedButtonCol);
    }

    // Update is called once per frame
    void Update()
    {
        greyOutButton();
    }


    public void greyOutButton() 
    {
        if(GetComponentInParent<Basic_Character_Class>().actionsLeft.moddedValue == 0) {
            AttackButton.GetComponent<Image>().color = usedButtonCol;
            MoveButton.GetComponent<Image>().color = usedButtonCol;
            if(MagicButton != null) {
                MagicButton.GetComponent<Image>().color = usedButtonCol;
            }
        }
        if(GetComponentInParent<Basic_Character_Class>().hasWalked == true) {
            MoveButton.GetComponent<Image>().color = usedButtonCol;
        }
        if(GetComponentInParent<Basic_Character_Class>().actionsLeft.moddedValue == 0) {
            AttackButton.GetComponent<Image>().color = Color.white;
            if(MagicButton != null) {
                MagicButton.GetComponent<Image>().color = Color.white;
            }
        }
        if(GetComponentInParent<Basic_Character_Class>().hasWalked == false) {
            MoveButton.GetComponent<Image>().color = Color.white;
        }
    }
    
}
