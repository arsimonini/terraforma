using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharSummonButtons : MonoBehaviour
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

    public void summonButtonClicked(string s) {
        buttonClicked = s;
        swapSummon(go.GetComponent<ChangeCharMenu>().currChanging, buttonClicked);
        
        
        Destroy(thisGO);
    }

    public void swapSummonSprite(int index, string summon) {
        for(int i = 0; i < go.GetComponent<ChangeCharMenu>().summonSpriteKeys.Length; i++) {
            if(summon == go.GetComponent<ChangeCharMenu>().summonSpriteKeys[i]) {
                //go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
                go.GetComponent<ChangeCharMenu>().summonButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().summonSprites[i];
            }
        }

    }



    public void swapSummon(string place, string what) {
        if(what == "Locked") {
            return;
        }
        switch (place) {
            case "Hero1_Summon1":
            TeamInfoManager.instance.summon1_1 = what;
            swapSummonSprite(0, what);
            break;

            case "Hero1_Summon2":
            TeamInfoManager.instance.summon1_2 = what;
            swapSummonSprite(1, what);
            break;

            case "Hero2_Summon1":
            TeamInfoManager.instance.summon2_1 = what;
            swapSummonSprite(2, what);
            break;

            case "Hero2_Summon2":
            TeamInfoManager.instance.summon2_2 = what;
            swapSummonSprite(3, what);
            break;

            case "Hero3_Summon1":
            TeamInfoManager.instance.summon3_1 = what;
            swapSummonSprite(4, what);
            break;

            case "Hero3_Summon2":
            TeamInfoManager.instance.summon3_2 = what;
            swapSummonSprite(5, what);
            break;
            
        }
    }

}
