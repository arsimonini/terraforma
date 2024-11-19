using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{

    public GameObject atkPrefab;
    private GameObject callToPrefab;
    public Transform pos;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.H)) {
            callToPrefab = Instantiate(atkPrefab, new Vector3(pos.position.x + 0.5f, pos.position.y , pos.position.z), pos.rotation);
            //callToPrefab.transform.Find("slash-effect_0").GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
