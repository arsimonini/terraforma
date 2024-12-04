using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    public Camera mainCamera;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = mainCamera.gameObject.transform.position;
        this.gameObject.transform.rotation = mainCamera.gameObject.transform.rotation;
    }
}
