using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{

    public GameObject atkPrefab;
    private GameObject callToPrefab;
    public Transform pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.H)) {
            callToPrefab = Instantiate(atkPrefab, new Vector3(pos.position.x + 0.5f, pos.position.y , pos.position.z), pos.rotation);
        }
    }
}
