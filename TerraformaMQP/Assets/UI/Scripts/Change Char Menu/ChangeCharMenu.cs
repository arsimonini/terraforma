using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeCharMenu : MonoBehaviour
{

    public int menuMode = 0;
    public GameObject p0;
    public GameObject p1;
    public Transform currPos = null;
    public Transform parent;
    public GameObject spellQCAssetL;
    public GameObject spellQCAssetW;
    public GameObject spellQCAssetA;
    public GameObject SPQC = null;

    public GameObject summonQCAsset;
    public GameObject SMQC = null;

    public TeamInfo ti;

    public string currChanging;

    public HeroSwapButton h1;
    public HeroSwapButton h2;
    public HeroSwapButton h3;

    public GameObject[] spellButtons;
    public Sprite[] spellSprites;
    


    // Start is called before the first frame update
    void Start()
    {
        setTeam();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && menuMode == 0) {
            if(SPQC != null) {
                Destroy(SPQC);
                currChanging = "";
            }
            if(SMQC != null) {
                Destroy(SMQC);
                currChanging = "";
            }
        }
        else if(Input.GetKeyUp(KeyCode.Escape) && menuMode == 1){
            if(SPQC != null) {
                Destroy(SPQC);
            }
            if(SMQC != null) {
                Destroy(SMQC);
            }
            if(menuMode == 1) {
                menuMode = 0;
                p1.SetActive(false);
                p0.SetActive(true);
            }
        }
    }

    public void summonQuickChange(int slot) {
        //Get character in hero slot
        //Make the menu appear
        if(SMQC != null) {
            Destroy(SMQC);
        }
        if(SPQC != null) {
            Destroy(SPQC);
        }

        SPQC = Instantiate(summonQCAsset, new Vector3(currPos.position.x, currPos.position.y - 40, currPos.position.z), Quaternion.identity, parent);
        //fill in the blanks for the menu
    }

    public void spellQuickChange(int slot) {
        string hero = "";

        //Get character in hero slot
        if(slot >= 1 && slot <= 6) {
            hero = ti.hero1;
            currChanging = "Hero1_Spell" + (slot).ToString();
        } else if (slot >= 7 && slot <= 12) {
            hero = ti.hero2;
            currChanging = "Hero2_Spell" + (slot - 6).ToString();
        } else if (slot >= 13 && slot <= 18) {
            hero = ti.hero3;
            currChanging = "Hero3_Spell" + (slot - 12).ToString();
        }

        //Make the menu appear
        if(SPQC != null) {
            Destroy(SPQC);
        }
        if(SMQC != null) {
            Destroy(SMQC);
        }

        if(hero == "Lancin") {
            SPQC = Instantiate(spellQCAssetL, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        } else if(hero == "Wold") {
            SPQC = Instantiate(spellQCAssetW, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        } else if(hero == "Althea") {
            SPQC = Instantiate(spellQCAssetA, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        }

        
        
        
        //fill in the blanks for the menu        
    }

    public void heroSlowChange(int slot) {
        //Open Second Menu
        p0.SetActive(false);
        menuMode = 1;
        p1.SetActive(true);
        //fill in the blanks for the menu
    }

    public void setCurrPos(Transform t) {
        currPos = t;
    }

    public void setTeam() {
        ti.hero1 = h1.hero;
        ti.summon1_1 = h1.summon1;
        ti.summon1_2 = h1.summon2;
        ti.spell1_1 = h1.spell1;
        ti.spell1_2 = h1.spell2;
        ti.spell1_3 = h1.spell3;
        ti.spell1_4 = h1.spell4;
        ti.spell1_5 = h1.spell5;
        ti.spell1_6 = h1.spell6;

        ti.hero2 = h2.hero;
        ti.summon2_1 = h2.summon1;
        ti.summon2_2 = h2.summon2;
        ti.spell2_1 = h2.spell1;
        ti.spell2_2 = h2.spell2;
        ti.spell2_3 = h2.spell3;
        ti.spell2_4 = h2.spell4;
        ti.spell2_5 = h2.spell5;
        ti.spell2_6 = h2.spell6;

        ti.hero3 = h3.hero;
        ti.summon3_1 = h3.summon1;
        ti.summon3_2 = h3.summon2;
        ti.spell3_1 = h3.spell1;
        ti.spell3_2 = h3.spell2;
        ti.spell3_3 = h3.spell3;
        ti.spell3_4 = h3.spell4;
        ti.spell3_5 = h3.spell5;
        ti.spell3_6 = h3.spell6;

    }

    public void setSprites() {
        for(int i = 0; i < spellButtons.Length; i++) {
            //spellButtons[i].GetComponent<Image>().sprite = 
        }
    }

    public void swapSpell(string place, string what) {
        
        switch (place) {
            case "Hero1_Spell1":
            ti.spell1_1 = what;
            break;

            case "Hero1_Spell2":
            ti.spell1_2 = what;
            break;

            case "Hero1_Spell3":
            ti.spell1_3 = what;
            break;

            case "Hero1_Spell4":
            ti.spell1_4 = what;
            break;

            case "Hero1_Spell5":
            ti.spell1_5 = what;
            break;

            case "Hero1_Spell6":
            ti.spell1_6 = what;
            break;
            
            case "Hero2_Spell1":
            ti.spell2_1 = what;
            break;

            case "Hero2_Spell2":
            ti.spell2_2 = what;
            break;

            case "Hero2_Spell3":
            ti.spell2_3 = what;
            break;

            case "Hero2_Spell4":
            ti.spell2_4 = what;
            break;

            case "Hero2_Spell5":
            ti.spell2_5 = what;
            break;

            case "Hero2_Spell6":
            ti.spell2_6 = what;
            break;

            case "Hero3_Spell1":
            ti.spell3_1 = what;
            break;

            case "Hero3_Spell2":
            ti.spell3_2 = what;
            break;

            case "Hero3_Spell3":
            ti.spell3_3 = what;
            break;

            case "Hero3_Spell4":
            ti.spell3_4 = what;
            break;

            case "Hero3_Spell5":
            ti.spell3_5 = what;
            break;

            case "Hero3_Spell6":
            ti.spell3_6 = what;
            break;
        }
    }


}
