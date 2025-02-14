using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoldAbilityImageSwap : MonoBehaviour
{
    public GameObject go;
    public GameObject AM;
    public GameObject DM;
    public bool activated = false;

    // Start is called before the first frame update
    void Start()
    {


        
    }


    public void SwapImage()
    {

        for(int i = 0; i < go.GetComponent<Basic_Character_Class>().buffs.Count; i++) {
            if(go.GetComponent<Basic_Character_Class>().buffs[i].name == "Rollout") {
                go.GetComponent<Basic_Character_Class>().abilityPrefab = DM;
                activated = true;
            }
            else {
                go.GetComponent<Basic_Character_Class>().abilityPrefab = AM;
                activated = false;
            }
        }
        
    }
}
