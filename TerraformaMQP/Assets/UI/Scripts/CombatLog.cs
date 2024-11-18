using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatLog : MonoBehaviour
{


    public GameObject extendedCombatLog;
    public GameObject shortenedCombatLog;

    public TextMeshProUGUI extendedCombatLogText;
    public TextMeshProUGUI shortenedCombatLogText;

    public float textWidth = 497.3172f;
    public float textHeight = 200f;

    public int combatLogMode = 0;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T) && combatLogMode == 0) {
            shortenedCombatLog.SetActive(true);
            combatLogMode = 1;

        } else if (Input.GetKeyUp(KeyCode.T) && combatLogMode == 1) {
            shortenedCombatLog.SetActive(false);
            extendedCombatLog.SetActive(true);
            combatLogMode = 2;
        } else if (Input.GetKeyUp(KeyCode.T) && combatLogMode == 2) {
            extendedCombatLog.SetActive(false);
            combatLogMode = 0;
        }



        if (Input.GetKeyUp(KeyCode.X)) {
            
            //extendedCombatLogText.text = extendedCombatLogText.text + "Testing the Chat Log \n";
            addText("Test Log");
            //shortenedCombatLogText.text = "Testing the Chat Log \n";
        }
    }

    public void addText(string s) {
        extendedCombatLogText.text = extendedCombatLogText.text + "\n" + s;
        RectTransform rtext = extendedCombatLogText.GetComponent<RectTransform>();
        RectTransform rpanel = extendedCombatLogText.GetComponent<RectTransform>();
        textHeight = textHeight + 16;
        if(s.Length >= 60) {
            textHeight = textHeight + 16;
        }
        rtext.sizeDelta = new Vector2(textWidth, textHeight);
        rpanel.sizeDelta = new Vector2(textWidth, textHeight);
        //extendedCombatLogText.GetComponent<RectTransform>().height = extendedCombatLogText.GetComponent<RectTransform>().height + 12;
        shortenedCombatLogText.text = s;
    }


 }
