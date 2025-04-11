using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class setCooldownDisplay : MonoBehaviour
{

    public TextMeshProUGUI text = null;
    public void setDisplay(int coolVal){
        text.text = "   Cooldown: " + coolVal;
    }
}
