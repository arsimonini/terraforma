using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public GameObject prefab;
    public float timeTilDestroy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeTilDestroy = timeTilDestroy -= Time.deltaTime;
        if(timeTilDestroy <= 0) 
        {
            Destroy(prefab);
        }
    }
}
