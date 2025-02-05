using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeCharMenu : MonoBehaviour
{

    public int menuMode = 0;
    public GameObject p0;
    public GameObject p1;
    public Transform currPos = null;
    public Transform parent;
    public GameObject spellQCAsset;
    public GameObject SPQC = null;

    public GameObject summonQCAsset;
    public GameObject SMQC = null;

    public GameObject ti;

    // Start is called before the first frame update
    void Start()
    {
        //ti.SetTeam();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            if(SPQC != null) {
                Destroy(SPQC);
            }
            if(SMQC != null) {
                Destroy(SMQC);
            }
            if(menuMode == 1) {
                menuMode = 0;
                p1.SetActive(false);
                p0.SetActive(true);
            }
      
        }
    }

    public void summonQuickChange(int slot) {
        //Get character in hero slot
        //Make the menu appear
        if(SMQC != null) {
            Destroy(SMQC);
        }
        if(SPQC != null) {
            Destroy(SPQC);
        }

        SPQC = Instantiate(summonQCAsset, new Vector3(currPos.position.x, currPos.position.y - 40, currPos.position.z), Quaternion.identity, parent);
        //fill in the blanks for the menu
    }

    public void spellQuickChange(int slot) {
        //Get character in hero slot
        //Make the menu appear
        if(SPQC != null) {
            Destroy(SPQC);
        }
        if(SMQC != null) {
            Destroy(SMQC);
        }

        SPQC = Instantiate(spellQCAsset, new Vector3(currPos.position.x, currPos.position.y - 25, currPos.position.z), Quaternion.identity, parent);
        
        //fill in the blanks for the menu        
    }

    public void heroSlowChange(int slot) {
        //Open Second Menu
        p0.SetActive(false);
        menuMode = 1;
        p1.SetActive(true);
        //fill in the blanks for the menu
    }

    public void setCurrPos(Transform t) {
        currPos = t;
    }

    public void setTeam() {
        //
    }
}
