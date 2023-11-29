using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Assuming the player has a tag of "Player"
        if (other.CompareTag("Player"))
        {
          //  Debug.Log("Collected Coin!");
            // Tell the GameManager to increment the collected coin count
            GameManager.Instance.CollectCoin();

            // Optionally deactivate this coin so it can't be collected again
            gameObject.SetActive(false);
        }
    }
}

