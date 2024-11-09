using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform cam;
    public bool uiHover = false;
    void Start() {
        uiHover = false;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);   
    }

    void OnMouseEnter() 
    {
        uiHover = true;
    }

    void OnMouseExit() {
        uiHover = false;
    }
    
}
