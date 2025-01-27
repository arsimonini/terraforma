using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeCharMenu : MonoBehaviour
{

    public int menuMode = 0;
    public GameObject p0;
    public GameObject p1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.LeftArrow)) {
            if(menuMode == 0) {
                return;
            }
            else {
                menuMode = menuMode - 1;
            }
        }
        if(Input.GetKeyUp(KeyCode.RightArrow)) {
            if(menuMode == 3) {
                return;
            }
            else {
                menuMode = menuMode + 1;
            }
        }

        switch (menuMode)
        {
        case 0:
            p0.SetActive(true);
            p1.SetActive(false);
            break;
            
        case 1:
            p0.SetActive(false);
            p1.SetActive(true);
            break;
        }
    }

    public void summonQuickChange(int slot) {
        //Get character in hero slot
        //Make the menu appear
        //fill in the blanks for the menu
    }

    public void spellQuickChange(int slot) {
        //Get character in hero slot
        //Make the menu appear
        //fill in the blanks for the menu        
    }

    public void heroSlowChange(int slot) {
        //Open Second Menu
        //fill in the blanks for the menu
    }
}
