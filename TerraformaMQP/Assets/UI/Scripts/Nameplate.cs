using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Nameplate : MonoBehaviour
{
    [SerializeField] 

    public TextMeshProUGUI charName;
    public Slider health;
    public Slider mana;
    public Image pic;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI res; 
    public TextMeshProUGUI def;
    public TextMeshProUGUI spd;
    public TextMeshProUGUI crit;
    public TextMeshProUGUI acc;
    public TextMeshProUGUI mag;
    public GameObject magicAreaObj;

    public TextMeshProUGUI atkBuff;
    public TextMeshProUGUI resBuff; 
    public TextMeshProUGUI defBuff;
    public TextMeshProUGUI spdBuff;
    public TextMeshProUGUI critBuff;
    public TextMeshProUGUI accBuff;
    public TextMeshProUGUI magBuff;

    public TextMeshProUGUI healthVal;
    public TextMeshProUGUI manaVal;

    public GameObject[] effectsVisuals; //the effects that the characters have
    public List<NameplateEffect> effects; //The list of effects has the key, name, image, and more
    public int effectNum; //How many effects the character has
    public List<string> effectKeyNames; //all of the key names the effects have
    public GameObject extraEffectArea; //For cases where there are more than 6 effects
    public GameObject extraEffectButton; //The buttong for more effects
    public bool exEffectOn; //a bool to see if the extra effects area is open
    public bool nameplateIsActive; //whether the nameplate is active



    public void displayName(string name) {
        charName.text = name;
    }

    public void displayImage(Sprite sp) {
        //UnityEngine.Debug.Log("Setting Sprite");
        pic.sprite = sp;
    }


    //Display

    public void displayHealth(int newhealth, stat newmaxhealth) {
        health.maxValue = (float)newmaxhealth.moddedValue;
        health.value = (float)newhealth;
    }

    public void displayMana(int newmana, stat newmaxmana) {
        mana.maxValue = newmaxmana.moddedValue;
        mana.value = newmana;
    }

    public void displayHealthValue(int currHealth, stat maxHealth) {
        healthVal.text = currHealth.ToString() + "/" + maxHealth.moddedValue;
    }

    public void displayManaValue(int currMana, stat maxMana) {
        manaVal.text = currMana.ToString() + "/" + maxMana.moddedValue;
    }


    //Stats

    public void displayAtk(stat st) {
        atk.text = st.value.ToString();
    }

    public void displayDef(stat st) {
        def.text = st.value.ToString();
    }

    public void displayMag(stat st) {
        mag.text = st.value.ToString();
    }

    public void displayRes(stat st) {
        res.text = st.value.ToString();
    }

    public void displaySpd(stat st) {
        spd.text = st.value.ToString();
    }

    public void displayCrit(stat st) {
        crit.text = st.value.ToString();
    }

    public void displayAcc(stat st) {
        acc.text = st.value.ToString();
    }

    public void displayMagicArea(bool b) {
        if(b == true) {
            magicAreaObj.SetActive(true);
        }
        else if(b == false) {
            magicAreaObj.SetActive(false);
        }
    }

    public void displayMagBuff(stat st) {
        
        if((st.moddedValue - st.value) == 0) {
            magBuff.text = "";
        }
        else if((st.moddedValue - st.value) > 0) {
            magBuff.color = Color.blue;
            magBuff.text = "+" + (st.moddedValue - st.value).ToString();
        }
        else if((st.moddedValue - st.value) < 0) {
            magBuff.color = Color.red;
            magBuff.text = (st.moddedValue - st.value).ToString();
        }
    }

    public void displayAtkBuff(stat st) {
        
        if((st.moddedValue - st.value) == 0) {
            atkBuff.text = "";
        }
        else if((st.moddedValue - st.value) > 0) {
            atkBuff.color = Color.blue;
            atkBuff.text = "+" + (st.moddedValue - st.value).ToString();
        }
        else if((st.moddedValue - st.value) < 0) {
            atkBuff.color = Color.red;
            atkBuff.text = (st.moddedValue - st.value).ToString();
        }
    }  
    

    public void displayDefBuff(stat st) {
        
        if((st.moddedValue - st.value) == 0) {
            defBuff.text = "";
        }
        else if((st.moddedValue - st.value) > 0) {
            defBuff.color = Color.blue;
            defBuff.text = "+" + (st.moddedValue - st.value).ToString();
        }
        else if((st.moddedValue - st.value) < 0) {
            defBuff.color = Color.red;
            defBuff.text = (st.moddedValue - st.value).ToString();
        }
        
    }

    public void displayResBuff(stat st) {
        
        if((st.moddedValue - st.value) == 0) {
            resBuff.text = "";
        }
        else if((st.moddedValue - st.value) > 0) {
            resBuff.color = Color.blue;
            resBuff.text = "+" + (st.moddedValue - st.value).ToString();
        }
        else if((st.moddedValue - st.value) < 0) {
            resBuff.color = Color.red;
            resBuff.text = (st.moddedValue - st.value).ToString();
        }  
    }

    public void displayCritBuff(stat st) {
        
        if((st.moddedValue - st.value) == 0) {
            critBuff.text = "";
        }
        else if((st.moddedValue - st.value) > 0) {
            critBuff.color = Color.blue;
            critBuff.text = "+" + (st.moddedValue - st.value).ToString();
        }
        else if((st.moddedValue - st.value) < 0) {
            critBuff.color = Color.red;
            critBuff.text = (st.moddedValue - st.value).ToString();
        }
    }

    public void displayAccBuff(stat st) {
        
        if((st.moddedValue - st.value) == 0) {
            accBuff.text = "";
        }
        else if((st.moddedValue - st.value) > 0) {
            accBuff.color = Color.blue;
            accBuff.text = "+" + (st.moddedValue - st.value).ToString();
        }
        else if((st.moddedValue - st.value) < 0) {
            accBuff.color = Color.red;
            accBuff.text = (st.moddedValue - st.value).ToString();
        }   
    }

    public void displaySpdBuff(stat st) {
        
        if((st.moddedValue - st.value) == 0) {
            spdBuff.text = "";
        }
        else if((st.moddedValue - st.value) > 0) {
            spdBuff.color = Color.blue;
            spdBuff.text = "+" + (st.moddedValue - st.value).ToString();
        }
        else if((st.moddedValue - st.value) < 0) {
            spdBuff.color = Color.red;
            spdBuff.text = (st.moddedValue - st.value).ToString();
        }  
    }

    public void getTileNum(List<BuffClass> buffs, string tileName, ClickableTile tile, bool b) {
        //Resets Effect Num on call
        effectNum = 0;
        effectKeyNames.Clear();


        //Tile Type TODO Need to figure out a faster way for this
        if(tileName.Contains("Grass")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("Grass");
        }
        else if(tileName.Contains("Dirt")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("Dirt");
        }
        else if(tileName.Contains("Ashen")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("Ashen");
        }
        else if(tileName.Contains("Mud")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("Mud");
        }
        else if(tileName.Contains("Sand")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("Sand");
        }
        else if(tileName.Contains("Wood")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("Wood");
        }
        else if(tileName.Contains("Metal")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("Metal");
        }
        else if(tileName.Contains("LightForest")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("LForest");
        }
        else if(tileName.Contains("DenseForest")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("DForest");
        }        
        else if(tileName.Contains("ShallowWater")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("SWater");
        }        
        else if(tileName.Contains("DeepWater")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("DWater");
        }
        else if(tileName.Contains("Ice")) 
        {
            effectNum = effectNum + 1;
            effectKeyNames.Add("Ice");
        }

        //Tile Effect
        for(int i = 0; i < tile.effectsOnTile.Count; i++) {

            effectNum = effectNum + 1;
            effectKeyNames.Add(tile.effectsOnTile[i].name);
        }


        //Other Effects
        for(int i = 0; i < buffs.Count; i++) {
            //UnityEngine.Debug.Log(buffs[i].name);

            effectNum = effectNum + 1;
            effectKeyNames.Add(buffs[i].name);
        }

        //Display the effects
        displayEffect(b);
        exEffButtonNeeded(b);
        if(b == false) {
            openExtraEffects(b);
        }


        //UnityEngine.Debug.Log(effectKeyNames.length());

    }

    public void displayStatBlock(stat atk, stat def, stat res, stat acc, stat crit, stat spd) {
        displayAtk(atk);
        displayDef(def);
        displayRes(res);
        displayAcc(acc);
        displayCrit(crit);
        displaySpd(spd);

        displayAtkBuff(atk);
        displayDefBuff(def);
        displayResBuff(res);
        displayAccBuff(acc);
        displayCritBuff(crit);
        displaySpdBuff(spd);
    }

    public void displayEffect(bool b) {
        
        if(b == true) {
            for(int i = 0; i < effectNum; i++) {
                effectsVisuals[i].SetActive(true);

                for(int j = 0; j < effects.Count; j++) {
                    if(effects[j].key == effectKeyNames[i]) {
                        effectsVisuals[i].GetComponent<Image>().sprite = effects[j].effectVis;
                        //Figure out how to set up the timers
                        //effectsVisuals[i].GetComponent<Image>().sprite = effects[j].effectVis;
                    }
                }
            }
        }
        else if(b == false) {
            for(int i = 0; i < effectNum; i++) {
                effectsVisuals[i].SetActive(false);
            }
        }
    }

    public void exEffButtonNeeded(bool b) {
        if(b == true) {
            if(effectNum >= 7) {
                extraEffectButton.SetActive(true);
            }
            else {
                extraEffectButton.SetActive(false);
            }
        }
        else if(b == false) {
            extraEffectButton.SetActive(false);
        }
    }

    public void openExtraEffects(bool b) 
    {
        if(b == true) {
            extraEffectArea.SetActive(true);

            for(int i = 0; i < 21; i++) {
                effectsVisuals[i].SetActive(false);
            }
            for(int i = 0; i < effectNum; i++) {
                effectsVisuals[i].SetActive(true);
            }

            exEffectOn = true;
        }
        else if(b == false) {
            for(int i = 0; i < 21; i++) {
                effectsVisuals[i].SetActive(false);
            }
            
            extraEffectArea.SetActive(false);

            exEffectOn = false;

        }
    }

}
