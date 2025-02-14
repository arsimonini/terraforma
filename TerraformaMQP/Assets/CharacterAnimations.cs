using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    public Basic_Character_Class bcc;
    public Animator ani;
    

    // Update is called once per frame
    void Update()
    {
        if(bcc.isMoving == true && bcc.hasWalked == true) {
            ani.SetBool("charIsMoving", true);
        }
        else {
            ani.SetBool("charIsMoving", false);
        }
    }



}
