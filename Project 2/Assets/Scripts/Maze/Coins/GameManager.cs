using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshPro coinCounterText; // UI Text to display the coin counter.

    private int totalCoinsActive;
    private int coinsCollected;

    void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize your coins from the cells if necessary
        totalCoinsActive = FindObjectsOfType<CoinVisuals>().Length;
        UpdateCoinCounterText();
    }

    public void CollectCoin()
    {
        coinsCollected++;
        Debug.Log("Current Coin Count" + coinsCollected);
        UpdateCoinCounterText();
    }

    public void UpdateCoinCounterText()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = $"{coinsCollected}/{totalCoinsActive}";
        }
    }

    // Call this method when coins are toggled on/off to update the count.
    public void UpdateTotalActiveCoins(int change)
    {
        totalCoinsActive += change;
        UpdateCoinCounterText();
    }
    public void SetTotalActiveCoins(int total)
    {
        totalCoinsActive = total;
        UpdateCoinCounterText();
    }
}

