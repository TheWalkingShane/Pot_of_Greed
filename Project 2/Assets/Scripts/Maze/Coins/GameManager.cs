using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI coinCounterText; // UI Text to display the coin counter.
    public TextMeshProUGUI cardCounterText; // UI Text to display the card counter.
    
    
    private int totalCoinsActive;
    private int totalCardsActive;
    
    private int coinsCollected;
    private int cardsCollected;


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
        totalCardsActive = FindObjectsOfType<CoinVisuals>().Length;
        UpdateCoinCounterText();
        UpdateCardCounterText();
    }
    
    
    
    //===================================================================
    public void CollectCoin()
    {
        coinsCollected++;
       // Debug.Log("Current Coin Count" + coinsCollected);
        UpdateCoinCounterText();
    }
    public void CollectCard()
    {
        cardsCollected++;
        //Debug.Log("Current Card Count" + cardsCollected);
        UpdateCardCounterText();
    }
    //===================================================================
    
    
    //===================================================================
    public void UpdateCoinCounterText()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = $"Coins {coinsCollected}/{totalCoinsActive}";
        }
    }
    public void UpdateCardCounterText()
    {
        if (cardCounterText != null)
        {
            cardCounterText.text = $"Cards {cardsCollected}/{totalCardsActive}";
        }
    }
    //===================================================================
    
    
    //===================================================================
    // Call this method when coins are toggled on/off to update the count.
    public void UpdateTotalActiveCoins(int change)
    {
        totalCoinsActive += change;
        UpdateCoinCounterText();
    }
    
    // Call this method when coins are toggled on/off to update the count.
    public void UpdateTotalActiveCards(int change)
    {
        totalCardsActive += change;
        UpdateCardCounterText();
    }
    //===================================================================
    
    
    //===================================================================
    public void SetTotalActiveCoins(int total)
    {
        totalCoinsActive = total;
        UpdateCoinCounterText();
    }
    public void SetTotalActiveCards(int total)
    {
        totalCardsActive = total;
        UpdateCardCounterText();
    }
    //===================================================================
}

