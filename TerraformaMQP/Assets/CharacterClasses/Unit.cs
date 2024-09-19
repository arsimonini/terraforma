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
                    if(hitInfo.collider.gameObject.GetComponent<Basic_Character_Class>());
                    {
                        charSelected = true;
                        map.selectedUnit = this.gameObject;
                        Debug.Log("Clicked on Unit");

                    }
                }
            }
        }
        */
        /*
        if(Input.GetKeyUp("escape") && charSelected == true) {
            charSelected = false;
            UnityEngine.Debug.Log("Character UnSelected");
        }
        */
    }





}
