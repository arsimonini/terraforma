using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public GameObject summonQCAssetL;
    public GameObject summonQCAssetW;
    public GameObject summonQCAssetA;
    public GameObject SMQC = null;

    public GameObject heroQCAssetL;
    public GameObject heroQCAssetW;
    public GameObject heroQCAssetA;
    public GameObject HQC = null;

    public TeamInfo ti;

    public string currChanging;

    public HeroSwapButton h1;
    public HeroSwapButton h2;
    public HeroSwapButton h3;

    public GameObject[] spellButtons;
    public Sprite[] spellSprites;
    public string[] spellSpriteKeys;

    public GameObject[] summonButtons;
    public Sprite[] summonSprites;
    public string[] summonSpriteKeys;

    public GameObject[] heroButtons;
    public Sprite[] heroSprites;
    public string[] heroSpriteKeys;
    


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
            if(HQC != null) {
                Destroy(HQC);
                
            }
        }
        
        else if(Input.GetKeyUp(KeyCode.Escape) && menuMode == 1){
            if(SPQC != null) {
                Destroy(SPQC);
                currChanging = "";
            }
            if(SMQC != null) {
                Destroy(SMQC);
                currChanging = "";
            }
            if(HQC != null) {
                Destroy(HQC);
                currChanging = "";
            }
            if(menuMode == 1) {
                menuMode = 0;
                p1.SetActive(false);
                p0.SetActive(true);
                currChanging = "";
            }
        }
    }

    public void summonQuickChange(int slot) {
        string hero = "";

        //Get character in hero slot

        // FOR BOTH SUMMONS BEING UNLOCKED
        // if(slot == 1 || slot == 2) {
        //     hero = ti.hero1;
        //     currChanging = "Hero1_Summon" + (slot).ToString();
        // } else if (slot == 3 || slot == 4) {
        //     hero = ti.hero2;
        //     currChanging = "Hero2_Summon" + (slot - 2).ToString();
        // } else if (slot == 5 || slot == 6) {
        //     hero = ti.hero3;
        //     currChanging = "Hero3_Summon" + (slot - 4).ToString();
        // }

        if(slot == 1) {
            hero = ti.hero1;
            currChanging = "Hero1_Summon" + (slot).ToString();
        } else if (slot == 3) {
            hero = ti.hero2;
            currChanging = "Hero2_Summon" + (slot - 2).ToString();
        } else if (slot == 5) {
            hero = ti.hero3;
            currChanging = "Hero3_Summon" + (slot - 4).ToString();
        }
        //Make the menu appear
        if(SMQC != null) {
            Destroy(SMQC);
        }
        if(SPQC != null) {
            Destroy(SPQC);
        }
        if(HQC != null) {
            Destroy(HQC);
        }

        if(hero == "Lancin Bermane") {
            SMQC = Instantiate(summonQCAssetL, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        } else if(hero == "Wold Wold") {
            SMQC = Instantiate(summonQCAssetW, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        } else if(hero == "Althea Petrik") {
            SMQC = Instantiate(summonQCAssetA, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        }
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
        if(HQC != null) {
            Destroy(HQC);
        }

        if(hero == "Lancin Bermane") {
            SPQC = Instantiate(spellQCAssetL, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        } else if(hero == "Wold Wold") {
            SPQC = Instantiate(spellQCAssetW, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        } else if(hero == "Althea Petrik") {
            SPQC = Instantiate(spellQCAssetA, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        }

        
        
        
        //fill in the blanks for the menu        
    }
    
    public void heroQuickChange(int slot) {
        string hero = "";

        //Get character in hero slot
        if(slot == 1) {
            hero = ti.hero1;
            currChanging = "Hero1";
        } else if (slot == 2) {
            hero = ti.hero2;
            currChanging = "Hero2";
        } else if (slot == 3) {
            hero = ti.hero3;
            currChanging = "Hero3";
        }

        //Make the menu appear
        if(SPQC != null) {
            Destroy(SPQC);
        }
        if(SMQC != null) {
            Destroy(SMQC);
        }
        if(HQC != null) {
            Debug.Log("Arrived Here");
            Destroy(HQC);

        }

        if(hero == "Lancin Bermane") {
            HQC = Instantiate(heroQCAssetL, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        } else if(hero == "Wold Wold") {
            HQC = Instantiate(heroQCAssetW, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        } else if(hero == "Althea Petrik") {
            HQC = Instantiate(heroQCAssetA, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        }

        
        
        
        //fill in the blanks for the menu        
    }






    public void heroSlowChange(int slot) {
        //Open Second Menu
        p0.SetActive(false);
        menuMode = 1;
        p1.SetActive(true);
        //fill in the blanks for the menu

        if(slot == 1) {
            currChanging = "Hero1";
        } else if(slot == 2) {
            currChanging = "Hero2";
        } else if(slot == 3) {
            currChanging = "Hero3";
        }
    }

    public void setCurrPos(Transform t) {
        currPos = t;
    }

    public void setTeam() {
        h1.hero = ti.hero1;
        h1.summon1 = ti.summon1_1;
        h1.summon2 = ti.summon1_2;
        h1.spell1 = ti.spell1_1;
        h1.spell2 = ti.spell1_2;
        h1.spell3 = ti.spell1_3;
        h1.spell4 = ti.spell1_4;
        h1.spell5 = ti.spell1_5;
        h1.spell6 = ti.spell1_6;

        swapHero("Hero1", h1.hero);
        swapSummon("Hero1_Summon1", h1.summon1);
        swapSummon("Hero1_Summon2", h1.summon2);
        swapSpell("Hero1_Spell1", h1.spell1);
        swapSpell("Hero1_Spell2", h1.spell2);
        swapSpell("Hero1_Spell3", h1.spell3);
        swapSpell("Hero1_Spell4", h1.spell4);
        swapSpell("Hero1_Spell5", h1.spell5);
        swapSpell("Hero1_Spell6", h1.spell6);

        h2.hero = ti.hero2;
        h2.summon1 = ti.summon2_1;
        h2.summon2 = ti.summon2_2;
        h2.spell1 = ti.spell2_1;
        h2.spell2 = ti.spell2_2;
        h2.spell3 = ti.spell2_3;
        h2.spell4 = ti.spell2_4;
        h2.spell5 = ti.spell2_5;
        h2.spell6 = ti.spell2_6;

        swapHero("Hero2", h2.hero);
        swapSummon("Hero2_Summon1", h2.summon1);
        swapSummon("Hero2_Summon2", h2.summon2);
        swapSpell("Hero2_Spell1", h2.spell1);
        swapSpell("Hero2_Spell2", h2.spell2);
        swapSpell("Hero2_Spell3", h2.spell3);
        swapSpell("Hero2_Spell4", h2.spell4);
        swapSpell("Hero2_Spell5", h2.spell5);
        swapSpell("Hero2_Spell6", h2.spell6);

        h3.hero = ti.hero3;
        h3.summon1 = ti.summon3_1;
        h3.summon2 = ti.summon3_2;
        h3.spell1 = ti.spell3_1;
        h3.spell2 = ti.spell3_2;
        h3.spell3 = ti.spell3_3;
        h3.spell4 = ti.spell3_4;
        h3.spell5 = ti.spell3_5;
        h3.spell6 = ti.spell3_6;

        swapHero("Hero3", h3.hero);
        swapSummon("Hero3_Summon1", h3.summon1);
        swapSummon("Hero3_Summon2", h3.summon2);
        swapSpell("Hero3_Spell1", h3.spell1);
        swapSpell("Hero3_Spell2", h3.spell2);
        swapSpell("Hero3_Spell3", h3.spell3);
        swapSpell("Hero3_Spell4", h3.spell4);
        swapSpell("Hero3_Spell5", h3.spell5);
        swapSpell("Hero3_Spell6", h3.spell6);


        
        
        // ti.hero1 = h1.hero;
        // ti.summon1_1 = h1.summon1;
        // ti.summon1_2 = h1.summon2;
        // ti.spell1_1 = h1.spell1;
        // ti.spell1_2 = h1.spell2;
        // ti.spell1_3 = h1.spell3;
        // ti.spell1_4 = h1.spell4;
        // ti.spell1_5 = h1.spell5;
        // ti.spell1_6 = h1.spell6;

        // ti.hero2 = h2.hero;
        // ti.summon2_1 = h2.summon1;
        // ti.summon2_2 = h2.summon2;
        // ti.spell2_1 = h2.spell1;
        // ti.spell2_2 = h2.spell2;
        // ti.spell2_3 = h2.spell3;
        // ti.spell2_4 = h2.spell4;
        // ti.spell2_5 = h2.spell5;
        // ti.spell2_6 = h2.spell6;

        // ti.hero3 = h3.hero;
        // ti.summon3_1 = h3.summon1;
        // ti.summon3_2 = h3.summon2;
        // ti.spell3_1 = h3.spell1;
        // ti.spell3_2 = h3.spell2;
        // ti.spell3_3 = h3.spell3;
        // ti.spell3_4 = h3.spell4;
        // ti.spell3_5 = h3.spell5;
        // ti.spell3_6 = h3.spell6;

    }

    public void swapSpellSprite(int index, string spell) {
        for(int i = 0; i < spellSpriteKeys.Length; i++) {
            if(spell == spellSpriteKeys[i]) {
                //go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
                spellButtons[index].GetComponent<Image>().sprite = spellSprites[i];
            }
        }

    }



    public void swapSpell(string place, string what) {
        // if(what == "Locked") {
        //     return;
        // }
        switch (place) {
            case "Hero1_Spell1":
            ti.spell1_1 = what;
            swapSpellSprite(0, what);
            break;

            case "Hero1_Spell2":
            ti.spell1_2 = what;
            swapSpellSprite(1, what);
            break;

            case "Hero1_Spell3":
            ti.spell1_3 = what;
            swapSpellSprite(2, what);
            break;

            case "Hero1_Spell4":
            ti.spell1_4 = what;
            swapSpellSprite(3, what);
            break;

            case "Hero1_Spell5":
            ti.spell1_5 = what;
            swapSpellSprite(4, what);
            break;

            case "Hero1_Spell6":
            ti.spell1_6 = what;
            swapSpellSprite(5, what);
            break;
            
            case "Hero2_Spell1":
            ti.spell2_1 = what;
            swapSpellSprite(6, what);
            break;

            case "Hero2_Spell2":
            ti.spell2_2 = what;
            swapSpellSprite(7, what);
            break;

            case "Hero2_Spell3":
            ti.spell2_3 = what;
            swapSpellSprite(8, what);
            break;

            case "Hero2_Spell4":
            ti.spell2_4 = what;
            swapSpellSprite(9, what);
            break;

            case "Hero2_Spell5":
            ti.spell2_5 = what;
            swapSpellSprite(10, what);
            break;

            case "Hero2_Spell6":
            ti.spell2_6 = what;
            swapSpellSprite(11, what);
            break;

            case "Hero3_Spell1":
            ti.spell3_1 = what;
            swapSpellSprite(12, what);
            break;

            case "Hero3_Spell2":
            ti.spell3_2 = what;
            swapSpellSprite(13, what);
            break;

            case "Hero3_Spell3":
            ti.spell3_3 = what;
            swapSpellSprite(14, what);
            break;

            case "Hero3_Spell4":
            ti.spell3_4 = what;
            swapSpellSprite(15, what);
            break;

            case "Hero3_Spell5":
            ti.spell3_5 = what;
            swapSpellSprite(16, what);
            break;

            case "Hero3_Spell6":
            ti.spell3_6 = what;
            swapSpellSprite(17, what);
            break;
        }
    }

        public void swapSummonSprite(int index, string summon) {
        for(int i = 0; i < summonSpriteKeys.Length; i++) {
            if(summon == summonSpriteKeys[i]) {
                //go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
                summonButtons[index].GetComponent<Image>().sprite = summonSprites[i];
            }
        }

    }



    public void swapSummon(string place, string what) {
        // if(what == "Locked") {
        //     return;
        // }
        switch (place) {
            case "Hero1_Summon1":
            ti.summon1_1 = what;
            swapSummonSprite(0, what);
            break;

            case "Hero1_Summon2":
            ti.summon1_2 = what;
            swapSummonSprite(1, what);
            break;

            case "Hero2_Summon1":
            ti.summon2_1 = what;
            swapSummonSprite(2, what);
            break;

            case "Hero2_Summon2":
            ti.summon2_2 = what;
            swapSummonSprite(3, what);
            break;

            case "Hero3_Summon1":
            ti.summon3_1 = what;
            swapSummonSprite(4, what);
            break;

            case "Hero3_Summon2":
            ti.summon3_2 = what;
            swapSummonSprite(5, what);
            break;
            
        }
    }


    public void GoToMenu() {
        
        SceneManager.LoadScene("Title");
        //Debug.Log("Quit Clicked");
        //Application.Quit();
    }



    public void swapHeroSprite(int index, string hero) {
        for(int i = 0; i < heroSpriteKeys.Length; i++) {
            if(hero == heroSpriteKeys[i]) {
                //go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
                heroButtons[index].GetComponent<Image>().sprite = heroSprites[i];
            }
        }

    }



    public void swapHero(string place, string what) {
        if(what == "Locked") {
            return;
        }
        switch (place) {
            case "Hero1":
            ti.hero1 = what;
            swapHeroSprite(0, what);
            break;

            case "Hero2":
            ti.hero2 = what;
            swapHeroSprite(1, what);
            break;

            case "Hero3":
            ti.hero3 = what;
            swapHeroSprite(2, what);
            break;

        }
    }


}
