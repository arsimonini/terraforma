using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreAttackMenu : MonoBehaviour
{

    public GameObject heroNameplate;
    public GameObject enemyNameplate;
    public GameObject preattack;

    public TextMeshProUGUI playerAcc;
    public TextMeshProUGUI playerCrit; 
    public TextMeshProUGUI playerDam;

    public TextMeshProUGUI enemyAcc;
    public TextMeshProUGUI enemyCrit; 
    public TextMeshProUGUI enemyDam;

    // Update is called once per frame
    void Update()
    {
        if(heroNameplate.activeSelf==true && enemyNameplate.activeSelf==true)
        {
            preattack.SetActive(true);
            calculateStats();
        }
        else 
        {
            preattack.SetActive(false);
        }
        
    }

    public void calculateStats() {
        //Crit
        calculateCrit(heroNameplate, playerCrit);
        calculateCrit(enemyNameplate, enemyCrit);

        //Acc
        calculateAcc(heroNameplate, playerAcc);
        calculateAcc(enemyNameplate, enemyAcc);

        calculateDam(heroNameplate, enemyNameplate, playerDam);
        calculateDam(enemyNameplate, heroNameplate, enemyDam);

    }

    public void calculateAcc(GameObject go, TextMeshProUGUI tmpGUI) {
        
        if(go.GetComponent<Nameplate>() == null) {
            return;
        }

        int accVal = System.Convert.ToInt32(go.GetComponent<Nameplate>().acc.text);
        int accBuffVal;
        if(go.GetComponent<Nameplate>().accBuff.text.Length == 0) {
            accBuffVal = 0;
        }
        else {
            accBuffVal = System.Convert.ToInt32(go.GetComponent<Nameplate>().accBuff.text.Substring(1,go.GetComponent<Nameplate>().accBuff.text.Length));
        }
        
        int spdVal = System.Convert.ToInt32(go.GetComponent<Nameplate>().spd.text);
        int spdBuffVal;
        if(go.GetComponent<Nameplate>().spdBuff.text.Length == 0) {
            spdBuffVal = 0;
        }
        else {
            spdBuffVal = System.Convert.ToInt32(go.GetComponent<Nameplate>().spdBuff.text.Substring(1,go.GetComponent<Nameplate>().spdBuff.text.Length));
        }

        float finalVal = 75+3*(((float)accVal + (float)accBuffVal) - ((float)spdVal + (float)spdBuffVal));

        tmpGUI.text = finalVal.ToString() + "%";
    }

    public void calculateCrit(GameObject go, TextMeshProUGUI tmpGUI) {

        if(go.GetComponent<Nameplate>() == null) {
            return;
        }

        int val = System.Convert.ToInt32(go.GetComponent<Nameplate>().crit.text);
        int buffVal;
        if(go.GetComponent<Nameplate>().critBuff.text.Length == 0) {
            buffVal = 0;
        }
        else {
            buffVal = System.Convert.ToInt32(go.GetComponent<Nameplate>().critBuff.text.Substring(1,go.GetComponent<Nameplate>().critBuff.text.Length));
        }
        
        float finalVal = ((float)val + (float)buffVal) / 20.0f * 100;

        tmpGUI.text = finalVal.ToString() + "%";
    }

    public void calculateDam(GameObject primaryGo, GameObject secondaryGo, TextMeshProUGUI primaryTMPGUI) {

        if(primaryGo.GetComponent<Nameplate>() == null) {
            return;
        }
        if(secondaryGo.GetComponent<Nameplate>() == null) {
            return;
        }

        //Attack
        int attack = System.Convert.ToInt32(primaryGo.GetComponent<Nameplate>().atk.text);
        int attackbuff;
        if(primaryGo.GetComponent<Nameplate>().atkBuff.text.Length == 0) {
            attackbuff = 0;
        }
        else {
            attackbuff = System.Convert.ToInt32(primaryGo.GetComponent<Nameplate>().atkBuff.text.Substring(1,primaryGo.GetComponent<Nameplate>().atkBuff.text.Length));
        }

        //Defense
        int defense = System.Convert.ToInt32(secondaryGo.GetComponent<Nameplate>().def.text);
        int defensebuff;
        if(secondaryGo.GetComponent<Nameplate>().defBuff.text.Length == 0) {
            defensebuff = 0;
        }
        else {
            defensebuff = System.Convert.ToInt32(secondaryGo.GetComponent<Nameplate>().defBuff.text.Substring(1,secondaryGo.GetComponent<Nameplate>().defBuff.text.Length));
        }

        int finalAttack = attack + attackbuff;
        int finalDefense = defense + defensebuff;
        

        float mitigatedDamage = Mathf.Round((float)finalAttack * (20f/(20f + (float)finalDefense)));
        
        
        primaryTMPGUI.text = mitigatedDamage.ToString();
    }


}
