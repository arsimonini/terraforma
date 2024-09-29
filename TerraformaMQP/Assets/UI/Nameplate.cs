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


    public void displayName(string name) {
        charName.text = name;
    }

    public void displayImage(Sprite sp) {
        pic.sprite = sp;
    }

    public void displayHealth(stat newhealth, stat newmaxhealth) {
        //health.value = dummy;
        //health.maxValue = newmaxhealth;
    }

        public void displayMana(stat newmana, stat newmaxmana) {
        //mana.value = newmana;
        //mana.maxValue = newmaxmana;
    }


}
