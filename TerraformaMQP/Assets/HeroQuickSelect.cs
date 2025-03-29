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

    public void heroCheck(int num, string changingToHero) {

        if(num == 1) {
            if(ti.hero2 == changingToHero) {
                string tempHero = ti.hero2;
                string tempSummon1 = ti.summon2_1;
                string tempSummon2 = ti.summon2_2;
                string tempSpell1 = ti.spell2_1;
                string tempSpell2 = ti.spell2_2;
                string tempSpell3 = ti.spell2_3;
                string tempSpell4 = ti.spell2_4;
                string tempSpell5 = ti.spell2_5;
                string tempSpell6 = ti.spell2_6;

                



            }
        }

    }

}
