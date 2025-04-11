using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroQuickSelect : MonoBehaviour
{

    public string buttonClicked;
    public TeamInfo ti;
    public GameObject go;
    public GameObject thisGO;

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void heroButtonClicked(string s) {
        buttonClicked = s;
        swapHero(go.GetComponent<ChangeCharMenu>().currChanging, buttonClicked);
        
        
        Destroy(thisGO);
    }

    public void swapHeroSprite(int index, string hero) {
        for(int i = 0; i < go.GetComponent<ChangeCharMenu>().heroSpriteKeys.Length; i++) {
            if(hero == go.GetComponent<ChangeCharMenu>().heroSpriteKeys[i]) {
                //go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
                go.GetComponent<ChangeCharMenu>().heroButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().heroSprites[i];
            }
        }

    }



    public void swapHero(string place, string what) {
        if(what == "Locked") {
            return;
        }
        switch (place) {
            case "Hero1":
            heroCheck(1, what);
            ti.hero1 = what;
            swapHeroSprite(0, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero2":
            heroCheck(2, what);
            ti.hero2 = what;
            swapHeroSprite(1, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero3":
            heroCheck(3, what);
            ti.hero3 = what;
            swapHeroSprite(2, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

        }
    }

    public void heroCheck(int num, string changingToHero) {

        if(num == 1) {
            if(ti.hero2 == changingToHero) {
                string tempHero = ti.hero1;
                string tempSummon1 = ti.summon1_1;
                string tempSummon2 = ti.summon1_2;
                string tempSpell1 = ti.spell1_1;
                string tempSpell2 = ti.spell1_2;
                string tempSpell3 = ti.spell1_3;
                string tempSpell4 = ti.spell1_4;
                string tempSpell5 = ti.spell1_5;
                string tempSpell6 = ti.spell1_6;

                ti.hero1 = ti.hero2;
                ti.summon1_1 = ti.summon2_1;
                ti.summon1_2 = ti.summon2_2;
                ti.spell1_1 = ti.spell2_1;
                ti.spell1_2 = ti.spell2_2;
                ti.spell1_3 = ti.spell2_3;
                ti.spell1_4 = ti.spell2_4;
                ti.spell1_5 = ti.spell2_5;
                ti.spell1_6 = ti.spell2_6;

                ti.hero2 = tempHero;
                ti.summon2_1 = tempSummon1;
                ti.summon2_2 = tempSummon2;
                ti.spell2_1 = tempSpell1;
                ti.spell2_2 = tempSpell2;
                ti.spell2_3 = tempSpell3;
                ti.spell2_4 = tempSpell4;
                ti.spell2_5 = tempSpell5;
                ti.spell2_6 = tempSpell6;
            }
            else if(ti.hero3 == changingToHero) {
                string tempHero = ti.hero1;
                string tempSummon1 = ti.summon1_1;
                string tempSummon2 = ti.summon1_2;
                string tempSpell1 = ti.spell1_1;
                string tempSpell2 = ti.spell1_2;
                string tempSpell3 = ti.spell1_3;
                string tempSpell4 = ti.spell1_4;
                string tempSpell5 = ti.spell1_5;
                string tempSpell6 = ti.spell1_6;

                ti.hero1 = ti.hero3;
                ti.summon1_1 = ti.summon3_1;
                ti.summon1_2 = ti.summon3_2;
                ti.spell1_1 = ti.spell3_1;
                ti.spell1_2 = ti.spell3_2;
                ti.spell1_3 = ti.spell3_3;
                ti.spell1_4 = ti.spell3_4;
                ti.spell1_5 = ti.spell3_5;
                ti.spell1_6 = ti.spell3_6;

                ti.hero3 = tempHero;
                ti.summon3_1 = tempSummon1;
                ti.summon3_2 = tempSummon2;
                ti.spell3_1 = tempSpell1;
                ti.spell3_2 = tempSpell2;
                ti.spell3_3 = tempSpell3;
                ti.spell3_4 = tempSpell4;
                ti.spell3_5 = tempSpell5;
                ti.spell3_6 = tempSpell6;
            }
        } else if(num == 2) {
            if(ti.hero1 == changingToHero) {
                string tempHero = ti.hero2;
                string tempSummon1 = ti.summon2_1;
                string tempSummon2 = ti.summon2_2;
                string tempSpell1 = ti.spell2_1;
                string tempSpell2 = ti.spell2_2;
                string tempSpell3 = ti.spell2_3;
                string tempSpell4 = ti.spell2_4;
                string tempSpell5 = ti.spell2_5;
                string tempSpell6 = ti.spell2_6;

                ti.hero2 = ti.hero1;
                ti.summon2_1 = ti.summon1_1;
                ti.summon2_2 = ti.summon1_2;
                ti.spell2_1 = ti.spell1_1;
                ti.spell2_2 = ti.spell1_2;
                ti.spell2_3 = ti.spell1_3;
                ti.spell2_4 = ti.spell1_4;
                ti.spell2_5 = ti.spell1_5;
                ti.spell2_6 = ti.spell1_6;

                ti.hero1 = tempHero;
                ti.summon1_1 = tempSummon1;
                ti.summon1_2 = tempSummon2;
                ti.spell1_1 = tempSpell1;
                ti.spell1_2 = tempSpell2;
                ti.spell1_3 = tempSpell3;
                ti.spell1_4 = tempSpell4;
                ti.spell1_5 = tempSpell5;
                ti.spell1_6 = tempSpell6;
            }
            else if(ti.hero3 == changingToHero) {
                string tempHero = ti.hero2;
                string tempSummon1 = ti.summon2_1;
                string tempSummon2 = ti.summon2_2;
                string tempSpell1 = ti.spell2_1;
                string tempSpell2 = ti.spell2_2;
                string tempSpell3 = ti.spell2_3;
                string tempSpell4 = ti.spell2_4;
                string tempSpell5 = ti.spell2_5;
                string tempSpell6 = ti.spell2_6;

                ti.hero2 = ti.hero3;
                ti.summon2_1 = ti.summon3_1;
                ti.summon2_2 = ti.summon3_2;
                ti.spell2_1 = ti.spell3_1;
                ti.spell2_2 = ti.spell3_2;
                ti.spell2_3 = ti.spell3_3;
                ti.spell2_4 = ti.spell3_4;
                ti.spell2_5 = ti.spell3_5;
                ti.spell2_6 = ti.spell3_6;

                ti.hero3 = tempHero;
                ti.summon3_1 = tempSummon1;
                ti.summon3_2 = tempSummon2;
                ti.spell3_1 = tempSpell1;
                ti.spell3_2 = tempSpell2;
                ti.spell3_3 = tempSpell3;
                ti.spell3_4 = tempSpell4;
                ti.spell3_5 = tempSpell5;
                ti.spell3_6 = tempSpell6;
            }
        } else if(num == 3) {
            if(ti.hero2 == changingToHero) {
                string tempHero = ti.hero3;
                string tempSummon1 = ti.summon3_1;
                string tempSummon2 = ti.summon3_2;
                string tempSpell1 = ti.spell3_1;
                string tempSpell2 = ti.spell3_2;
                string tempSpell3 = ti.spell3_3;
                string tempSpell4 = ti.spell3_4;
                string tempSpell5 = ti.spell3_5;
                string tempSpell6 = ti.spell3_6;

                ti.hero3 = ti.hero2;
                ti.summon3_1 = ti.summon2_1;
                ti.summon3_2 = ti.summon2_2;
                ti.spell3_1 = ti.spell2_1;
                ti.spell3_2 = ti.spell2_2;
                ti.spell3_3 = ti.spell2_3;
                ti.spell3_4 = ti.spell2_4;
                ti.spell3_5 = ti.spell2_5;
                ti.spell3_6 = ti.spell2_6;

                ti.hero2 = tempHero;
                ti.summon2_1 = tempSummon1;
                ti.summon2_2 = tempSummon2;
                ti.spell2_1 = tempSpell1;
                ti.spell2_2 = tempSpell2;
                ti.spell2_3 = tempSpell3;
                ti.spell2_4 = tempSpell4;
                ti.spell2_5 = tempSpell5;
                ti.spell2_6 = tempSpell6;
            }
            else if(ti.hero1 == changingToHero) {
                string tempHero = ti.hero3;
                string tempSummon1 = ti.summon3_1;
                string tempSummon2 = ti.summon3_2;
                string tempSpell1 = ti.spell3_1;
                string tempSpell2 = ti.spell3_2;
                string tempSpell3 = ti.spell3_3;
                string tempSpell4 = ti.spell3_4;
                string tempSpell5 = ti.spell3_5;
                string tempSpell6 = ti.spell3_6;

                ti.hero3 = ti.hero1;
                ti.summon3_1 = ti.summon1_1;
                ti.summon3_2 = ti.summon1_2;
                ti.spell3_1 = ti.spell1_1;
                ti.spell3_2 = ti.spell1_2;
                ti.spell3_3 = ti.spell1_3;
                ti.spell3_4 = ti.spell1_4;
                ti.spell3_5 = ti.spell1_5;
                ti.spell3_6 = ti.spell1_6;

                ti.hero1 = tempHero;
                ti.summon1_1 = tempSummon1;
                ti.summon1_2 = tempSummon2;
                ti.spell1_1 = tempSpell1;
                ti.spell1_2 = tempSpell2;
                ti.spell1_3 = tempSpell3;
                ti.spell1_4 = tempSpell4;
                ti.spell1_5 = tempSpell5;
                ti.spell1_6 = tempSpell6;
            }
        }
    }

}
