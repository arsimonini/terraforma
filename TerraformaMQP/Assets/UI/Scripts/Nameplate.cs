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

    public void displayName(string name) {
        charName.text = name;
    }

    public void displayImage(Sprite sp) {
        UnityEngine.Debug.Log("Setting Sprite");
        pic.sprite = sp;
    }

    public void displayHealth(int newhealth, stat newmaxhealth) {
        health.maxValue = (float)newmaxhealth.moddedValue;
        health.value = (float)newhealth;
    }

    public void displayMana(int newmana, stat newmaxmana) {
        mana.maxValue = newmaxmana.moddedValue;
        mana.value = newmana;
    }

    public void displayAtk(stat st) {
        atk.text = st.moddedValue.ToString();
    }

    public void displayDef(stat st) {
        def.text = st.moddedValue.ToString();
    }

    public void displayMag(stat st) {
        mag.text = st.moddedValue.ToString();
    }

    public void displayRes(stat st) {
        res.text = st.moddedValue.ToString();
    }

    public void displaySpd(stat st) {
        spd.text = st.moddedValue.ToString();
    }

    public void displayCrit(stat st) {
        crit.text = st.moddedValue.ToString();
    }

    public void displayAcc(stat st) {
        acc.text = st.moddedValue.ToString();
    }

    public void displayMagicArea(bool b) {
        if(b == true) {
            magicAreaObj.SetActive(true);
        }
        else if(b == false) {
            magicAreaObj.SetActive(false);
        }
    }

}
