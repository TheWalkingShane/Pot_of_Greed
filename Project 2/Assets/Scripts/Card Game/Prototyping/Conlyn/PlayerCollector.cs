using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public string cardTag = "Card";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(cardTag))
        {
            return;
        }
        Debug.Log("Collecting Card");

        CardInventory.instance.testCounter++;
        
        Debug.Log("Collected " + CardInventory.instance.testCounter + " cards");
        
        Destroy(other.gameObject);
    }
}
