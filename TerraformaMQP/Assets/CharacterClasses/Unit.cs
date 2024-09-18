using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public int tileX = 0;
    public int tileY = 0;
    public TileType tile;
    public TileMap map;

    public bool charSelected = false;
    public bool charHover  = false;

    public List<Node> path = null;

    public Camera camera;
    public Renderer renderer;


    public void SetTile(int x, int y) {

    }

    //void Start() {
    //     renderer = GetComponent<Renderer>();
    //}

    void Update() {
        /*
        if(charHover) {
            if(Input.GetMouseButtonDown(0)) {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                //Debug.Log("Check 1");

                if(Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    //Debug.Log("Check 2");
                    if(hitInfo.collider.gameObject.GetComponent<Unit>());
                    {
                        charSelected = true;
                        map.selectedUnit = this.gameObject;
                        Debug.Log("Clicked on Unit");

                    }
                }
            }
        }
        */

        if(Input.GetKeyUp("escape") && charSelected == true) {
            charSelected = false;
            UnityEngine.Debug.Log("Character UnSelected");
        }
    }

    //Recolors when mouse is hovering over a unit
    public void mouseEnter() {
        if (charSelected == false)
        {
            renderer.material.color = Color.blue;
        }
        UnityEngine.Debug.Log("Mouse Entered");
        charHover = true;
    }

    //Resets when mouse has stopped hovering over a unit
    public void mouseExit() {
        if (charSelected == false) 
        {
            renderer.material.color = Color.white;
        }
        charHover = false;
        UnityEngine.Debug.Log("Mouse Exited");
    }

}
