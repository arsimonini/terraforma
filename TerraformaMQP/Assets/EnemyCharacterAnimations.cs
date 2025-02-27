using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterAnimations : MonoBehaviour
{
    public Basic_Character_Class bcc;
    //public Animator ani;
    public Animator a;
    public Transform t;
    public float camRot;
    public float startX;
    public float startZ;
    public float x;
    public float z;
    

    void Start() {
        startX = t.position.x;
        startZ = t.position.z;
    }
    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Q)) {
        //     camRot = camRot - 45;
        // }
        // if(Input.GetKeyDown(KeyCode.E)) {
        //     camRot = camRot + 45;
        // }

        camRot = (int)(transform.rotation.eulerAngles.y);
        

        if(bcc.isMoving == true) {
            a.SetBool("charIsMoving", true);
            checkDirection();
        }
        else {
            a.SetBool("charIsMoving", false);
            a.SetBool("charIsMovingBack", false); //back
            a.SetBool("charIsMovingForward", false);
            a.SetBool("charIsMovingSideways", false);
            t.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
        }
    }

    public void checkDirection() {
        
        x = t.position.x;
        z = t.position.z;

        if(camRot == 0) {
            if(startX > x) {
                //UnityEngine.Debug.Log("X should be more than x: " + startX + "  " + x);
                t.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                a.SetBool("charIsMovingSideways", true); //Left
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingForward", false);
            }
            else if(startX < x) {
                //UnityEngine.Debug.Log("X should be less than x: " + startX + "  " + x);
                t.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
                a.SetBool("charIsMovingSideways", true); //Right
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingForward", false);
            }
            else if(startZ > z) {
                //UnityEngine.Debug.Log("Z should be less than z: " + startZ + "  " + z);
                a.SetBool("charIsMovingForward", true); //forward
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingSideways", false);
            }
            else if(startZ < z) {
                //UnityEngine.Debug.Log("Z should be more than z: " + startZ + "  " + z);
                a.SetBool("charIsMovingBack", true); //back
                a.SetBool("charIsMovingForward", false);
                a.SetBool("charIsMovingSideways", false);

            }
            else if(startX == x) {
                //UnityEngine.Debug.Log("This system won't work");

            }
    
        }
        else if(camRot == 90) {
            if(startX > x) {
                //X is getting Lower
                a.SetBool("charIsMovingForward", true); //forward
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingSideways", false);

            }
            else if(startX < x) {
                a.SetBool("charIsMovingBack", true); //back
                a.SetBool("charIsMovingForward", false);
                a.SetBool("charIsMovingSideways", false);
            }
            else if(startZ > z) {
                //Z is getting Lower
                t.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
                a.SetBool("charIsMovingSideways", true); //Right
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingForward", false);
                
            }
            else if(startZ < z) {
                t.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                a.SetBool("charIsMovingSideways", true); //Left
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingForward", false);
                
            }
            else if(startX == x) {
            }
        }
        else if(camRot == 180 || camRot == 179) {
            if(startX > x) {
                //X is getting Lower
                t.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
                a.SetBool("charIsMovingSideways", true); //Right
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingForward", false);

            }
            else if(startX < x) {
                t.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                a.SetBool("charIsMovingSideways", true); //Left
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingForward", false);
                
            }
            else if(startZ > z) {
                //Z is getting Lower
                a.SetBool("charIsMovingBack", true); //back
                a.SetBool("charIsMovingForward", false);
                a.SetBool("charIsMovingSideways", false);
                
            }
            else if(startZ < z) {
                //UnityEngine.Debug.Log("Z should be less than z: " + startZ + "  " + z);
                a.SetBool("charIsMovingForward", true); //forward
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingSideways", false);
                
            }
            else if(startX == x) {
                
            }
        }
        else if(camRot == 270 || camRot == 269) {
            if(startX > x) {
                //X is getting Lower
                a.SetBool("charIsMovingBack", true); //back
                a.SetBool("charIsMovingForward", false);
                a.SetBool("charIsMovingSideways", false);
                
            }
            else if(startX < x) {
                a.SetBool("charIsMovingForward", true); //forward
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingSideways", false);
                
            }
            else if(startZ > z) {
                //Z is getting Lower
                t.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                a.SetBool("charIsMovingSideways", true); //Left
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingForward", false);
                
            }
            else if(startZ < z) {
                t.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
                a.SetBool("charIsMovingSideways", true); //Right
                a.SetBool("charIsMovingBack", false);
                a.SetBool("charIsMovingForward", false);
                
            }
            else if(startX == x) {
                
            }
        }

        startX = x;
        startZ = z;

    }
}
