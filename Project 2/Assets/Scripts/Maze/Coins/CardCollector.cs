using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Assuming the player has a tag of "Player"
        if (other.CompareTag("Player"))
        {
           // Debug.Log("Collected Card!");
            // Tell the GameManager to increment the collected card count
            GameManager.Instance.CollectCard();

            // Optionally deactivate this coin so it can't be collected again
            gameObject.SetActive(false);
        }
    }
}