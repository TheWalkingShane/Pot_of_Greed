using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
   // public static GameManager Instance { get; private set; }
    public static GameManager Instance;

    public TextMeshProUGUI coinCounterText; // UI Text to display the coin counter.
    public TextMeshProUGUI cardCounterText; // UI Text to display the card counter.

    public GameObject playerGO;
    public GameObject eventListenerGO;
    
    public Image img;
    public AnimationCurve curve;
    
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
            StartCoroutine(FadeIn());
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

    public void DestroyInstance()
    {
        Destroy(this);
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
    
    
    //===================================================================
    
                    //      [OBSERVER PATTERN]
    public void HandleReturnToMaze()
    {
        StartCoroutine(FadeBetween(true));

        // Re-enable state changes for the monster
        MonsterController.Instance.AllowStateChange();

        // Transition the monster back to RoamingState
        StartCoroutine(DelayedRoam());
    }
  
    public void ChangeToCaughtScene()
    {
        StartCoroutine(FadeBetween(false));
        
        // Handle any pre-scene change logic here
        // For example, set the monster to an idle state

        MonsterController.Instance.EnterIdleState();

        // Load the new scene where the player is caught
        //SceneManager.LoadScene("Test_Dale");
    }
    //===================================================================
    
    IEnumerator FadeIn()
    {
        eventListenerGO.SetActive(true);

        float t = 1f;

        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            
            img.color = new Color(0, 0, 0, a);
            
            yield return 0;
        }
    }
    
    public IEnumerator FadeToMenu()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            
            img.color = new Color(0, 0, 0, a);
            
            yield return 0;
        }
        eventListenerGO.SetActive(false);
        SceneManager.LoadScene("MainMenu");
        CardInventory.instance.DestroyInstance();
        Instance.DestroyInstance();
    }

    IEnumerator FadeBetween(bool comingFromCard)
    {
        eventListenerGO.SetActive(true);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            
            img.color = new Color(0, 0, 0, a);
            
            yield return 0;
        }

        eventListenerGO.SetActive(false);

        if (comingFromCard)
        {
            SceneManager.UnloadSceneAsync("CardsMain", UnloadSceneOptions.None);
            playerGO.SetActive(true);
        
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            SceneManager.LoadScene("CardsMain", LoadSceneMode.Additive);
            playerGO.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        t = 1f;

        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            
            img.color = new Color(0, 0, 0, a);
            
            yield return 0;
        }
    }

    IEnumerator DelayedRoam()
    {
        yield return new WaitForSeconds(5f);
        MonsterController.Instance.TransitionToRoaming();
    }
}

