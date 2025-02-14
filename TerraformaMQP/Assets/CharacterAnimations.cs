using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    public Basic_Character_Class bcc;
    public Animator ani;
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
        

        if(bcc.isMoving == true && bcc.hasWalked == true) {
            ani.SetBool("charIsMoving", true);
            checkDirection();
        }
        else {
            ani.SetBool("charIsMoving", false);
            ani.SetBool("charIsMovingBack", false); //back
            ani.SetBool("charIsMovingForward", false);
            ani.SetBool("charIsMovingSideways", false);
        }
    }

    public void checkDirection() {
        
        x = t.position.x;
        z = t.position.z;

        if(camRot == 0) {
            if(startX > x) {
                ani.SetBool("charIsMovingSideways", true); //Left
                ani.SetBool("charIsMovingBack", false);
                ani.SetBool("charIsMovingForward", false);
            }
            else if(startX < x) {
                ani.SetBool("charIsMovingSideways", true); //Right
                ani.SetBool("charIsMovingBack", false);
                ani.SetBool("charIsMovingForward", false);
            }
            else if(startZ > z) {
                ani.SetBool("charIsMovingBack", true); //back
                ani.SetBool("charIsMovingForward", false);
                ani.SetBool("charIsMovingSideways", false);
            }
            else if(startZ < z) {
                ani.SetBool("charIsMovingForward", true); //forward
                ani.SetBool("charIsMovingBack", false);
                ani.SetBool("charIsMovingSideways", false);
            }
            else if(startX == x) {
                //UnityEngine.Debug.Log("This system won't work");

            }
    
        }
        else if(camRot == 90) {
            if(startX > x) {
                UnityEngine.Debug.Log("Cam at 0, They go Left");

            }
            else if(startX < x) {
                UnityEngine.Debug.Log("Cam at 0, They go Right");
            }
            else if(startZ > z) {
                UnityEngine.Debug.Log("Cam at 0, They go Down");
            }
            else if(startZ < z) {
                UnityEngine.Debug.Log("Cam at 0, They go Up");
            }
            else if(startX == x) {
                UnityEngine.Debug.Log("This system won't work");
            }
        }
        else if(camRot == 180) {
                        if(startX > x) {
                UnityEngine.Debug.Log("Cam at 0, They go Left");
            }
            else if(startX < x) {
                UnityEngine.Debug.Log("Cam at 0, They go Right");
            }
            else if(startZ > z) {
                UnityEngine.Debug.Log("Cam at 0, They go Down");
            }
            else if(startZ < z) {
                UnityEngine.Debug.Log("Cam at 0, They go Up");
            }
            else if(startX == x) {
                UnityEngine.Debug.Log("This system won't work");
            }
        }
        else if(camRot == 270) {
                        if(startX > x) {
                UnityEngine.Debug.Log("Cam at 0, They go Left");
            }
            else if(startX < x) {
                UnityEngine.Debug.Log("Cam at 0, They go Right");
            }
            else if(startZ > z) {
                UnityEngine.Debug.Log("Cam at 0, They go Down");
            }
            else if(startZ < z) {
                UnityEngine.Debug.Log("Cam at 0, They go Up");
            }
            else if(startX == x) {
                UnityEngine.Debug.Log("This system won't work");
            }
        }

        startX = x;
        startZ = z;

    }
}
