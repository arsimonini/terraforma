using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Controller : MonoBehaviour
{
    public bool playerTurn;
    public int roundCounter;
    // Start is called before the first frame update
    void Start()
    {
        playerTurn = true;  
        roundCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn)
        {

        }
        else
        {

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerTurn)
            {
                playerTurn = false;
                UnityEngine.Debug.Log("Entering Enemy Turn");
            }
            else
            {
                playerTurn = true;
                roundCounter++;
                UnityEngine.Debug.Log("Entering Player Turn");
                UnityEngine.Debug.Log("Entering Round: " + roundCounter);
            }
        }
    }
}
