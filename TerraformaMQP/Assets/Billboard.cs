using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Billboard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData data) 
    {
        uiHover = true;
    }

    public void OnPointerExit(PointerEventData data) {
        uiHover = false;
    }
    
}
