using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoldSpecificAnimation : MonoBehaviour
{
    public Basic_Character_Class bcc;
    public Animator ani;
    public string currName;
    public bool isActive;

    // Update is called once per frame
    void Update()
    {
        if(bcc.buffs.Count == 0) {
            ani.SetBool("defenseMode", false);
        }
        else {
            for(int i = 0; i < bcc.buffs.Count; i++) {
                currName = bcc.buffs[i].name;
                if(bcc.buffs[i].name == "Rollout") {
                   ani.SetBool("defenseMode", true);
                   currName = "Rollout";
                   isActive = true;
                   return;
                }
                else {
                  ani.SetBool("defenseMode", false);
                  currName = "";
                  isActive = false;
            }
        }

        }
    }



}
