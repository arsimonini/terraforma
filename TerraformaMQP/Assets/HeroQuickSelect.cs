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
            TeamInfoManager.instance.hero1 = what;
            swapHeroSprite(0, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero2":
            heroCheck(2, what);
            TeamInfoManager.instance.hero2 = what;
            swapHeroSprite(1, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero3":
            heroCheck(3, what);
            TeamInfoManager.instance.hero3 = what;
            swapHeroSprite(2, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

        }
    }

    public void heroCheck(int num, string changingToHero) {

        if(num == 1) {
            if(TeamInfoManager.instance.hero2 == changingToHero) {
                string tempHero = TeamInfoManager.instance.hero1;
                string tempSummon1 = TeamInfoManager.instance.summon1_1;
                string tempSummon2 = TeamInfoManager.instance.summon1_2;
                string tempSpell1 = TeamInfoManager.instance.spell1_1;
                string tempSpell2 = TeamInfoManager.instance.spell1_2;
                string tempSpell3 = TeamInfoManager.instance.spell1_3;
                string tempSpell4 = TeamInfoManager.instance.spell1_4;
                string tempSpell5 = TeamInfoManager.instance.spell1_5;
                string tempSpell6 = TeamInfoManager.instance.spell1_6;

                TeamInfoManager.instance.hero1 = TeamInfoManager.instance.hero2;
                TeamInfoManager.instance.summon1_1 = TeamInfoManager.instance.summon2_1;
                TeamInfoManager.instance.summon1_2 = TeamInfoManager.instance.summon2_2;
                TeamInfoManager.instance.spell1_1 = TeamInfoManager.instance.spell2_1;
                TeamInfoManager.instance.spell1_2 = TeamInfoManager.instance.spell2_2;
                TeamInfoManager.instance.spell1_3 = TeamInfoManager.instance.spell2_3;
                TeamInfoManager.instance.spell1_4 = TeamInfoManager.instance.spell2_4;
                TeamInfoManager.instance.spell1_5 = TeamInfoManager.instance.spell2_5;
                TeamInfoManager.instance.spell1_6 = TeamInfoManager.instance.spell2_6;

                TeamInfoManager.instance.hero2 = tempHero;
                TeamInfoManager.instance.summon2_1 = tempSummon1;
                TeamInfoManager.instance.summon2_2 = tempSummon2;
                TeamInfoManager.instance.spell2_1 = tempSpell1;
                TeamInfoManager.instance.spell2_2 = tempSpell2;
                TeamInfoManager.instance.spell2_3 = tempSpell3;
                TeamInfoManager.instance.spell2_4 = tempSpell4;
                TeamInfoManager.instance.spell2_5 = tempSpell5;
                TeamInfoManager.instance.spell2_6 = tempSpell6;
            }
            else if(TeamInfoManager.instance.hero3 == changingToHero) {
                string tempHero = TeamInfoManager.instance.hero1;
                string tempSummon1 = TeamInfoManager.instance.summon1_1;
                string tempSummon2 = TeamInfoManager.instance.summon1_2;
                string tempSpell1 = TeamInfoManager.instance.spell1_1;
                string tempSpell2 = TeamInfoManager.instance.spell1_2;
                string tempSpell3 = TeamInfoManager.instance.spell1_3;
                string tempSpell4 = TeamInfoManager.instance.spell1_4;
                string tempSpell5 = TeamInfoManager.instance.spell1_5;
                string tempSpell6 = TeamInfoManager.instance.spell1_6;

                TeamInfoManager.instance.hero1 = TeamInfoManager.instance.hero3;
                TeamInfoManager.instance.summon1_1 = TeamInfoManager.instance.summon3_1;
                TeamInfoManager.instance.summon1_2 = TeamInfoManager.instance.summon3_2;
                TeamInfoManager.instance.spell1_1 = TeamInfoManager.instance.spell3_1;
                TeamInfoManager.instance.spell1_2 = TeamInfoManager.instance.spell3_2;
                TeamInfoManager.instance.spell1_3 = TeamInfoManager.instance.spell3_3;
                TeamInfoManager.instance.spell1_4 = TeamInfoManager.instance.spell3_4;
                TeamInfoManager.instance.spell1_5 = TeamInfoManager.instance.spell3_5;
                TeamInfoManager.instance.spell1_6 = TeamInfoManager.instance.spell3_6;

                TeamInfoManager.instance.hero3 = tempHero;
                TeamInfoManager.instance.summon3_1 = tempSummon1;
                TeamInfoManager.instance.summon3_2 = tempSummon2;
                TeamInfoManager.instance.spell3_1 = tempSpell1;
                TeamInfoManager.instance.spell3_2 = tempSpell2;
                TeamInfoManager.instance.spell3_3 = tempSpell3;
                TeamInfoManager.instance.spell3_4 = tempSpell4;
                TeamInfoManager.instance.spell3_5 = tempSpell5;
                TeamInfoManager.instance.spell3_6 = tempSpell6;
            }
        } else if(num == 2) {
            if(TeamInfoManager.instance.hero1 == changingToHero) {
                string tempHero = TeamInfoManager.instance.hero2;
                string tempSummon1 = TeamInfoManager.instance.summon2_1;
                string tempSummon2 = TeamInfoManager.instance.summon2_2;
                string tempSpell1 = TeamInfoManager.instance.spell2_1;
                string tempSpell2 = TeamInfoManager.instance.spell2_2;
                string tempSpell3 = TeamInfoManager.instance.spell2_3;
                string tempSpell4 = TeamInfoManager.instance.spell2_4;
                string tempSpell5 = TeamInfoManager.instance.spell2_5;
                string tempSpell6 = TeamInfoManager.instance.spell2_6;

                TeamInfoManager.instance.hero2 = TeamInfoManager.instance.hero1;
                TeamInfoManager.instance.summon2_1 = TeamInfoManager.instance.summon1_1;
                TeamInfoManager.instance.summon2_2 = TeamInfoManager.instance.summon1_2;
                TeamInfoManager.instance.spell2_1 = TeamInfoManager.instance.spell1_1;
                TeamInfoManager.instance.spell2_2 = TeamInfoManager.instance.spell1_2;
                TeamInfoManager.instance.spell2_3 = TeamInfoManager.instance.spell1_3;
                TeamInfoManager.instance.spell2_4 = TeamInfoManager.instance.spell1_4;
                TeamInfoManager.instance.spell2_5 = TeamInfoManager.instance.spell1_5;
                TeamInfoManager.instance.spell2_6 = TeamInfoManager.instance.spell1_6;

                TeamInfoManager.instance.hero1 = tempHero;
                TeamInfoManager.instance.summon1_1 = tempSummon1;
                TeamInfoManager.instance.summon1_2 = tempSummon2;
                TeamInfoManager.instance.spell1_1 = tempSpell1;
                TeamInfoManager.instance.spell1_2 = tempSpell2;
                TeamInfoManager.instance.spell1_3 = tempSpell3;
                TeamInfoManager.instance.spell1_4 = tempSpell4;
                TeamInfoManager.instance.spell1_5 = tempSpell5;
                TeamInfoManager.instance.spell1_6 = tempSpell6;
            }
            else if(TeamInfoManager.instance.hero3 == changingToHero) {
                string tempHero = TeamInfoManager.instance.hero2;
                string tempSummon1 = TeamInfoManager.instance.summon2_1;
                string tempSummon2 = TeamInfoManager.instance.summon2_2;
                string tempSpell1 = TeamInfoManager.instance.spell2_1;
                string tempSpell2 = TeamInfoManager.instance.spell2_2;
                string tempSpell3 = TeamInfoManager.instance.spell2_3;
                string tempSpell4 = TeamInfoManager.instance.spell2_4;
                string tempSpell5 = TeamInfoManager.instance.spell2_5;
                string tempSpell6 = TeamInfoManager.instance.spell2_6;

                TeamInfoManager.instance.hero2 = TeamInfoManager.instance.hero3;
                TeamInfoManager.instance.summon2_1 = TeamInfoManager.instance.summon3_1;
                TeamInfoManager.instance.summon2_2 = TeamInfoManager.instance.summon3_2;
                TeamInfoManager.instance.spell2_1 = TeamInfoManager.instance.spell3_1;
                TeamInfoManager.instance.spell2_2 = TeamInfoManager.instance.spell3_2;
                TeamInfoManager.instance.spell2_3 = TeamInfoManager.instance.spell3_3;
                TeamInfoManager.instance.spell2_4 = TeamInfoManager.instance.spell3_4;
                TeamInfoManager.instance.spell2_5 = TeamInfoManager.instance.spell3_5;
                TeamInfoManager.instance.spell2_6 = TeamInfoManager.instance.spell3_6;

                TeamInfoManager.instance.hero3 = tempHero;
                TeamInfoManager.instance.summon3_1 = tempSummon1;
                TeamInfoManager.instance.summon3_2 = tempSummon2;
                TeamInfoManager.instance.spell3_1 = tempSpell1;
                TeamInfoManager.instance.spell3_2 = tempSpell2;
                TeamInfoManager.instance.spell3_3 = tempSpell3;
                TeamInfoManager.instance.spell3_4 = tempSpell4;
                TeamInfoManager.instance.spell3_5 = tempSpell5;
                TeamInfoManager.instance.spell3_6 = tempSpell6;
            }
        } else if(num == 3) {
            if(TeamInfoManager.instance.hero2 == changingToHero) {
                string tempHero = TeamInfoManager.instance.hero3;
                string tempSummon1 = TeamInfoManager.instance.summon3_1;
                string tempSummon2 = TeamInfoManager.instance.summon3_2;
                string tempSpell1 = TeamInfoManager.instance.spell3_1;
                string tempSpell2 = TeamInfoManager.instance.spell3_2;
                string tempSpell3 = TeamInfoManager.instance.spell3_3;
                string tempSpell4 = TeamInfoManager.instance.spell3_4;
                string tempSpell5 = TeamInfoManager.instance.spell3_5;
                string tempSpell6 = TeamInfoManager.instance.spell3_6;

                TeamInfoManager.instance.hero3 = TeamInfoManager.instance.hero2;
                TeamInfoManager.instance.summon3_1 = TeamInfoManager.instance.summon2_1;
                TeamInfoManager.instance.summon3_2 = TeamInfoManager.instance.summon2_2;
                TeamInfoManager.instance.spell3_1 = TeamInfoManager.instance.spell2_1;
                TeamInfoManager.instance.spell3_2 = TeamInfoManager.instance.spell2_2;
                TeamInfoManager.instance.spell3_3 = TeamInfoManager.instance.spell2_3;
                TeamInfoManager.instance.spell3_4 = TeamInfoManager.instance.spell2_4;
                TeamInfoManager.instance.spell3_5 = TeamInfoManager.instance.spell2_5;
                TeamInfoManager.instance.spell3_6 = TeamInfoManager.instance.spell2_6;

                TeamInfoManager.instance.hero2 = tempHero;
                TeamInfoManager.instance.summon2_1 = tempSummon1;
                TeamInfoManager.instance.summon2_2 = tempSummon2;
                TeamInfoManager.instance.spell2_1 = tempSpell1;
                TeamInfoManager.instance.spell2_2 = tempSpell2;
                TeamInfoManager.instance.spell2_3 = tempSpell3;
                TeamInfoManager.instance.spell2_4 = tempSpell4;
                TeamInfoManager.instance.spell2_5 = tempSpell5;
                TeamInfoManager.instance.spell2_6 = tempSpell6;
            }
            else if(TeamInfoManager.instance.hero1 == changingToHero) {
                string tempHero = TeamInfoManager.instance.hero3;
                string tempSummon1 = TeamInfoManager.instance.summon3_1;
                string tempSummon2 = TeamInfoManager.instance.summon3_2;
                string tempSpell1 = TeamInfoManager.instance.spell3_1;
                string tempSpell2 = TeamInfoManager.instance.spell3_2;
                string tempSpell3 = TeamInfoManager.instance.spell3_3;
                string tempSpell4 = TeamInfoManager.instance.spell3_4;
                string tempSpell5 = TeamInfoManager.instance.spell3_5;
                string tempSpell6 = TeamInfoManager.instance.spell3_6;

                TeamInfoManager.instance.hero3 = TeamInfoManager.instance.hero1;
                TeamInfoManager.instance.summon3_1 = TeamInfoManager.instance.summon1_1;
                TeamInfoManager.instance.summon3_2 = TeamInfoManager.instance.summon1_2;
                TeamInfoManager.instance.spell3_1 = TeamInfoManager.instance.spell1_1;
                TeamInfoManager.instance.spell3_2 = TeamInfoManager.instance.spell1_2;
                TeamInfoManager.instance.spell3_3 = TeamInfoManager.instance.spell1_3;
                TeamInfoManager.instance.spell3_4 = TeamInfoManager.instance.spell1_4;
                TeamInfoManager.instance.spell3_5 = TeamInfoManager.instance.spell1_5;
                TeamInfoManager.instance.spell3_6 = TeamInfoManager.instance.spell1_6;

                TeamInfoManager.instance.hero1 = tempHero;
                TeamInfoManager.instance.summon1_1 = tempSummon1;
                TeamInfoManager.instance.summon1_2 = tempSummon2;
                TeamInfoManager.instance.spell1_1 = tempSpell1;
                TeamInfoManager.instance.spell1_2 = tempSpell2;
                TeamInfoManager.instance.spell1_3 = tempSpell3;
                TeamInfoManager.instance.spell1_4 = tempSpell4;
                TeamInfoManager.instance.spell1_5 = tempSpell5;
                TeamInfoManager.instance.spell1_6 = tempSpell6;
            }
        }
    }

}
