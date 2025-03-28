using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharMenuMode1 : MonoBehaviour
{

    public GameObject go;

    public string currHero;
    public string currSummon1;
    public string currSummon2;
    public string currSpell1;
    public string currSpell2;
    public string currSpell3;
    public string currSpell4;
    public string currSpell5;
    public string currSpell6;

    public GameObject[] menuButtonSprites;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setCurrHeroSwap();
        swapAllSpellSprites();
        swapAllSummonSprites();
        
        
        
    }

    public void setCurrHeroSwap() {
        if(go.GetComponent<ChangeCharMenu>().currChanging == "Hero1") {
            currHero = go.GetComponent<ChangeCharMenu>().ti.hero1;
            currSummon1 = go.GetComponent<ChangeCharMenu>().ti.summon1_1;
            currSummon2 = go.GetComponent<ChangeCharMenu>().ti.summon1_2;
            currSpell1 = go.GetComponent<ChangeCharMenu>().ti.spell1_1;
            currSpell2 = go.GetComponent<ChangeCharMenu>().ti.spell1_2;
            currSpell3 = go.GetComponent<ChangeCharMenu>().ti.spell1_3;
            currSpell4 = go.GetComponent<ChangeCharMenu>().ti.spell1_4;
            currSpell5 = go.GetComponent<ChangeCharMenu>().ti.spell1_5;
            currSpell6 = go.GetComponent<ChangeCharMenu>().ti.spell1_6;


        }
        else if(go.GetComponent<ChangeCharMenu>().currChanging == "Hero2") {
            currHero = go.GetComponent<ChangeCharMenu>().ti.hero2;
            currSummon1 = go.GetComponent<ChangeCharMenu>().ti.summon2_1;
            currSummon2 = go.GetComponent<ChangeCharMenu>().ti.summon2_2;
            currSpell1 = go.GetComponent<ChangeCharMenu>().ti.spell2_1;
            currSpell2 = go.GetComponent<ChangeCharMenu>().ti.spell2_2;
            currSpell3 = go.GetComponent<ChangeCharMenu>().ti.spell2_3;
            currSpell4 = go.GetComponent<ChangeCharMenu>().ti.spell2_4;
            currSpell5 = go.GetComponent<ChangeCharMenu>().ti.spell2_5;
            currSpell6 = go.GetComponent<ChangeCharMenu>().ti.spell2_6;
        }
        else if(go.GetComponent<ChangeCharMenu>().currChanging == "Hero3") {
            currHero = go.GetComponent<ChangeCharMenu>().ti.hero3;
            currSummon1 = go.GetComponent<ChangeCharMenu>().ti.summon3_1;
            currSummon2 = go.GetComponent<ChangeCharMenu>().ti.summon3_2;
            currSpell1 = go.GetComponent<ChangeCharMenu>().ti.spell3_1;
            currSpell2 = go.GetComponent<ChangeCharMenu>().ti.spell3_2;
            currSpell3 = go.GetComponent<ChangeCharMenu>().ti.spell3_3;
            currSpell4 = go.GetComponent<ChangeCharMenu>().ti.spell3_4;
            currSpell5 = go.GetComponent<ChangeCharMenu>().ti.spell3_5;
            currSpell6 = go.GetComponent<ChangeCharMenu>().ti.spell3_6;
        }
    }

    public void swapAllSpellSprites() {
        swapSpellSprite(3, currSpell1);
        swapSpellSprite(4, currSpell2);
        swapSpellSprite(5, currSpell3);
        swapSpellSprite(6, currSpell4);
        swapSpellSprite(7, currSpell5);
        swapSpellSprite(8, currSpell6);
    }

    public void swapAllSummonSprites() {
        swapSummonSprite(1, currSummon1);
        swapSummonSprite(2, currSummon2);
    }

    public void swapSpellSprite(int index, string spell) {
        for(int i = 0; i < go.GetComponent<ChangeCharMenu>().spellSpriteKeys.Length; i++) {
            if(spell == go.GetComponent<ChangeCharMenu>().spellSpriteKeys[i]) {
                //go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
                menuButtonSprites[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
            }
        }

    }

    public void swapSummonSprite(int index, string summon) {
        for(int i = 0; i < go.GetComponent<ChangeCharMenu>().summonSpriteKeys.Length; i++) {
            if(summon == go.GetComponent<ChangeCharMenu>().summonSpriteKeys[i]) {
                //go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
                menuButtonSprites[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().summonSprites[i];
            }
        }

    }
}
