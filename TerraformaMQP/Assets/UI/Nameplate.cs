using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nameplate : MonoBehaviour
{
    [SerializeField] 
    public Text charName;
    public Slider health;
    public Slider mana;

    public void displayName(string name) {
        charName.text = name;
    }

    public void displayHealth(int newhealth) {
        health.value = newhealth;
    }

        public void displayMana(int newmana) {
        mana.value = newmana;
    }

}
